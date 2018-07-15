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

namespace SDRSharp.PhaseInspector
{
    public partial class PhaseInspectorPanel : UserControl
    {
        public delegate void OnShowPhaseDelegateHandler(bool show);
        public event OnShowPhaseDelegateHandler OnShowPhase;

        public delegate void OnScaleChangeDelegateHandler(int scale);
        public event OnScaleChangeDelegateHandler OnScaleChange;

        public PhaseInspectorPanel()
        {
            InitializeComponent();

            chkEnabled.Checked = Utils.GetBooleanSetting("PhaseInspector.Show");
            scaleBar.Value = Utils.GetIntSetting("PhaseInspector.Scale", 1);
        }

        public bool ShowPhase { get { return chkEnabled.Checked; } }
        public int ScaleFactor { get { return scaleBar.Value; } }

        private void OnEnabled(object sender, EventArgs e)
        {
            Utils.SaveSetting("PhaseInspector.Show", chkEnabled.Checked);

            OnShowPhase?.Invoke(chkEnabled.Checked);
        }

        private void ScaleChange(object sender, EventArgs e)
        {
            Utils.SaveSetting("PhaseInspector.Scale", scaleBar.Value);

            OnScaleChange?.Invoke(scaleBar.Value);
        }
    }
}
