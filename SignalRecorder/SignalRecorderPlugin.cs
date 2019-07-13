using System;
using System.IO;
using System.Windows.Forms;
using SDRSharp.Common;
using SDRSharp.Radio;
using System.ComponentModel;

namespace SDRSharp.SignalRecorder
{
    public unsafe class SignalRecorderPlugin : ISharpPlugin, IIQProcessor
    {
        private SignalRecorderPanel _controlPanel;
        private PowerSpectrumPanel _powerSpectrumPanel;
        private BinaryWriter _writer;
        private bool _recording;
        private bool _manualRecording;
        private double _sampleRate;
        private int _fileCounter;
        private int _onhold;
        private Complex[] _buffer;
        private int _bufferPos;
        private Complex[] _workingBuffer;
        private float[] _window;
        private int _powerTriggerCount;

        public const int MaxDataRange = 64;//8096;
        public const int BytesPerSample = 8;//2;

        public string DisplayName
        {
            get { return "Signal Recorder"; }
        }

        public UserControl Gui
        {
            get { return _controlPanel; }
        }

        public bool HasGui
        {
            get { return true; }
        }

        public UserControl GuiControl
        {
            get { return _controlPanel; }
        }

        internal static ISharpControl MainControl;
        public void Initialize(ISharpControl control)
        {
            MainControl = control;

            control.PropertyChanged += ControlOnPropertyChanged;

            _controlPanel = new SignalRecorderPanel();
            _controlPanel.PowerMonitorChanged += OnPowerMonitorChanged;
            _controlPanel.ManualRecording += OnManualRecording;

            _powerSpectrumPanel = new PowerSpectrumPanel(_controlPanel);
            OnPowerMonitorChanged();

            control.RegisterFrontControl(_powerSpectrumPanel, PluginPosition.Bottom);
            control.RegisterStreamHook(this, ProcessorType.RawIQ);

        }

        private void OnPowerMonitorChanged()
        {
            if (_controlPanel.PowerMonitorEnabled)
            {
                if (!_powerSpectrumPanel.Visible)
                    _powerSpectrumPanel.Show();
            }
            else
            {
                if (_powerSpectrumPanel.Visible)
                {
                    _powerSpectrumPanel.Hide();
                    _powerSpectrumPanel.Restart();
                }
            }
        }

        private void OnManualRecording(bool record)
        {
            _manualRecording = record;
        }

