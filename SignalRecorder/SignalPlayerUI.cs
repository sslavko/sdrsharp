using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using SDRSharp.Radio;

namespace SDRSharp.SignalRecorder
{
    public partial class SignalPlayerUI : Form
    {
        public SignalPlayerUI()
        {
            InitializeComponent();

            FileName = Utils.GetStringSetting("SignalPlayerFileName", "");
        }

        public string FileName
        {
            get { return txtFileName.Text; }
            set
            {
                if (!File.Exists(value))
                    return;

                txtFileName.Text = value;

                using (var strm = File.OpenRead(FileName))
                using (var reader = new BinaryReader(strm))
                {
                    TimeOfRecording = DateTime.FromBinary(reader.ReadInt64());
                    SampleRate = reader.ReadDouble();
                    CenterFrequency = reader.ReadInt64();
                    Frequency = reader.ReadInt64();
                    Duration = TimeSpan.FromMilliseconds(((strm.Length - 32.0f) / 8) / SampleRate * 1000);

                    if (SignalRecorderPlugin.MainControl != null)
                    {
                        SignalRecorderPlugin.MainControl.CenterFrequency = CenterFrequency;
                        SignalRecorderPlugin.MainControl.Frequency = Frequency;
                        SignalRecorderPlugin.MainControl.StopRadio();
                        SignalRecorderPlugin.MainControl.StartRadio();
                    }
                }

                Utils.SaveSetting("SignalPlayerFileName", value);
            }
        }

        private DateTime _timeOfRecording;

        public DateTime TimeOfRecording
        {
            get { return _timeOfRecording; }
            set
            {
                _timeOfRecording = value;
                txtTime.Text = _timeOfRecording.ToString(CultureInfo.InvariantCulture);
            }
        }

        private double _sampleRate = 2500000;

        public double SampleRate
        {
            get { return _sampleRate; }
            set
            {
                _sampleRate = value;
                txtSampleRate.Text = _sampleRate.ToString(CultureInfo.InvariantCulture);
            }
        }

        private long _frequency = 15000000U;
        
        public long Frequency
        {
            get { return _frequency; }
            private set
            {
                _frequency = value;
                txtFrequency.Text = _frequency.ToString(CultureInfo.InvariantCulture);
            }
        }

        private long _centerFrequency = 1500000;
        public long CenterFrequency
        {
            get { return _centerFrequency; }
            set
            {
                _centerFrequency = value;
                txtCenterFrequency.Text = _centerFrequency.ToString(CultureInfo.InvariantCulture);
            }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                txtDuration.Text = _duration.ToString();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog{DefaultExt = "dat"};
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            FileName = dlg.FileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
