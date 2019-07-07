using System;
using System.Collections.Generic;
using SDRSharp.Common;
using SDRSharp.Radio;

namespace SDRSharp.PhaseInspector
{
    public unsafe class PhaseInspectorPlugin : ISharpPlugin, IIQProcessor
    {
        ISharpControl _control;
        PhaseInspectorPanel _gui;
        PhaseInspectorDrawing _drawing;
        double _sampleRate;

        Complex[] _workingBuffer;
        float[] _window;

        public string DisplayName
        {
            get
            {
                return "Phase Inspector";
            }
        }

        public bool Enabled { get; set; }

        public System.Windows.Forms.UserControl Gui
        {
            get
            {
                return _gui;
            }
        }

        public double SampleRate
        {
            set
            {
                _sampleRate = value;
            }
        }

        public void Close()
        {
            
        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
            _gui = new PhaseInspectorPanel();
            _drawing = new PhaseInspectorDrawing();

            if (_gui.ShowPhase)
            {
                Enabled = true;
            }
            else
            {
                _drawing.Hide();
            }
            _drawing.ScaleFactor = _gui.ScaleFactor;

            _gui.OnShowPhase += (show) =>
            {
                if (show)
                {
                    _drawing.Clear();
                    _drawing.Show();
                }
                else
                {
                    _drawing.Hide();
                }

                Enabled = show;
            };

            _gui.OnScaleChange += (scale) =>
            {
                _drawing.ScaleFactor = scale;
            };

            _control.PropertyChanged += (sender, propArgs) => 
            {
                if (propArgs.PropertyName == "Frequency")
                    _drawing.Clear();
            };
            control.RegisterStreamHook(this, ProcessorType.RawIQ);
            control.RegisterFrontControl(_drawing, PluginPosition.Bottom);
        }

        public void Process(Complex* buffer, int length)
        {
            /*if (_workingBuffer == null || _workingBuffer.Length < length)
                _workingBuffer = new Complex[length];

            for (var n = 0; n < length; n++)
                _workingBuffer[n] = buffer[n];

            if (_window == null || _window.Length < length)
                _window = FilterBuilder.MakeWindow(WindowType.Hamming, length);

            fixed (Complex* workingPtr = &_workingBuffer[0])
            {
                fixed (float* winPtr = &_window[0])
                    Fourier.ApplyFFTWindow(workingPtr, winPtr, length);

                Fourier.ForwardTransform(workingPtr, length);
                _drawing.AddDataLine(workingPtr, length);
            }*/

            _drawing.AddDataLine(buffer, length);
        }
    }
}