        private void ControlOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "StopRadio":
                case "Frequency":
                    Stop();
                    break;
                case "StartRadio":
                    _powerSpectrumPanel.Restart();
                    break;
            }
        }

        void StartNewFile()
        {
            if (!Directory.Exists(_controlPanel.OutputFolder))
                Directory.CreateDirectory(_controlPanel.OutputFolder);

            var time = DateTime.Now;
            var outFolder = Path.Combine(_controlPanel.OutputFolder, time.ToString("dd.MMMMM"));

            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            _writer =
                new BinaryWriter(
                    File.OpenWrite(Path.Combine(outFolder, time.ToString("ddMMyyyy_HHmmss") + ".dat")));

            _writer.Write(time.ToBinary());
            _writer.Write(_sampleRate);
            _writer.Write(MainControl.CenterFrequency);
            _writer.Write(MainControl.Frequency);

            _fileCounter++;
        }

        public double SampleRate
        {
            set { _sampleRate = value; }
        }

        bool Recording
        {
            get { return _recording; }
            set
            {   
                _recording = value;
                if (_controlPanel != null)
                    _controlPanel.SetStatusText(_recording ? "Recording" : "Files recorded: " + _fileCounter);
            }
        }

        public bool Enabled
        {
            get { return RecordingEnabled || _powerSpectrumPanel.Visible; }
            set { }
        }

        public bool RecordingEnabled
        {
            get { return !string.IsNullOrEmpty(_controlPanel.OutputFolder) && _controlPanel.RecordingEnabled; }
            set
            {
                if (value == false)
                    Stop();
            }
        }

        private bool _squelchOpen;

        private bool SquelchOpen
        {
            get { return _squelchOpen; }
            set
            {
                _squelchOpen = value;
                _controlPanel.SetSquelchOpen(value);
            }
        }

        public int NextPowerOfTwo(int num)
        {
            var res = 1;

            while (res < num)
                res *= 2;

            return res;
        }

        private void CalculatePower(Complex* buffer, int pos, int length)
        {
            var fftSize = NextPowerOfTwo(length);

            if (_window == null || _window.Length != fftSize)
                _window = FilterBuilder.MakeWindow(WindowType.Hamming, fftSize);

            if (_workingBuffer == null || _workingBuffer.Length != fftSize)
            {
                _workingBuffer = new Complex[fftSize];
                _powerSpectrumPanel.PointsPerSecond = (ulong)(_sampleRate / length + 0.5);
            }

            for (int n = 0; n < fftSize; n++)
            {
                if (n < length)
                    _workingBuffer[n] = buffer[pos + n];
                else
                    _workingBuffer[n] = 0;
            }

            fixed (Complex* workingPtr = &_workingBuffer[0])
            {
                fixed (float* winPtr = &_window[0])
                    Fourier.ApplyFFTWindow(workingPtr, winPtr, fftSize);

                Fourier.ForwardTransform(workingPtr, fftSize);
            }

            var herzPerBin = _sampleRate / fftSize;
            var offsetHerz = MainControl.Frequency - MainControl.CenterFrequency;
            var offsetBins = (int)(offsetHerz / herzPerBin + 0.5);
            var freqBinIndex = fftSize / 2 + offsetBins;
            var avgInBins = (int)(MainControl.FilterBandwidth / herzPerBin + 0.5);
            if (avgInBins == 0)
                avgInBins = 1;

            int startIndex;
            int endIndex;
            switch (MainControl.DetectorType)
            {
                case DetectorType.LSB:
                    startIndex = freqBinIndex - avgInBins;
                    endIndex = freqBinIndex;
                    break;
                case DetectorType.USB:
                    startIndex = freqBinIndex;
                    endIndex = freqBinIndex + avgInBins;
                    break;
                default:
                    startIndex = (int) (freqBinIndex - avgInBins/2.0f + 0.5);
                    endIndex = (int) (freqBinIndex + avgInBins/2.0f + 0.5);
                    break;
            }

            if (startIndex < 0)
                startIndex = 0;

            if (endIndex >= _workingBuffer.Length)
                endIndex = _workingBuffer.Length;

            var totalPower = 0.0f;
            for (var n = startIndex; n < endIndex; n++)
            {
                //CheckMinMax(_workingBuffer[n]);
                totalPower += (float)(10.0 * Math.Log10(1e-60 + (_workingBuffer[n].Real * _workingBuffer[n].Real + _workingBuffer[n].Imag * _workingBuffer[n].Imag)));
            }

            var fftGain = (float)(10.0 * Math.Log10(fftSize / 2.0));
            var compensation = 24.0f - fftGain - 40.0f;

            var dataPointValue = totalPower/avgInBins + compensation;

            if (dataPointValue >= _controlPanel.SquelchValue)
                _powerTriggerCount++;
            else
                _powerTriggerCount = 0;

            SquelchOpen = _powerTriggerCount > 1 || _manualRecording;

            if(_powerSpectrumPanel.Visible)
                _powerSpectrumPanel.Draw(dataPointValue);
        }

        /*float minData = 1000, maxData = -1000;
        private void CheckMinMax(Complex c)
        {
            if (minData > c.Real)
                minData = c.Real;
            if (minData > c.Imag)
                minData = c.Imag;
            if (maxData < c.Real)
                maxData = c.Real;
            if (maxData < c.Imag)
                maxData = c.Imag;
        }*/

        public void Process(Complex* buffer, int length)
        {
            for (var n = 0; n < _controlPanel.TimeStretch; n++)
                CalculatePower(buffer, n * length / _controlPanel.TimeStretch, length / _controlPanel.TimeStretch);

            try
            {
                if (SquelchOpen || _manualRecording)
                {
                    _onhold = 0;

                    if (!Recording && RecordingEnabled)
                    {
                        // Create file, write previous buffer and then write this buffer
                        StartNewFile();
                        Recording = true;

                        if (_buffer != null)
                        {
                            for (var n = _bufferPos; n < _buffer.Length; n++)
                            {
                                _writer.Write(Quantize(_buffer[n].Real));
                                _writer.Write(Quantize(_buffer[n].Imag));
                            }
                            for (var n = 0; n < _bufferPos; n++)
                            {
                                _writer.Write(Quantize(_buffer[n].Real));
                                _writer.Write(Quantize(_buffer[n].Imag));
                            }
                            _bufferPos = 0;
                        }
                    }
                }
                else
                {
                    if (Recording)
                    {
                        _onhold += length;
                    }
                    else
                    {
                        if (_buffer == null || _buffer.Length != (int)_sampleRate)
                            _buffer = new Complex[(int)_sampleRate];

                        for (var n = 0; n < length; n++)
                        {
                            _buffer[_bufferPos++] = buffer[n];
                            if (_bufferPos >= _buffer.Length)
                                _bufferPos = 0;
                        }
                    }
                }

                if (Recording)
                {
                    for (var n = 0; n < length; n++)
                    {
                        _writer.Write(Quantize(buffer[n].Real));
                        _writer.Write(Quantize(buffer[n].Imag));
                    }

                    if (_onhold > (int)_sampleRate * 2)
                        Stop();
                }
            }
            catch (Exception)
            {
                Stop();
            }
        }
        
        public void Close()
        {
            Stop();
        }

        public void Stop()
        {
            Recording = false;

            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer = null;
            }

            _bufferPos = 0;
            if (_buffer != null)
            {
                for (var n = 0; n < _buffer.Length; n++)
                    _buffer[n] = 0;
            }
        }

        float Quantize(float x)
        {
            return x;
        }

        /*byte Quantize(float x)
        {
            x = (x + MaxDataRange) / (2 * MaxDataRange);
            x = (int)Math.Floor(256 * x);
            if (x < 0) 
                return 0;
            else if (x > 255) 
                return 255;
            
            return (byte)x;
        }*/
    }
}
