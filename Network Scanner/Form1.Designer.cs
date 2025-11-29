
namespace Network_Scanner;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        textBox1 = new System.Windows.Forms.TextBox();
        dataGridView1 = new System.Windows.Forms.DataGridView();
        contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
        copyIpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        copyRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        IPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
        HostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        PingReply = new System.Windows.Forms.DataGridViewTextBoxColumn();
        Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
        RoundtripTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
        button1 = new System.Windows.Forms.Button();
        buttonExport = new System.Windows.Forms.Button();
        comboBox1 = new System.Windows.Forms.ComboBox();
        progressBar1 = new System.Windows.Forms.ProgressBar();
        labelStart = new System.Windows.Forms.Label();
        labelEnd = new System.Windows.Forms.Label();
        startRangeUpDown = new System.Windows.Forms.NumericUpDown();
        endRangeUpDown = new System.Windows.Forms.NumericUpDown();
        labelStatus = new System.Windows.Forms.Label();
        checkBoxResolveHosts = new System.Windows.Forms.CheckBox();
        labelTimeout = new System.Windows.Forms.Label();
        numericTimeout = new System.Windows.Forms.NumericUpDown();
        headerPanel = new System.Windows.Forms.Panel();
        labelHeader = new System.Windows.Forms.Label();
        labelStats = new System.Windows.Forms.Label();
        labelCurrentTarget = new System.Windows.Forms.Label();
        footerPanel = new System.Windows.Forms.Panel();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        contextMenuStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)startRangeUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)endRangeUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericTimeout).BeginInit();
        headerPanel.SuspendLayout();
        footerPanel.SuspendLayout();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Location = new System.Drawing.Point(164, 60);
        textBox1.Name = "textBox1";
        textBox1.Size = new System.Drawing.Size(160, 25);
        textBox1.TabIndex = 0;
        // 
        // dataGridView1
        // 
        dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.ContextMenuStrip = contextMenuStrip1;
        dataGridView1.Location = new System.Drawing.Point(12, 96);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new System.Drawing.Size(876, 468);
        dataGridView1.TabIndex = 1;
        dataGridView1.Text = "dataGridView1";
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyIpMenuItem, copyRowMenuItem });
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new System.Drawing.Size(129, 48);
        // 
        // copyIpMenuItem
        // 
        copyIpMenuItem.Name = "copyIpMenuItem";
        copyIpMenuItem.Size = new System.Drawing.Size(128, 22);
        copyIpMenuItem.Text = "Copy IP";
        copyIpMenuItem.Click += copyIpMenuItem_Click;
        // 
        // copyRowMenuItem
        // 
        copyRowMenuItem.Name = "copyRowMenuItem";
        copyRowMenuItem.Size = new System.Drawing.Size(128, 22);
        copyRowMenuItem.Text = "Copy Row";
        copyRowMenuItem.Click += copyRowMenuItem_Click;
        // 
        // IPAddress
        // 
        IPAddress.HeaderText = "IP Address";
        IPAddress.Name = "IPAddress";
        // 
        // HostName 
        // 
        HostName.HeaderText = "Host Name ";
        HostName.Name = "HostName ";
        // 
        // PingReply 
        // 
        PingReply.HeaderText = "Ping Reply ";
        PingReply.Name = "PingReply ";
        // 
        // Status
        // 
        Status.HeaderText = "Status";
        Status.Name = "Status";
        // 
        // RoundtripTime
        // 
        RoundtripTime.HeaderText = "Roundtrip Time";
        RoundtripTime.Name = "RoundtripTime";
        // 
        // button1
        // 
        button1.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
        button1.BackColor = System.Drawing.Color.FromArgb(((int)((byte)55)), ((int)((byte)60)), ((int)((byte)65)));
        button1.Cursor = System.Windows.Forms.Cursors.Hand;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        button1.ForeColor = System.Drawing.Color.White;
        button1.Location = new System.Drawing.Point(777, 40);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(111, 61);
        button1.TabIndex = 2;
        button1.Text = "Start";
        button1.UseVisualStyleBackColor = false;
        button1.Click += button1_Click;
        // 
        // buttonExport
        // 
        buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
        buttonExport.BackColor = System.Drawing.Color.FromArgb(((int)((byte)235)), ((int)((byte)235)), ((int)((byte)235)));
        buttonExport.Cursor = System.Windows.Forms.Cursors.Hand;
        buttonExport.FlatAppearance.BorderSize = 0;
        buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        buttonExport.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)40)), ((int)((byte)40)), ((int)((byte)40)));
        buttonExport.Location = new System.Drawing.Point(660, 40);
        buttonExport.Name = "buttonExport";
        buttonExport.Size = new System.Drawing.Size(111, 61);
        buttonExport.TabIndex = 9;
        buttonExport.Text = "Export";
        buttonExport.UseVisualStyleBackColor = false;
        buttonExport.Click += buttonExport_Click;
        // 
        // comboBox1
        // 
        comboBox1.FormattingEnabled = true;
        comboBox1.Location = new System.Drawing.Point(12, 60);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new System.Drawing.Size(146, 25);
        comboBox1.TabIndex = 3;
        // 
        // progressBar1
        // 
        progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        progressBar1.Location = new System.Drawing.Point(12, 10);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new System.Drawing.Size(876, 20);
        progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
        progressBar1.TabIndex = 4;
        progressBar1.Visible = false;
        // 
        // labelStart
        // 
        labelStart.AutoSize = true;
        labelStart.Location = new System.Drawing.Point(334, 69);
        labelStart.Name = "labelStart";
        labelStart.Size = new System.Drawing.Size(35, 17);
        labelStart.TabIndex = 5;
        labelStart.Text = "Start";
        // 
        // labelEnd
        // 
        labelEnd.AutoSize = true;
        labelEnd.Location = new System.Drawing.Point(446, 69);
        labelEnd.Name = "labelEnd";
        labelEnd.Size = new System.Drawing.Size(30, 17);
        labelEnd.TabIndex = 6;
        labelEnd.Text = "End";
        // 
        // startRangeUpDown
        // 
        startRangeUpDown.Location = new System.Drawing.Point(372, 60);
        startRangeUpDown.Maximum = new decimal(new int[] { 254, 0, 0, 0 });
        startRangeUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        startRangeUpDown.Name = "startRangeUpDown";
        startRangeUpDown.Size = new System.Drawing.Size(62, 25);
        startRangeUpDown.TabIndex = 7;
        startRangeUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // endRangeUpDown
        // 
        endRangeUpDown.Location = new System.Drawing.Point(480, 60);
        endRangeUpDown.Maximum = new decimal(new int[] { 254, 0, 0, 0 });
        endRangeUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        endRangeUpDown.Name = "endRangeUpDown";
        endRangeUpDown.Size = new System.Drawing.Size(62, 25);
        endRangeUpDown.TabIndex = 8;
        endRangeUpDown.Value = new decimal(new int[] { 254, 0, 0, 0 });
        // 
        // labelStatus
        // 
        labelStatus.AutoSize = true;
        labelStatus.Location = new System.Drawing.Point(12, 84);
        labelStatus.Name = "labelStatus";
        labelStatus.Size = new System.Drawing.Size(44, 17);
        labelStatus.TabIndex = 10;
        labelStatus.Text = "Ready";
        // 
        // checkBoxResolveHosts
        // 
        checkBoxResolveHosts.AutoSize = true;
        checkBoxResolveHosts.Location = new System.Drawing.Point(558, 66);
        checkBoxResolveHosts.Name = "checkBoxResolveHosts";
        checkBoxResolveHosts.Size = new System.Drawing.Size(107, 21);
        checkBoxResolveHosts.TabIndex = 11;
        checkBoxResolveHosts.Text = "Resolve hosts";
        checkBoxResolveHosts.UseVisualStyleBackColor = true;
        // 
        // labelTimeout
        // 
        labelTimeout.AutoSize = true;
        labelTimeout.Location = new System.Drawing.Point(670, 69);
        labelTimeout.Name = "labelTimeout";
        labelTimeout.Size = new System.Drawing.Size(76, 17);
        labelTimeout.TabIndex = 12;
        labelTimeout.Text = "Timeout ms";
        // 
        // numericTimeout
        // 
        numericTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
        numericTimeout.Location = new System.Drawing.Point(748, 60);
        numericTimeout.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
        numericTimeout.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        numericTimeout.Name = "numericTimeout";
        numericTimeout.Size = new System.Drawing.Size(92, 25);
        numericTimeout.TabIndex = 13;
        numericTimeout.Value = new decimal(new int[] { 1000, 0, 0, 0 });
        numericTimeout.ValueChanged += numericTimeout_ValueChanged;
        // 
        // headerPanel
        // 
        headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)((byte)224)), ((int)((byte)224)), ((int)((byte)224)));
        headerPanel.Controls.Add(labelHeader);
        headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
        headerPanel.Location = new System.Drawing.Point(0, 0);
        headerPanel.Name = "headerPanel";
        headerPanel.Size = new System.Drawing.Size(900, 53);
        headerPanel.TabIndex = 14;
        // 
        // labelHeader
        // 
        labelHeader.AutoSize = true;
        labelHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
        labelHeader.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)45)), ((int)((byte)45)), ((int)((byte)45)));
        labelHeader.Location = new System.Drawing.Point(12, 10);
        labelHeader.Name = "labelHeader";
        labelHeader.Size = new System.Drawing.Size(208, 25);
        labelHeader.TabIndex = 0;
        labelHeader.Text = "Network Scanner · Fast";
        // 
        // labelStats
        // 
        labelStats.AutoSize = true;
        labelStats.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
        labelStats.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)10)), ((int)((byte)132)), ((int)((byte)255)));
        labelStats.Location = new System.Drawing.Point(12, 58);
        labelStats.Name = "labelStats";
        labelStats.Size = new System.Drawing.Size(206, 17);
        labelStats.TabIndex = 15;
        labelStats.Text = "Online: 0 | Avg: - | Min: - | Max: -";
        // 
        // labelCurrentTarget
        // 
        labelCurrentTarget.AutoSize = true;
        labelCurrentTarget.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)48)), ((int)((byte)209)), ((int)((byte)88)));
        labelCurrentTarget.Location = new System.Drawing.Point(12, 33);
        labelCurrentTarget.Name = "labelCurrentTarget";
        labelCurrentTarget.Size = new System.Drawing.Size(110, 17);
        labelCurrentTarget.TabIndex = 16;
        labelCurrentTarget.Text = "Current: Waiting...";
        // 
        // footerPanel
        // 
        footerPanel.Controls.Add(button1);
        footerPanel.Controls.Add(buttonExport);
        footerPanel.Controls.Add(progressBar1);
        footerPanel.Controls.Add(labelStatus);
        footerPanel.Controls.Add(labelStats);
        footerPanel.Controls.Add(labelCurrentTarget);
        footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
        footerPanel.Location = new System.Drawing.Point(0, 571);
        footerPanel.Name = "footerPanel";
        footerPanel.Size = new System.Drawing.Size(900, 116);
        footerPanel.TabIndex = 17;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)247)), ((int)((byte)248)), ((int)((byte)250)));
        ClientSize = new System.Drawing.Size(900, 687);
        Controls.Add(headerPanel);
        Controls.Add(numericTimeout);
        Controls.Add(labelTimeout);
        Controls.Add(checkBoxResolveHosts);
        Controls.Add(endRangeUpDown);
        Controls.Add(startRangeUpDown);
        Controls.Add(labelEnd);
        Controls.Add(labelStart);
        Controls.Add(comboBox1);
        Controls.Add(footerPanel);
        Controls.Add(dataGridView1);
        Controls.Add(textBox1);
        Font = new System.Drawing.Font("Segoe UI", 9.75F);
        MinimumSize = new System.Drawing.Size(820, 480);
        Text = "Network Scanner";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        contextMenuStrip1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)startRangeUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)endRangeUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericTimeout).EndInit();
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        footerPanel.ResumeLayout(false);
        footerPanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.ComboBox comboBox1;

    private System.Windows.Forms.DataGridViewTextBoxColumn IPAddress;
    private System.Windows.Forms.DataGridViewTextBoxColumn HostName;
    private System.Windows.Forms.DataGridViewTextBoxColumn PingReply;
    private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    private System.Windows.Forms.DataGridViewTextBoxColumn RoundtripTime;

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button buttonExport;

    private System.Windows.Forms.DataGridView dataGridView1;

    private System.Windows.Forms.TextBox textBox1;

    private System.Windows.Forms.ProgressBar progressBar1;

    private System.Windows.Forms.Label labelStart;
    private System.Windows.Forms.Label labelEnd;
    private System.Windows.Forms.NumericUpDown startRangeUpDown;
    private System.Windows.Forms.NumericUpDown endRangeUpDown;
    private System.Windows.Forms.Label labelStatus;
    private System.Windows.Forms.CheckBox checkBoxResolveHosts;
    private System.Windows.Forms.Label labelTimeout;
    private System.Windows.Forms.NumericUpDown numericTimeout;
    private System.Windows.Forms.Panel headerPanel;
    private System.Windows.Forms.Label labelHeader;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem copyIpMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyRowMenuItem;
    private System.Windows.Forms.Label labelStats;
    private System.Windows.Forms.Label labelCurrentTarget;
    private System.Windows.Forms.Panel footerPanel;

    #endregion

    
}
