using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDRSharp.Radio;

namespace SDRSharp.SpectrumAnalyzer
{
    public partial class SpectrumAnalyzerPanel : UserControl
    {
        public delegate void OnScanningDelegateHandler();
        public event OnScanningDelegateHandler OnScanning;

        public delegate void OnExportDelegateHandler();
        public event OnExportDelegateHandler OnExport;

        public delegate void OnShowSpectrumDelegateHandler(bool show);
        public event OnShowSpectrumDelegateHandler OnShowSpectrum;

        public SpectrumAnalyzerPanel()
        {
            InitializeComponent();

            numStart.Value = Utils.GetLongSetting("SpectrumAnalyzer.StartFreq", 1);
            numEnd.Value = Utils.GetLongSetting("SpectrumAnalyzer.EndFreq", 10);
            numStep.Value = Utils.GetLongSetting("SpectrumAnalyzer.Step", 1);
            chkShowSpectrum.Checked = Utils.GetBooleanSetting("SpectrumAnalyzer.ShowSpectrum");
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            Utils.SaveSetting("SpectrumAnalyzer.StartFreq", numStart.Value);
            Utils.SaveSetting("SpectrumAnalyzer.EndFreq", numEnd.Value);
            Utils.SaveSetting("SpectrumAnalyzer.Step", numStep.Value);

            if (OnScanning != null)
                OnScanning();
        }

        public void Scanning(bool scanning)
        {
            if (scanning)
                btnScan.Text = "Stop";
            else
                btnScan.Text = "Scan";
        }

        public long StartFreq { get { return (uint)numStart.Value; } }

        public long EndFreq { get { return (uint)numEnd.Value; } }

        public long Step { get { return (uint)numStep.Value; } }

        private void ShowSpectrumChanged(object sender, EventArgs e)
        {
            Utils.SaveSetting("SpectrumAnalyzer.ShowSpectrum", chkShowSpectrum.Checked);

            if (OnShowSpectrum != null)
                OnShowSpectrum(chkShowSpectrum.Checked);
        }

        public bool ShowSpectrum { get { return chkShowSpectrum.Checked; } }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (OnExport != null)
                OnExport();
        }
    }
}
