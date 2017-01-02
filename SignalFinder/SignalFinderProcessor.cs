using SDRSharp.Common;
using SDRSharp.PanView;
using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRSharp.SignalFinder
{
    public class SignalFinderProcessor : IIQProcessor
    {
        double _sampleRate;
        bool _enabled;
        ISharpControl _control;
        SignalFinderDisplay _display;

        public SignalFinderProcessor(ISharpControl control)
        {
            _control = control;

            _display = new SignalFinderDisplay();

            _control.RegisterStreamHook(this, Radio.ProcessorType.RawIQ);
            _control.RegisterFrontControl(_display, PluginPosition.Bottom);
            _display.Visible = true;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                _display.Visible = _enabled;
            }
        }

        public double SampleRate
        {
            set
            {
                _sampleRate = value;
            }
        }

        float[] buf = new float[1024];
        public unsafe void Process(Complex* buffer, int length)
        {
            for (var n = 0; n < 1024; n++)
                buf[n] = buffer[n].ModulusSquared();

            fixed (float* pf = buf)
            {
                //_s.Render(pf, 1024);
            }
        }
    }
}
