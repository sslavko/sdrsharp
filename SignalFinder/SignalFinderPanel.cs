using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDRSharp.SignalFinder
{
    public partial class SignalFinderPanel : UserControl
    {
        SignalFinderProcessor _processor;

        public SignalFinderPanel(SignalFinderProcessor processor)
        {
            _processor = processor;

            InitializeComponent();
        }

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            _processor.Enabled = chkEnable.Checked;
        }

        private void barScale_Scroll(object sender, EventArgs e)
        {
            _processor.Scale = barScale.Value;
        }

        private void barOffset_Scroll(object sender, EventArgs e)
        {
            _processor.Offset = barOffset.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _processor.UseWindow = checkBox1.Checked;
        }
    }
}
