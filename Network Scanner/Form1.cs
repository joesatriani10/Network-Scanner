using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Scanner;

public partial class Form1 : Form
{
    private const bool EnableResolutionLogging = true;
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

        // Store scan results for export and UI rendering
        results = new ConcurrentBag<(string, string, string, IPStatus, long)>();

        // Prepare the grid and populate interface choices
        InitializeDataGridView();
        LoadNetworkInterfaces();

        // Constrain IPv4 input for the base address selector
        textBox1.KeyPress += textBox1_KeyPress;
        textBox1.MaxLength = 15;

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
        // Style and size the grid so scan results are easy to scan
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
            ForeColor = Color.FromArgb(45, 45, 45),
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

        // Allow common wired and wireless adapter names while skipping virtuals
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
        // Prefill the base IPv4 using the selected adapter (ending in .1)
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
        // Validate IPv4 input and return the /24 base prefix
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

    private async void button1_Click(object sender, EventArgs e)
    {
        // Toggle between starting a scan and cancelling an in-flight scan
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
        // Fan out ping requests across the host range with controlled parallelism
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
        // Ping a single host and update UI-bound collections on success
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
                var hostInfo = await ResolveHostNameAsync(ipString, resolveHosts, token);
                hostName = hostInfo.Name;

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
                    if (EnableResolutionLogging && !string.Equals(hostInfo.Name, "Unknown", StringComparison.OrdinalIgnoreCase))
                    {
                        SetStatus($"Resolved {ipString} via {hostInfo.Source}: {hostInfo.Name}");
                    }
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
        // Maintain running aggregates for average, min, and max latency labels
        Interlocked.Increment(ref _onlineCount);
        Interlocked.Add(ref _latencyTotal, rtt);
        UpdateMin(ref _latencyMin, rtt);
        UpdateMax(ref _latencyMax, rtt);
    }

    private async Task<(string Name, string Source)> ResolveHostNameAsync(string ipString, bool resolveHosts, CancellationToken token)
    {
        if (!resolveHosts)
            return ("Unknown", "Disabled");

        try
        {
            var host = await Dns.GetHostEntryAsync(ipString);
            if (!string.IsNullOrWhiteSpace(host.HostName))
            {
                LogResolution(ipString, "DNS", host.HostName, success: true);
                return (host.HostName, "DNS");
            }
        }
        catch
        {
            LogResolution(ipString, "DNS", "No PTR or DNS error");
        }

        try
        {
            string? netBios = await ResolveNetBiosNameAsync(ipString, token);
            if (!string.IsNullOrWhiteSpace(netBios))
            {
                LogResolution(ipString, "NetBIOS", netBios, success: true);
                return (netBios, "NetBIOS");
            }
        }
        catch
        {
            LogResolution(ipString, "NetBIOS", "Resolution blocked or failed");
        }

        try
        {
            string? multicast = await ResolveMulticastNameAsync(ipString, token);
            if (!string.IsNullOrWhiteSpace(multicast))
            {
                LogResolution(ipString, "Multicast", multicast, success: true);
                return (multicast, "Multicast");
            }
        }
        catch
        {
            LogResolution(ipString, "Multicast", "Resolution blocked or failed");
        }

        LogResolution(ipString, "Resolution", "No name found across DNS/NetBIOS/Multicast");
        return ("Unknown", "None");
    }

    private async Task<string?> ResolveNetBiosNameAsync(string ipString, CancellationToken token)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "nbtstat",
                Arguments = $"-A {ipString}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        if (!process.Start())
            return null;

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var waitTask = process.WaitForExitAsync(token);
        var completed = await Task.WhenAny(waitTask, Task.Delay(2000, token));

        if (completed != waitTask)
        {
            try { process.Kill(); } catch { }
            return null;
        }

