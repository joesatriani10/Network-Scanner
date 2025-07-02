
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
        comboBox1 = new System.Windows.Forms.ComboBox();
        progressBar1 = new System.Windows.Forms.ProgressBar();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
        button1.Location = new System.Drawing.Point(713, 419);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(75, 23);
        button1.TabIndex = 2;
        button1.Text = "Start";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
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
        progressBar1.Size = new System.Drawing.Size(701, 23);
        progressBar1.TabIndex = 4;
        progressBar1.Visible = false;
        //
        // Form1
        //
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(progressBar1);
        Controls.Add(comboBox1);
        Controls.Add(button1);
        Controls.Add(dataGridView1);
        Controls.Add(textBox1);
        Text = "Form1";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
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

    private System.Windows.Forms.DataGridView dataGridView1;

    private System.Windows.Forms.TextBox textBox1;

    private System.Windows.Forms.ProgressBar progressBar1;

    #endregion

    
}