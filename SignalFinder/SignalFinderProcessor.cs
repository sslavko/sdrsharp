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
        Complex[] _workingBuffer;
        float[] _window;
        float[] _spectrum;
        float[] _avgData;

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

        public unsafe void Process(Complex* buffer, int length)
        {
            if (_workingBuffer == null || _workingBuffer.Length < length)
                _workingBuffer = new Complex[length];

            for (var n = 0; n < length; n++)
                _workingBuffer[n] = buffer[n];

            if (_window == null || _window.Length < length)
                _window = FilterBuilder.MakeWindow(WindowType.Hamming, length);

            fixed (Complex* workingPtr = _workingBuffer)
            {
                fixed (float* winPtr = _window)
                    Fourier.ApplyFFTWindow(workingPtr, winPtr, length);

                Fourier.ForwardTransform(workingPtr, length);

                if (_avgData == null || _spectrum == null || _spectrum.Length < length)
                {
                    _spectrum = new float[length];
                    _avgData = new float[length];
                }

                fixed (float* spectrumPtr = _spectrum)
                    Fourier.SpectrumPower(workingPtr, spectrumPtr, length);

                for (var n = 0; n < length; n++)
                {
                    _avgData[n] = _avgData[n] * 0.95f + _spectrum[n] * 0.05f;
                }
            }

            _display.AddDataLine(_avgData);
        }
    }
}
