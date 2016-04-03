namespace SDRSharp.SignalRecorder
{
    partial class SignalRecorderPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkEnableRecording = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.outputFolder = new System.Windows.Forms.TextBox();
            this.statusText = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chkPowerMonitor = new System.Windows.Forms.CheckBox();
            this.trkRange = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trkOffset = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.numSquelch = new System.Windows.Forms.NumericUpDown();
            this.numStretch = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquelch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStretch)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEnableRecording
            // 
            this.chkEnableRecording.AutoSize = true;
            this.chkEnableRecording.Location = new System.Drawing.Point(4, 4);
            this.chkEnableRecording.Name = "chkEnableRecording";
            this.chkEnableRecording.Size = new System.Drawing.Size(106, 17);
            this.chkEnableRecording.TabIndex = 0;
            this.chkEnableRecording.Text = "Enable recording";
            this.chkEnableRecording.UseVisualStyleBackColor = true;
            this.chkEnableRecording.CheckedChanged += new System.EventHandler(this.chkEnableRecording_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Output folder:";
            // 
            // outputFolder
            // 
            this.outputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputFolder.Location = new System.Drawing.Point(4, 45);
            this.outputFolder.MaxLength = 280;
            this.outputFolder.Name = "outputFolder";
            this.outputFolder.Size = new System.Drawing.Size(272, 20);
            this.outputFolder.TabIndex = 2;
            this.outputFolder.TextChanged += new System.EventHandler(this.outputFolder_TextChanged);
            // 
            // statusText
            // 
            this.statusText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusText.ForeColor = System.Drawing.Color.Red;
            this.statusText.Location = new System.Drawing.Point(0, 97);
            this.statusText.Name = "statusText";
            this.statusText.ReadOnly = true;
            this.statusText.Size = new System.Drawing.Size(312, 20);
            this.statusText.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(281, 44);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(34, 19);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkPowerMonitor
            // 
            this.chkPowerMonitor.AutoSize = true;
            this.chkPowerMonitor.Location = new System.Drawing.Point(4, 129);
            this.chkPowerMonitor.Name = "chkPowerMonitor";
            this.chkPowerMonitor.Size = new System.Drawing.Size(93, 17);
            this.chkPowerMonitor.TabIndex = 0;
            this.chkPowerMonitor.Text = "Power monitor";
            this.chkPowerMonitor.UseVisualStyleBackColor = true;
            this.chkPowerMonitor.CheckedChanged += new System.EventHandler(this.chkPowerMonitor_CheckedChanged);
            // 
            // trkRange
            // 
            this.trkRange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkRange.Location = new System.Drawing.Point(4, 174);
            this.trkRange.Maximum = 150;
            this.trkRange.Minimum = 10;
            this.trkRange.Name = "trkRange";
            this.trkRange.Size = new System.Drawing.Size(311, 45);
            this.trkRange.TabIndex = 5;
            this.trkRange.Value = 150;
            this.trkRange.Scroll += new System.EventHandler(this.trkRange_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Range";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Offset";
            // 
            // trkOffset
            // 
            this.trkOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkOffset.Location = new System.Drawing.Point(4, 238);
            this.trkOffset.Maximum = 50;
            this.trkOffset.Name = "trkOffset";
            this.trkOffset.Size = new System.Drawing.Size(311, 45);
            this.trkOffset.TabIndex = 5;
            this.trkOffset.Scroll += new System.EventHandler(this.trkOffset_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Squelch level";
            // 
            // numSquelch
            // 
            this.numSquelch.DecimalPlaces = 1;
            this.numSquelch.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numSquelch.Location = new System.Drawing.Point(83, 71);
            this.numSquelch.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSquelch.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            -2147483648});
            this.numSquelch.Name = "numSquelch";
            this.numSquelch.Size = new System.Drawing.Size(53, 20);
            this.numSquelch.TabIndex = 8;
            this.numSquelch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSquelch.ValueChanged += new System.EventHandler(this.numSquelch_ValueChanged);
            // 
            // numStretch
            // 
            this.numStretch.Location = new System.Drawing.Point(82, 282);
            this.numStretch.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numStretch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStretch.Name = "numStretch";
            this.numStretch.Size = new System.Drawing.Size(53, 20);
            this.numStretch.TabIndex = 8;
            this.numStretch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStretch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStretch.ValueChanged += new System.EventHandler(this.numStretch_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Time stretch";
            // 
            // SignalRecorderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numStretch);
            this.Controls.Add(this.numSquelch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trkOffset);
            this.Controls.Add(this.trkRange);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.outputFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkPowerMonitor);
            this.Controls.Add(this.chkEnableRecording);
            this.Name = "SignalRecorderPanel";
            this.Size = new System.Drawing.Size(321, 340);
            ((System.ComponentModel.ISupportInitialize)(this.trkRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquelch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStretch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEnableRecording;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outputFolder;
        private System.Windows.Forms.TextBox statusText;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkPowerMonitor;
        private System.Windows.Forms.TrackBar trkRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trkOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSquelch;
        private System.Windows.Forms.NumericUpDown numStretch;
        private System.Windows.Forms.Label label5;
    }
}
