
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
        textBox1 = new System.Windows.Forms.TextBox();
        dataGridView1 = new System.Windows.Forms.DataGridView();
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
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)startRangeUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)endRangeUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericTimeout).BeginInit();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Location = new System.Drawing.Point(154, 8);
        textBox1.Name = "textBox1";
        textBox1.Size = new System.Drawing.Size(160, 23);
        textBox1.TabIndex = 0;
        // 
        // dataGridView1
        // 
        dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new System.Drawing.Point(6, 37);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new System.Drawing.Size(788, 376);
        dataGridView1.TabIndex = 1;
        dataGridView1.Text = "dataGridView1";
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
        button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        button1.Location = new System.Drawing.Point(653, 419);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(75, 23);
        button1.TabIndex = 2;
        button1.Text = "Start";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        //
        // buttonExport
        //
        buttonExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        buttonExport.Location = new System.Drawing.Point(572, 419);
        buttonExport.Name = "buttonExport";
        buttonExport.Size = new System.Drawing.Size(75, 23);
        buttonExport.TabIndex = 9;
        buttonExport.Text = "Export";
        buttonExport.UseVisualStyleBackColor = true;
        buttonExport.Click += buttonExport_Click;
        // 
        // comboBox1
        //
        comboBox1.FormattingEnabled = true;
        comboBox1.Location = new System.Drawing.Point(6, 8);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new System.Drawing.Size(142, 23);
        comboBox1.TabIndex = 3;
        //
        // progressBar1
        //
        progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
        progressBar1.Location = new System.Drawing.Point(6, 419);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new System.Drawing.Size(560, 23);
        progressBar1.TabIndex = 4;
        progressBar1.Visible = false;
        //
        // labelStart
        //
        labelStart.AutoSize = true;
        labelStart.Location = new System.Drawing.Point(320, 12);
        labelStart.Name = "labelStart";
        labelStart.Size = new System.Drawing.Size(32, 15);
        labelStart.TabIndex = 5;
        labelStart.Text = "Start";
        //
        // labelEnd
        //
        labelEnd.AutoSize = true;
        labelEnd.Location = new System.Drawing.Point(426, 12);
        labelEnd.Name = "labelEnd";
        labelEnd.Size = new System.Drawing.Size(27, 15);
        labelEnd.TabIndex = 6;
        labelEnd.Text = "End";
        //
        // startRangeUpDown
        //
        startRangeUpDown.Location = new System.Drawing.Point(358, 8);
        startRangeUpDown.Maximum = new decimal(new int[] {254, 0, 0, 0});
        startRangeUpDown.Minimum = new decimal(new int[] {1, 0, 0, 0});
        startRangeUpDown.Name = "startRangeUpDown";
        startRangeUpDown.Size = new System.Drawing.Size(62, 23);
        startRangeUpDown.TabIndex = 7;
        startRangeUpDown.Value = new decimal(new int[] {1, 0, 0, 0});
        //
        // endRangeUpDown
        //
        endRangeUpDown.Location = new System.Drawing.Point(459, 8);
        endRangeUpDown.Maximum = new decimal(new int[] {254, 0, 0, 0});
        endRangeUpDown.Minimum = new decimal(new int[] {1, 0, 0, 0});
        endRangeUpDown.Name = "endRangeUpDown";
        endRangeUpDown.Size = new System.Drawing.Size(62, 23);
        endRangeUpDown.TabIndex = 8;
        endRangeUpDown.Value = new decimal(new int[] {254, 0, 0, 0});
        //
        // checkBoxResolveHosts
        //
        checkBoxResolveHosts.AutoSize = true;
        checkBoxResolveHosts.Checked = true;
        checkBoxResolveHosts.CheckState = System.Windows.Forms.CheckState.Checked;
        checkBoxResolveHosts.Location = new System.Drawing.Point(527, 10);
        checkBoxResolveHosts.Name = "checkBoxResolveHosts";
        checkBoxResolveHosts.Size = new System.Drawing.Size(99, 19);
        checkBoxResolveHosts.TabIndex = 11;
        checkBoxResolveHosts.Text = "Resolve hosts";
        checkBoxResolveHosts.UseVisualStyleBackColor = true;
        //
        // labelTimeout
        //
        labelTimeout.AutoSize = true;
        labelTimeout.Location = new System.Drawing.Point(632, 12);
        labelTimeout.Name = "labelTimeout";
        labelTimeout.Size = new System.Drawing.Size(64, 15);
        labelTimeout.TabIndex = 12;
        labelTimeout.Text = "Timeout ms";
        //
        // numericTimeout
        //
        numericTimeout.Increment = new decimal(new int[] {100, 0, 0, 0});
        numericTimeout.Location = new System.Drawing.Point(702, 8);
        numericTimeout.Maximum = new decimal(new int[] {5000, 0, 0, 0});
        numericTimeout.Minimum = new decimal(new int[] {100, 0, 0, 0});
        numericTimeout.Name = "numericTimeout";
        numericTimeout.Size = new System.Drawing.Size(92, 23);
        numericTimeout.TabIndex = 13;
        numericTimeout.Value = new decimal(new int[] {1000, 0, 0, 0});
        //
        // labelStatus
        //
        labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        labelStatus.AutoSize = true;
        labelStatus.Location = new System.Drawing.Point(6, 401);
        labelStatus.Name = "labelStatus";
        labelStatus.Size = new System.Drawing.Size(36, 15);
        labelStatus.TabIndex = 10;
        labelStatus.Text = "Ready";
        //
        // Form1
        //
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(numericTimeout);
        Controls.Add(labelTimeout);
        Controls.Add(checkBoxResolveHosts);
        Controls.Add(labelStatus);
        Controls.Add(endRangeUpDown);
        Controls.Add(startRangeUpDown);
        Controls.Add(labelEnd);
        Controls.Add(labelStart);
        Controls.Add(buttonExport);
        Controls.Add(progressBar1);
        Controls.Add(comboBox1);
        Controls.Add(button1);
        Controls.Add(dataGridView1);
        Controls.Add(textBox1);
        Text = "Network Scanner";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ((System.ComponentModel.ISupportInitialize)startRangeUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)endRangeUpDown).EndInit();
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

    #endregion

    
}
