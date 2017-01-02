using SDRSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRSharp.SignalFinder
{
    public class SignalFinderPlugin : ISharpPlugin
    {
        private const string _displayName = "Signal Finder";
        private SignalFinderPanel _controlPanel;
        private SignalFinderProcessor _processor;

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public System.Windows.Forms.UserControl Gui
        {
            get
            {
                return _controlPanel;
            }
        }

        public void Initialize(ISharpControl control)
        {
            _processor = new SignalFinderProcessor(control);
            _processor.Enabled = false;

            _controlPanel = new SignalFinderPanel(_processor);
        }

        public void Close()
        {
        }
    }
}