        string output = await outputTask;
        return ParseNetBiosName(output);
    }

    private async Task<string?> ResolveMulticastNameAsync(string ipString, CancellationToken token)
    {
        if (!System.Net.IPAddress.TryParse(ipString, out var address))
            return null;

        string reverse = string.Join(".", address.GetAddressBytes().Reverse()) + ".in-addr.arpa";

        var targets = new List<(byte[] Query, IPEndPoint Endpoint)>
        {
            (BuildPtrQuery(reverse, requestUnicastResponse: false, zeroTransactionId: false),
                new IPEndPoint(System.Net.IPAddress.Parse("224.0.0.252"), 5355)), // LLMNR
            (BuildPtrQuery(reverse, requestUnicastResponse: true, zeroTransactionId: true),
                new IPEndPoint(System.Net.IPAddress.Parse("224.0.0.251"), 5353))  // mDNS with QU flag
        };

        foreach (var target in targets)
        {
            string? name = await SendDnsQueryAsync(target.Query, target.Endpoint, token);
            if (!string.IsNullOrWhiteSpace(name))
                return name;
        }

        return null;
    }

    private async Task<string?> SendDnsQueryAsync(byte[] query, IPEndPoint endpoint, CancellationToken token)
    {
        using var client = new UdpClient(AddressFamily.InterNetwork);
        client.Client.ReceiveTimeout = 1500;
        try
        {
            await client.SendAsync(query, query.Length, endpoint);

            var receiveTask = client.ReceiveAsync();
            var completed = await Task.WhenAny(receiveTask, Task.Delay(1500, token));
            if (completed != receiveTask)
                return null;

            var response = receiveTask.Result.Buffer;
            string? name = ParsePtrAnswer(response);
            if (!string.IsNullOrWhiteSpace(name))
            {
                LogResolution(endpoint.ToString(), "MulticastResponse", name, success: true);
            }
            return name;
        }
        catch
        {
            return null;
        }
    }

    private static byte[] BuildPtrQuery(string reverseName, bool requestUnicastResponse, bool zeroTransactionId)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        ushort id = zeroTransactionId ? (ushort)0 : (ushort)RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        WriteUInt16(writer, id);
        WriteUInt16(writer, 0); // flags
        WriteUInt16(writer, 1); // questions
        WriteUInt16(writer, 0); // answers
        WriteUInt16(writer, 0); // authority
        WriteUInt16(writer, 0); // additional

        WriteName(writer, reverseName);
        WriteUInt16(writer, 12); // PTR
        ushort qclass = requestUnicastResponse ? (ushort)(0x8000 | 1) : (ushort)1;
        WriteUInt16(writer, qclass);  // IN (+QU for mDNS)

        return ms.ToArray();
    }

    private static string? ParsePtrAnswer(byte[] response)
    {
        if (response.Length < 12)
            return null;

        int qdCount = ReadUInt16(response, 4);
        int anCount = ReadUInt16(response, 6);
        int offset = 12;

        for (int i = 0; i < qdCount; i++)
        {
            ReadName(response, ref offset);
            offset += 4; // type + class
        }

        for (int i = 0; i < anCount; i++)
        {
            ReadName(response, ref offset);
            if (offset + 10 > response.Length)
                return null;

            ushort type = ReadUInt16(response, offset);
            offset += 2;
            offset += 2; // class
            offset += 4; // ttl
            ushort rdLength = ReadUInt16(response, offset);
            offset += 2;

            if (offset + rdLength > response.Length)
                return null;

            if (type == 12) // PTR
            {
                int rdataOffset = offset;
                string name = ReadName(response, ref rdataOffset);
                return string.IsNullOrWhiteSpace(name) ? null : name;
            }

            offset += rdLength;
        }

        return null;
    }

    private static void WriteUInt16(BinaryWriter writer, ushort value)
    {
        writer.Write((byte)(value >> 8));
        writer.Write((byte)(value & 0xFF));
    }

    private static void WriteName(BinaryWriter writer, string domain)
    {
        foreach (var label in domain.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            var bytes = Encoding.ASCII.GetBytes(label);
            writer.Write((byte)bytes.Length);
            writer.Write(bytes);
        }
        writer.Write((byte)0);
    }

    private static string ReadName(byte[] buffer, ref int offset)
    {
        var labels = new List<string>();
        int pos = offset;
        bool jumped = false;
        int guard = 0;

        while (pos < buffer.Length && guard++ < 128)
        {
            byte len = buffer[pos++];
            if (len == 0)
                break;

            if ((len & 0xC0) == 0xC0)
            {
                if (pos >= buffer.Length)
                    break;

                int pointer = ((len & 0x3F) << 8) | buffer[pos++];
                if (!jumped)
                {
                    offset = pos;
                    jumped = true;
                }
                pos = pointer;
                continue;
            }

            if (pos + len > buffer.Length)
                break;

            labels.Add(Encoding.ASCII.GetString(buffer, pos, len));
            pos += len;
        }

        if (!jumped)
            offset = pos;

        return labels.Count == 0 ? "" : string.Join(".", labels);
    }

    private static ushort ReadUInt16(byte[] buffer, int offset)
    {
        if (offset + 1 >= buffer.Length)
            return 0;
        return (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
    }

    private void LogResolution(string target, string source, string message, bool success = false)
    {
        if (!EnableResolutionLogging)
            return;

        string text = success ? $"[{source}] {target}: {message}" : $"[{source}] {target}: {message}";
        Debug.WriteLine(text);
    }

    private static string? ParseNetBiosName(string output)
    {
        if (string.IsNullOrWhiteSpace(output))
            return null;

        foreach (var raw in output.Split('\n'))
        {
            var line = raw.Trim();
            if (!line.Contains("<") || !line.Contains(">"))
                continue;

            if (!line.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase))
                continue;

            int tagIndex = line.IndexOf('<');
            if (tagIndex <= 0)
                continue;

            string name = line[..tagIndex].Trim();
            if (string.IsNullOrWhiteSpace(name) || name.StartsWith("__"))
                continue;

            return name;
        }

        return null;
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

    private void labelStatus_Click(object sender, EventArgs e)
    {
    }

    private void labelStats_Click(object sender, EventArgs e)
    {
    }

    private void numericTimeout_ValueChanged(object sender, EventArgs e)
    {
        int timeout = (int)numericTimeout.Value;
        SetStatus($"Timeout set to {timeout} ms.");
    }
}
