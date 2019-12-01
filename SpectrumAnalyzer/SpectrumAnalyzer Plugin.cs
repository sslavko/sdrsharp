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
        System.Threading.Timer _timer;
        long _checkedFrequency;

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
            _drawing.OnFreqChange += ChangeFrequency;
            _drawing.SetRanges(_gui.StartFreq, _gui.EndFreq, _gui.Step);
            _drawing.Frequency = _control.Frequency;
            if (_gui.ShowSpectrum == false)
                _drawing.Hide();

            _timer = new System.Threading.Timer(state => OnScanning(), null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            _gui.OnShowSpectrum += (show) => 
            {
                if (show)
                    _drawing.Show();
                else
                    _drawing.Hide();
            };

            _gui.OnExport += Export;
            _gui.OnAutoScan += AutoScan;

            _control.RegisterFrontControl(_drawing, PluginPosition.Bottom);
            _control.RegisterStreamHook(this, ProcessorType.RawIQ);
            _control.PropertyChanged += OnPropertyChanged;

            AutoScan();
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Frequency")
                return;

            _drawing.Frequency = _control.Frequency;
        }

        private void ChangeFrequency(float frequency)
        {
            _control.SetFrequency((long)(frequency * 1000000), false);
        }

        void Export()
        {
            Export(null);
        }

        void Export(string fileName)
        {
            var data = _drawing.GetDataPoints();
            if(data.Length == 0)
                return;

            if (fileName == null)
            {
                var dlg = new System.Windows.Forms.SaveFileDialog() { DefaultExt = "csv", Filter = "CSV Files (*.csv)|*.csv" };
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileName = dlg.FileName;
            }

            if(fileName != null)
            {
                try
                {
                    using (var writer = System.IO.File.CreateText(fileName))
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
                catch(Exception ex)
                {

                }
            }
        }

        void AutoScan()
        {
            if (_gui.AutoScan)
                _timer.Change(0, _gui.AutoScanPeriod * 60000);
            else
                _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
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

            if (_control.Frequency == _checkedFrequency)
                return;

            if (_workingBuffer == null || _workingBuffer.Length < length)
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

                if (_spectrum == null || _spectrum.Length < length)
                    _spectrum = new float[length];

                fixed ( float* spectrumPtr = &_spectrum[0])
                    Fourier.SpectrumPower(workingPtr, spectrumPtr, length);
            }

            float avg = 0.0f;
            for (var n = 0; n < length; n++)
                avg += _spectrum[n];

            avg /= length;

            _drawing.AddDataPoint(_control.Frequency, avg);

            if (_control.Frequency > _gui.EndFreq * 1000000 - _gui.Step * 1000)
            {
                StopScanning();
            }
            else
            {
                if (_control.IsPlaying)
                {
                    _checkedFrequency = _control.Frequency;
                    _control.SetFrequency(_control.Frequency + _gui.Step * 1000, false);
                }
            }
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
                _checkedFrequency = 0;
                Enabled = true;
            }

            _gui.Scanning(_scanning);
        }

        private void StopScanning()
        {
            _scanning = false;
            Enabled = false;
            _gui.Scanning(_scanning);

            if(_gui.AutoScan && !string.IsNullOrWhiteSpace(_gui.AutoExportTo))
            {
                string fileName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".csv";
                Export(System.IO.Path.Combine(_gui.AutoExportTo, fileName));
            }
        }

        public double SampleRate
        {
            set { _sampleRate = value; }
        }

        public bool Enabled { get; set; }
    }
}
