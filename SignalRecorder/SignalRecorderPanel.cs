using System;
using System.Drawing;
using System.Globalization;
using System.Security.AccessControl;
using System.Threading;
using System.Windows.Forms;
using SDRSharp.Radio;

namespace SDRSharp.SignalRecorder
{
    public partial class SignalRecorderPanel : UserControl
    {
        private System.Threading.Timer _timer;
        public SignalRecorderPanel()
        {
            InitializeComponent();

            chkEnableRecording.Checked = Utils.GetBooleanSetting("SignalRecordingEnabled");
            chkPowerMonitor.Checked = Utils.GetBooleanSetting("SignalRecordingPowerMonitorEnabled");
            outputFolder.Text = Utils.GetStringSetting("SignalRecordingOutputFolder", "");
            numSquelch.Value = (decimal)Utils.GetDoubleSetting("SignalRecordingSquelchValue", -10);
            trkOffset.Value = Utils.GetIntSetting("SignalRecordingOffset", 0);
            trkRange.Value = Utils.GetIntSetting("SignalRecordingRange", 80);

            _timer = new System.Threading.Timer(state =>
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                numSquelch.BackColor = DefaultBackColor;
            }, null, Timeout.Infinite, Timeout.Infinite);
        }

        public delegate void PowerMonitorChangedEventHandler();
        public event PowerMonitorChangedEventHandler PowerMonitorChanged;

        public delegate void ManualRecordingEventHandler(bool record);
        public event ManualRecordingEventHandler ManualRecording;

        public string OutputFolder { get; private set; }

        public bool RecordingEnabled
        {
            get { return chkEnableRecording.Checked; }
        }

        public bool PowerMonitorEnabled
        {
            get { return chkPowerMonitor.Checked; }
        }

        public float SquelchValue
        {
            get { return (float)numSquelch.Value; }
        }

        public int TimeStretch
        {
            get { return 1; }
        }

        public void SetStatusText(string status)
        {
            statusText.Text = status;
        }

        public void SetSquelchOpen(bool isOpen)
        {
            if (isOpen)
            {
                numSquelch.BackColor = Color.Red;
                _timer.Change(300, Timeout.Infinite);
            }
        }

        public int Range
        {
            get { return trkRange.Value; }
        }

        public int Offset
        {
            get { return trkOffset.Value; }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog{SelectedPath = outputFolder.Text, ShowNewFolderButton = true, Description = "Where to save recordings"};
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                outputFolder.Text = dlg.SelectedPath;
                Utils.SaveSetting("SignalRecordingOutputFolder", dlg.SelectedPath);
            }
        }

        private void trkRange_Scroll(object sender, EventArgs e)
        {
            Utils.SaveSetting("SignalRecordingRange", trkRange.Value);
        }

        private void trkOffset_Scroll(object sender, EventArgs e)
        {
            if (trkOffset.Value >= trkRange.Value)
                trkOffset.Value = trkRange.Value - 1;

            Utils.SaveSetting("SignalRecordingOffset", trkOffset.Value);
        }

        private void numSquelch_ValueChanged(object sender, EventArgs e)
        {
            Utils.SaveSetting("SignalRecordingSquelchValue", numSquelch.Value);
        }

        private void chkPowerMonitor_CheckedChanged(object sender, EventArgs e)
        {
            Utils.SaveSetting("SignalRecordingPowerMonitorEnabled", chkPowerMonitor.Checked);
            PowerMonitorChanged?.Invoke();
        }

        private void chkEnableRecording_CheckedChanged(object sender, EventArgs e)
        {
            Utils.SaveSetting("SignalRecordingEnabled", chkEnableRecording.Checked);
        }

        private void outputFolder_TextChanged(object sender, EventArgs e)
        {
            OutputFolder = outputFolder.Text;
        }

        private void btnRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRecord.Checked)
                btnRecord.Text = "Stop";
            else
                btnRecord.Text = "Record";

            ManualRecording?.Invoke(btnRecord.Checked);
        }
    }
}
