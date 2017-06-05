namespace SDRSharp.SpectrumAnalyzer
{
    partial class SpectrumAnalyzerPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numStart = new System.Windows.Forms.NumericUpDown();
            this.numEnd = new System.Windows.Forms.NumericUpDown();
            this.numStep = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkShowSpectrum = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkAutoScan = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numPeriod = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtExportTo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "End";
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(6, 103);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Step";
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point(49, 8);
            this.numStart.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size(120, 20);
            this.numStart.TabIndex = 3;
            // 
            // numEnd
            // 
            this.numEnd.Location = new System.Drawing.Point(49, 31);
            this.numEnd.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size(120, 20);
            this.numEnd.TabIndex = 3;
            // 
            // numStep
            // 
            this.numStep.Location = new System.Drawing.Point(49, 54);
            this.numStep.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numStep.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numStep.Name = "numStep";
            this.numStep.Size = new System.Drawing.Size(120, 20);
            this.numStep.TabIndex = 3;
            this.numStep.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "MHz";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(175, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "MHz";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(175, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "KHz";
            // 
            // chkShowSpectrum
            // 
            this.chkShowSpectrum.AutoSize = true;
            this.chkShowSpectrum.Location = new System.Drawing.Point(49, 80);
            this.chkShowSpectrum.Name = "chkShowSpectrum";
            this.chkShowSpectrum.Size = new System.Drawing.Size(99, 17);
            this.chkShowSpectrum.TabIndex = 4;
            this.chkShowSpectrum.Text = "Show spectrum";
            this.chkShowSpectrum.UseVisualStyleBackColor = true;
            this.chkShowSpectrum.CheckedChanged += new System.EventHandler(this.ShowSpectrumChanged);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(94, 103);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkAutoScan
            // 
            this.chkAutoScan.AutoSize = true;
            this.chkAutoScan.Location = new System.Drawing.Point(6, 141);
            this.chkAutoScan.Name = "chkAutoScan";
            this.chkAutoScan.Size = new System.Drawing.Size(74, 17);
            this.chkAutoScan.TabIndex = 5;
            this.chkAutoScan.Text = "Auto-scan";
            this.chkAutoScan.UseVisualStyleBackColor = true;
            this.chkAutoScan.CheckedChanged += new System.EventHandler(this.OnAutoScanChange);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(87, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Period";
            // 
            // numPeriod
            // 
            this.numPeriod.Enabled = false;
            this.numPeriod.Location = new System.Drawing.Point(130, 138);
            this.numPeriod.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPeriod.Name = "numPeriod";
            this.numPeriod.Size = new System.Drawing.Size(39, 20);
            this.numPeriod.TabIndex = 8;
            this.numPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(175, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "min";
            // 
            // txtExportTo
            // 
            this.txtExportTo.Location = new System.Drawing.Point(49, 162);
            this.txtExportTo.Name = "txtExportTo";
            this.txtExportTo.Size = new System.Drawing.Size(120, 20);
            this.txtExportTo.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 165);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Output";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(175, 160);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnBrowse.TabIndex = 12;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // SpectrumAnalyzerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtExportTo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numPeriod);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkAutoScan);
            this.Controls.Add(this.chkShowSpectrum);
            this.Controls.Add(this.numStep);
            this.Controls.Add(this.numEnd);
            this.Controls.Add(this.numStart);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "SpectrumAnalyzerPanel";
            this.Size = new System.Drawing.Size(241, 224);
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numStart;
        private System.Windows.Forms.NumericUpDown numEnd;
        private System.Windows.Forms.NumericUpDown numStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkShowSpectrum;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkAutoScan;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numPeriod;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtExportTo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnBrowse;
    }
}
