using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDRSharp.Common;
using SDRSharp.Radio;

namespace SDRSharp.SpectrumAnalyzer
{
    public unsafe class SpectrumAnalyzerPlugin : ISharpPlugin, IIQProcessor
    {
        double _sampleRate;
        ISharpControl _control;
        SpectrumAnalyzerPanel _gui;
        SpectrumAnalyzerDrawing _drawing;
        bool _scanning;
        Complex[] _workingBuffer;
        float[] _window;
        float[] _spectrum;

        public void Close()
        {
            StopScanning();
        }

        public string DisplayName
        {
            get { return "Spectrum Analyzer"; }
        }

        public System.Windows.Forms.UserControl Gui
        {
            get { return _gui; }
        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
            _gui = new SpectrumAnalyzerPanel();
            _gui.OnScanning += OnScanning;

            _drawing = new SpectrumAnalyzerDrawing();
            _drawing.SetRanges(_gui.StartFreq, _gui.EndFreq, _gui.Step);
            if (_gui.ShowSpectrum == false)
                _drawing.Hide();

            _gui.OnShowSpectrum += (show) => 
            {
                if (show)
                    _drawing.Show();
                else
                    _drawing.Hide();
            };

            _gui.OnExport += Export;

            _control.RegisterFrontControl(_drawing, PluginPosition.Bottom);
            _control.RegisterStreamHook(this, ProcessorType.RawIQ);
        }

        void Export()
        {
            var data = _drawing.GetDataPoints();
            if(data.Length == 0)
                return;

            var dlg = new System.Windows.Forms.SaveFileDialog() { DefaultExt = "csv", Filter = "CSV Files (*.csv)|*.csv"};
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using(var writer = System.IO.File.CreateText(dlg.FileName))
                {
                    for (var n = 0; n < data.Length - 1; n++)
                    {
                        writer.Write(data[n].Key);
                        writer.Write(",");
                    }
                    writer.WriteLine(data[data.Length - 1].Key);
                    for (var n = 0; n < data.Length - 1; n++)
                    {
                        writer.Write(data[n].Value);
                        writer.Write(",");
                    }
                    writer.WriteLine(data[data.Length - 1].Value);
                }
            }
        }

        void OnScanning()
        {
            _scanning = !_scanning;

            if (_scanning)
                StartScanning();
            else
                StopScanning();
        }

        public void Process(Complex* buffer, int length)
        {
            if (!_scanning)
                return;

            if(_workingBuffer == null || _workingBuffer.Length != length)
                _workingBuffer = new Complex[length];

            for (var n = 0; n < length; n++)
                _workingBuffer[n] = buffer[n];

            if (_window == null || _window.Length != length)
                _window = FilterBuilder.MakeWindow(WindowType.Hamming, length);

            fixed (Complex* workingPtr = &_workingBuffer[0])
            {
                fixed (float* winPtr = &_window[0])
                    Fourier.ApplyFFTWindow(workingPtr, winPtr, length);

                Fourier.ForwardTransform(workingPtr, length);

                if (_spectrum == null || _spectrum.Length != length)
                    _spectrum = new float[length];

                fixed (float* spectrumPtr = &_spectrum[0])
                    Fourier.SpectrumPower(workingPtr, spectrumPtr, length);
            }

            float avg = 0.0f;
            var startIndex = 0;// length / 2 - 100;
            var endIndex = length;// / 2 + 100;
            for (var n = startIndex; n < endIndex; n++)
                avg += _spectrum[n];

            avg /= length;

            _drawing.AddDataPoint(_control.Frequency, avg);

            if(_control.IsPlaying)
                _control.SetFrequency(_control.Frequency + _gui.Step * 1000000, false);

            if (_control.Frequency > _gui.EndFreq * 1000000)
                StopScanning();
        }

        private void StartScanning()
        {
            if (!_control.IsPlaying || _gui.StartFreq >= _gui.EndFreq || _gui.Step == 0)
            {
                _scanning = false;
            }

            if (_scanning)
            {
                _drawing.SetRanges(_gui.StartFreq, _gui.EndFreq, _gui.Step);
                _control.SetFrequency(_gui.StartFreq * 1000000, false);
                Enabled = true;
            }

            _gui.Scanning(_scanning);
        }

        private void StopScanning()
        {
            _scanning = false;
            Enabled = false;
            _gui.Scanning(_scanning);
        }

        public double SampleRate
        {
            set { _sampleRate = value; }
        }

        public bool Enabled { get; set; }
    }
}
