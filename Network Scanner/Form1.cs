using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace Network_Scanner;

public partial class Form1 : Form
{
    private ConcurrentBag<(string IpAddress, string HostName, string PingReply, IPStatus Status, long RoundtripTime)> results;
    private CancellationTokenSource? _cts;
    private int _completed;

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
    }

    private void InitializeDataGridView()
    {
        dataGridView1.Columns.Clear();
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
            if (System.Net.IPAddress.TryParse(textBox1.Text, out System.Net.IPAddress ipAddress) &&
                ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                dataGridView1.Rows.Clear();
                results = new ConcurrentBag<(string, string, string, IPStatus, long)>(); // Reset stored results
                _cts = new CancellationTokenSource();
                _completed = 0;
                progressBar1.Value = 0;
                progressBar1.Maximum = 254;
                progressBar1.Visible = true;
                button1.Text = "Cancel";

                string baseIp = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf('.') + 1);
                try
                {
                    await PingNetworkAsync(baseIp, _cts.Token);
                }
                finally
                {
                    progressBar1.Visible = false;
                    _cts.Dispose();
                    _cts = null;
                    button1.Text = "Start";
                }
                return;
            }
        }
        MessageBox.Show("Please enter a valid IPv4 address.");
    }
    
    private void CancelScan()
    {
        if (_cts != null && !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }
    }

    private async Task PingNetworkAsync(string baseIp, CancellationToken token)
    {
        var tasks = new List<Task>();

        for (int i = 1; i <= 254; i++)
        {
            if (token.IsCancellationRequested)
                break;

            string ipString = baseIp + i.ToString();
            tasks.Add(Task.Run(() => PingAddress(ipString, token)));
        }

        await Task.WhenAll(tasks);
    }

    private void PingAddress(string ipString, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return;
        try
        {
            using var ping = new Ping();
            PingReply pingReply = ping.Send(ipString, 3000); // Timeout increased to 3000 ms (3 seconds)

            if (pingReply.Status == IPStatus.Success)
            {
                if (System.Net.IPAddress.TryParse(ipString, out var ipAddress))
                {
                    string name = "Unknown";
                    try
                    {
                        var host = Dns.GetHostEntry(ipAddress);
                        name = host.HostName;
                    }
                    catch
                    {
                        // Ignore error in case cant resolve host
                    }

                    var entry = (IpAddress: ipString,
                                  HostName: name,
                                  PingReply: ipAddress.ToString(),
                                  Status: pingReply.Status,
                                  RoundtripTime: pingReply.RoundtripTime);
                    results.Add(entry);
                    this.Invoke((Action)(() =>
                    {
                        int rowIndex = dataGridView1.Rows.Add();
                        dataGridView1.Rows[rowIndex].Cells[0].Value = entry.IpAddress;
                        dataGridView1.Rows[rowIndex].Cells[1].Value = entry.HostName;
                        dataGridView1.Rows[rowIndex].Cells[2].Value = entry.PingReply;
                        dataGridView1.Rows[rowIndex].Cells[3].Value = entry.Status.ToString();
                        dataGridView1.Rows[rowIndex].Cells[4].Value = entry.RoundtripTime;
                    }));
                }
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
                if (count <= progressBar1.Maximum)
                    progressBar1.Value = count;
            }));
        }
    }
}
