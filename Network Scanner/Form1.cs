using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;

namespace Network_Scanner;

public partial class Form1 : Form
{
    private ConcurrentBag<(string IpAddress, string HostName, string PingReply, IPStatus Status, long RoundtripTime)> results;
    private CancellationTokenSource? _cts;
    private int _completed;
    private int _totalHosts;
    private int _onlineCount;
    private long _latencyTotal;
    private long _latencyMin;
    private long _latencyMax;

    public Form1()
    {
        InitializeComponent();

        // Initialize the concurrent bag to store results
        results = new ConcurrentBag<(string, string, string, IPStatus, long)>();

        // Initialize DataGridView columns
        InitializeDataGridView();

        // Load available network interfaces
        LoadNetworkInterfaces();

        // Configure TextBox input for IPv4 addresses
        textBox1.KeyPress += textBox1_KeyPress;
        textBox1.MaxLength = 15;

        // Attach ComboBox selection event
        comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

        buttonExport.Enabled = false;
        labelStatus.Text = "Ready";
        labelStats.Text = "Online: 0 | Avg: - | Min: - | Max: -";
        labelCurrentTarget.Text = "Current: Waiting...";
        DoubleBuffered = true;
        dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
    }

    private void InitializeDataGridView()
    {
        dataGridView1.Columns.Clear();
        dataGridView1.BorderStyle = BorderStyle.None;
        dataGridView1.BackgroundColor = Color.White;
        dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToResizeRows = false;
        dataGridView1.EnableHeadersVisualStyles = false;
        dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

        var headerStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(242, 242, 247),
            ForeColor = Color.FromArgb(29, 29, 31),
            Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold, GraphicsUnit.Point),
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };
        dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;
        dataGridView1.ColumnHeadersHeight = 32;

        dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(221, 235, 255);
        dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
        dataGridView1.GridColor = Color.FromArgb(229, 229, 234);

        dataGridView1.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(251, 251, 253)
        };
        dataGridView1.RowTemplate.Height = 28;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.MultiSelect = false;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.Columns.Add("IPAddress", "IP Address");
        dataGridView1.Columns.Add("HostName", "Host Name");
        dataGridView1.Columns.Add("PingReply", "Ping Reply");
        dataGridView1.Columns.Add("Status", "Status");
        dataGridView1.Columns.Add("RoundtripTime", "Roundtrip Time");
    }

    private void LoadNetworkInterfaces()
    {
        comboBox1.Items.Clear();

        // Regular expression for valid interface names (Ethernet, Ethernet 2, Wi-Fi, Wi-Fi 2, WiFi, WiFi 2, etc.)
        var validPattern = new Regex(@"^(Ethernet|Wi-Fi|WiFi)( \d+)?$", RegexOptions.IgnoreCase);

        foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (netInterface.OperationalStatus == OperationalStatus.Up)
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    string interfaceName = netInterface.Name;

                    if (validPattern.IsMatch(interfaceName))
                    {
                        comboBox1.Items.Add(interfaceName);
                    }
                }
            }
        }

        if (comboBox1.Items.Count > 0)
        {
            comboBox1.SelectedIndex = 0;
            SetTextBoxIpAddress();
        }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetTextBoxIpAddress();
    }

    private void SetTextBoxIpAddress()
    {
        if (comboBox1.SelectedItem != null)
        {
            string selectedInterface = comboBox1.SelectedItem.ToString();
            string ipBase = GetLocalNetworkBase(selectedInterface);
            textBox1.Text = ipBase; // Display the base IP ending in .1
        }
    }

    private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Allow digits, the period, and control keys (such as Backspace)
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        {
            e.Handled = true;
        }
    }

    private bool TryGetBaseAddress(string input, out string baseIp, out string errorMessage)
    {
        baseIp = "";
        errorMessage = "";

        if (!System.Net.IPAddress.TryParse(input, out System.Net.IPAddress ipAddress) ||
            ipAddress.AddressFamily != AddressFamily.InterNetwork)
        {
            errorMessage = "Please enter a valid IPv4 address.";
            return false;
        }

        var parts = input.Split('.');
        if (parts.Length != 4)
        {
            errorMessage = "Please enter a valid IPv4 address.";
            return false;
        }

        baseIp = $"{parts[0]}.{parts[1]}.{parts[2]}.";
        return true;
    }

    private string GetLocalNetworkBase(string selectedInterface)
    {
        foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (netInterface.Name == selectedInterface && netInterface.OperationalStatus == OperationalStatus.Up)
            {
                var properties = netInterface.GetIPProperties();
                foreach (var ip in properties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        var localIP = ip.Address.ToString();
                        if (!string.IsNullOrEmpty(localIP))
                        {
                            var parts = localIP.Split('.');
                            if (parts.Length == 4)
                            {
                                return $"{parts[0]}.{parts[1]}.{parts[2]}.1";
                            }
                        }
                    }
                }
            }
        }
        return "";
    }

    // Method to start scanning the network
    private async void button1_Click(object sender, EventArgs e)
    {
        if (_cts != null)
        {
            CancelScan();
            return;
        }

        if (!string.IsNullOrWhiteSpace(textBox1.Text))
        {
            string ipAddressInput = textBox1.Text.Trim();
            if (TryGetBaseAddress(ipAddressInput, out string baseIp, out string errorMessage))
            {
                int start = (int)startRangeUpDown.Value;
                int end = (int)endRangeUpDown.Value;
                if (start > end)
                {
                    MessageBox.Show("The start host value must be less than or equal to the end value.");
                    return;
                }

                dataGridView1.Rows.Clear();
                results = new ConcurrentBag<(string, string, string, IPStatus, long)>(); // Reset stored results
                _cts = new CancellationTokenSource();
                _completed = 0;
                _totalHosts = end - start + 1;
                ResetStats();
                progressBar1.Value = 0;
                progressBar1.Maximum = _totalHosts;
                SetScanningUiState(true);
                SetStatus($"Scanning {baseIp}{start}-{end}...");
                bool wasCancelled = false;
                var ipList = Enumerable.Range(start, end - start + 1)
                    .Select(i => $"{baseIp}{i}")
                    .ToList();

                bool resolveHosts = checkBoxResolveHosts.Checked;
                int timeoutMs = (int)numericTimeout.Value;

                try
                {
                    await PingNetworkAsync(ipList, resolveHosts, timeoutMs, _cts.Token);
                }
                catch (OperationCanceledException)
                {
                    wasCancelled = true;
                }
                finally
                {
                    wasCancelled |= _cts?.IsCancellationRequested == true;
                    progressBar1.Value = Math.Min(_completed, progressBar1.Maximum);
                    _cts?.Dispose();
                    _cts = null;
                    SetScanningUiState(false);
                    UpdateCurrentTarget("Finished");
                    UpdateStatsLabels();
                    SetStatus(wasCancelled
                        ? $"Cancelled at {_completed}/{_totalHosts} hosts; found {results.Count} responsive."
                        : $"Completed {_totalHosts} hosts; found {results.Count} responsive.");
                }
                return;
            }
            else
            {
                MessageBox.Show(errorMessage);
                return;
            }
        }
        MessageBox.Show("Please enter a valid IPv4 address.");
    }

    private void buttonExport_Click(object sender, EventArgs e)
    {
        if (!results.Any())
        {
            MessageBox.Show("No results to export yet.");
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = "network-scan.csv",
            Title = "Export scan results"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var orderedResults = results
                    .OrderBy(r => ConvertToUint(r.IpAddress))
                    .ToList();

                var builder = new StringBuilder();
                builder.AppendLine("IP Address,Host Name,Ping Reply,Status,Roundtrip Time (ms)");
                foreach (var entry in orderedResults)
                {
                    builder.AppendLine(string.Join(",",
                        entry.IpAddress,
                        EscapeCsv(entry.HostName),
                        entry.PingReply,
                        entry.Status,
                        entry.RoundtripTime));
                }

                File.WriteAllText(dialog.FileName, builder.ToString());
                MessageBox.Show($"Exported {orderedResults.Count} entries to {dialog.FileName}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to export results: {ex.Message}");
            }
        }
    }

    private void CancelScan()
    {
        if (_cts != null && !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }
    }

    private void SetScanningUiState(bool scanning)
    {
        comboBox1.Enabled = !scanning;
        textBox1.Enabled = !scanning;
        startRangeUpDown.Enabled = !scanning;
        endRangeUpDown.Enabled = !scanning;
        checkBoxResolveHosts.Enabled = !scanning;
        numericTimeout.Enabled = !scanning;
        buttonExport.Enabled = !scanning && results.Any();
        progressBar1.Visible = scanning;
        button1.Text = scanning ? "Cancel" : "Start";
    }

    private void SetStatus(string message)
    {
        labelStatus.Text = message;
    }

    private void ResetStats()
    {
        _onlineCount = 0;
        _latencyTotal = 0;
        _latencyMin = long.MaxValue;
        _latencyMax = 0;
        UpdateStatsLabels();
        UpdateCurrentTarget("Waiting...");
    }

    private void UpdateProgress(int completed)
    {
        if (_totalHosts <= 0)
            return;

        int clamped = Math.Min(completed, _totalHosts);
        progressBar1.Value = Math.Max(progressBar1.Minimum, clamped);
        int percent = (int)Math.Round(clamped * 100.0 / _totalHosts);
        SetStatus($"Scanning: {clamped}/{_totalHosts} ({percent}%) hosts; found {results.Count} responsive.");
        UpdateStatsLabels();
    }

    private void UpdateStatsLabels()
    {
        int online = _onlineCount;
        long total = Interlocked.Read(ref _latencyTotal);
        long min = Interlocked.Read(ref _latencyMin);
        long max = Interlocked.Read(ref _latencyMax);

        string minStr = min == long.MaxValue ? "-" : $"{min} ms";
        string maxStr = max == 0 && online == 0 ? "-" : $"{max} ms";
        string avgStr = online > 0 ? $"{total / online} ms" : "-";

        labelStats.Text = $"Online: {online} | Avg: {avgStr} | Min: {minStr} | Max: {maxStr}";
    }

    private void UpdateCurrentTarget(string text)
    {
        labelCurrentTarget.Text = $"Current: {text}";
    }

    private DataGridViewRow? GetSelectedRow()
    {
        if (dataGridView1.SelectedRows.Count > 0)
        {
            return dataGridView1.SelectedRows[0];
        }

        if (dataGridView1.CurrentRow != null)
        {
            return dataGridView1.CurrentRow;
        }

        return null;
    }

    private async Task PingNetworkAsync(List<string> ipList, bool resolveHosts, int timeoutMs, CancellationToken token)
    {
        var options = new ParallelOptions
        {
            CancellationToken = token,
            MaxDegreeOfParallelism = Math.Min(256, Environment.ProcessorCount * 4)
        };

        await Parallel.ForEachAsync(ipList, options, async (ip, ct) =>
        {
            await PingAddressAsync(ip, resolveHosts, timeoutMs, ct);
        });
    }

    private async Task PingAddressAsync(string ipString, bool resolveHosts, int timeoutMs, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return;
        IPStatus status = IPStatus.Unknown;
        long roundtripTime = 0;
        string hostName = "";

        try
        {
            using var ping = new Ping();
            PingReply pingReply = await ping.SendPingAsync(ipString, timeoutMs);
            status = pingReply.Status;
            roundtripTime = pingReply.RoundtripTime;

            if (pingReply.Status == IPStatus.Success)
            {
                this.Invoke((Action)(() => UpdateCurrentTarget(ipString)));
                hostName = "Unknown";
                if (resolveHosts)
                {
                    try
                    {
                        var host = await Dns.GetHostEntryAsync(ipString);
                        hostName = host.HostName;
                    }
                    catch
                    {
                        // Ignore error in case cant resolve host
                    }
                }

                var entry = (IpAddress: ipString,
                              HostName: hostName,
                              PingReply: pingReply.Address?.ToString() ?? ipString,
                              Status: pingReply.Status,
                              RoundtripTime: pingReply.RoundtripTime);
                results.Add(entry);
                TrackLatency(entry.RoundtripTime);
                this.Invoke((Action)(() =>
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = entry.IpAddress;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = entry.HostName;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = entry.PingReply;
                    dataGridView1.Rows[rowIndex].Cells[3].Value = entry.Status.ToString();
                    dataGridView1.Rows[rowIndex].Cells[4].Value = entry.RoundtripTime;
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 238, 255);
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(206, 224, 255);
                    UpdateStatsLabels();
                }));
            }
        }
        catch
        {
            // Ignore Ping exceptions
        }
        finally
        {
            int count = Interlocked.Increment(ref _completed);
            this.Invoke((Action)(() =>
            {
                UpdateProgress(count);
            }));
        }
    }

    private static string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        bool requiresQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
        if (!requiresQuotes)
            return value;

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }

    private static uint ConvertToUint(string ipString)
    {
        var address = System.Net.IPAddress.Parse(ipString).GetAddressBytes();
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(address);
        }

        return BitConverter.ToUInt32(address, 0);
    }

    private void TrackLatency(long rtt)
    {
        Interlocked.Increment(ref _onlineCount);
        Interlocked.Add(ref _latencyTotal, rtt);
        UpdateMin(ref _latencyMin, rtt);
        UpdateMax(ref _latencyMax, rtt);
    }

    private static void UpdateMin(ref long target, long candidate)
    {
        long current;
        do
        {
            current = Interlocked.Read(ref target);
            if (candidate >= current)
                return;
        } while (Interlocked.CompareExchange(ref target, candidate, current) != current);
    }

    private static void UpdateMax(ref long target, long candidate)
    {
        long current;
        do
        {
            current = Interlocked.Read(ref target);
            if (candidate <= current)
                return;
        } while (Interlocked.CompareExchange(ref target, candidate, current) != current);
    }

    private void copyIpMenuItem_Click(object sender, EventArgs e)
    {
        var row = GetSelectedRow();
        if (row == null)
            return;

        var ip = row.Cells[0].Value?.ToString();
        if (!string.IsNullOrEmpty(ip))
        {
            Clipboard.SetText(ip);
            SetStatus($"Copied {ip} to clipboard.");
        }
    }

    private void copyRowMenuItem_Click(object sender, EventArgs e)
    {
        var row = GetSelectedRow();
        if (row == null)
            return;

        var cells = row.Cells;
        string rowText = string.Join(",",
            cells[0].Value?.ToString() ?? "",
            cells[1].Value?.ToString() ?? "",
            cells[2].Value?.ToString() ?? "",
            cells[3].Value?.ToString() ?? "",
            cells[4].Value?.ToString() ?? "");

        Clipboard.SetText(rowText);
        SetStatus("Copied row to clipboard.");
    }

    private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        var ip = dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString();
        if (!string.IsNullOrEmpty(ip))
        {
            Clipboard.SetText(ip);
            SetStatus($"Copied {ip} to clipboard.");
        }
    }
}
