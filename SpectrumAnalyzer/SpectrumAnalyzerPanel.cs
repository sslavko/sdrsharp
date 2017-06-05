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

        public delegate void OnAutoScanDelegateHandler();
        public event OnAutoScanDelegateHandler OnAutoScan;

        public SpectrumAnalyzerPanel()
        {
            InitializeComponent();

            numStart.Value = Utils.GetLongSetting("SpectrumAnalyzer.StartFreq", 1);
            numEnd.Value = Utils.GetLongSetting("SpectrumAnalyzer.EndFreq", 10);
            numStep.Value = Utils.GetLongSetting("SpectrumAnalyzer.Step", 1000);
            chkShowSpectrum.Checked = Utils.GetBooleanSetting("SpectrumAnalyzer.ShowSpectrum");
            chkAutoScan.Checked = Utils.GetBooleanSetting("SpectrumAnalyzer.AutoScan");
            numPeriod.Value = Utils.GetLongSetting("SpectrumAnalyzer.AutoScanPeriod", 1);
            txtExportTo.Text = Utils.GetStringSetting("SpectrumAnalyzer.AutoScanFolder", "");
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            Utils.SaveSetting("SpectrumAnalyzer.StartFreq", numStart.Value);
            Utils.SaveSetting("SpectrumAnalyzer.EndFreq", numEnd.Value);
            Utils.SaveSetting("SpectrumAnalyzer.Step", numStep.Value);

            OnScanning?.Invoke();
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

        public bool AutoScan { get { return chkAutoScan.Checked; } }

        public string AutoExportTo { get { return txtExportTo.Text; } }

        public int AutoScanPeriod { get { return (int)numPeriod.Value; } }

        private void ShowSpectrumChanged(object sender, EventArgs e)
        {
            Utils.SaveSetting("SpectrumAnalyzer.ShowSpectrum", chkShowSpectrum.Checked);

            OnShowSpectrum?.Invoke(chkShowSpectrum.Checked);
        }

        public bool ShowSpectrum { get { return chkShowSpectrum.Checked; } }

        private void btnExport_Click(object sender, EventArgs e)
        {
            OnExport?.Invoke();
        }

        private void OnAutoScanChange(object sender, EventArgs e)
        {
            numPeriod.Enabled = chkAutoScan.Checked;
            txtExportTo.Enabled = chkAutoScan.Checked;

            Utils.SaveSetting("SpectrumAnalyzer.AutoScan", chkAutoScan.Checked);
            Utils.SaveSetting("SpectrumAnalyzer.AutoScanPeriod", numPeriod.Value);

            OnAutoScan?.Invoke();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtExportTo.Text = dlg.SelectedPath;
                Utils.SaveSetting("SpectrumAnalyzer.AutoScanFolder", txtExportTo.Text);
            }
        }
    }
}
