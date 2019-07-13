namespace SDRSharp.SignalFinder
{
    partial class SignalFinderPanel
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
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.lblOffset = new System.Windows.Forms.Label();
            this.lblScale = new System.Windows.Forms.Label();
            this.barOffset = new System.Windows.Forms.TrackBar();
            this.barScale = new System.Windows.Forms.TrackBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.barOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barScale)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Location = new System.Drawing.Point(3, 13);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(59, 17);
            this.chkEnable.TabIndex = 1;
            this.chkEnable.Text = "Enable";
            this.chkEnable.UseVisualStyleBackColor = true;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(-1, 45);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(35, 13);
            this.lblOffset.TabIndex = 2;
            this.lblOffset.Text = "Offset";
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(-3, 96);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(34, 13);
            this.lblScale.TabIndex = 3;
            this.lblScale.Text = "Scale";
            // 
            // barOffset
            // 
            this.barOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barOffset.Location = new System.Drawing.Point(40, 45);
            this.barOffset.Maximum = 20;
            this.barOffset.Name = "barOffset";
            this.barOffset.Size = new System.Drawing.Size(223, 45);
            this.barOffset.TabIndex = 4;
            this.barOffset.Scroll += new System.EventHandler(this.barOffset_Scroll);
            // 
            // barScale
            // 
            this.barScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barScale.Location = new System.Drawing.Point(40, 96);
            this.barScale.Minimum = 1;
            this.barScale.Name = "barScale";
            this.barScale.Size = new System.Drawing.Size(223, 45);
            this.barScale.TabIndex = 5;
            this.barScale.Value = 1;
            this.barScale.Scroll += new System.EventHandler(this.barScale_Scroll);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(0, 137);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SignalFinderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.barScale);
            this.Controls.Add(this.barOffset);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.chkEnable);
            this.Name = "SignalFinderPanel";
            this.Size = new System.Drawing.Size(266, 174);
            ((System.ComponentModel.ISupportInitialize)(this.barOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEnable;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.TrackBar barOffset;
        private System.Windows.Forms.TrackBar barScale;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
