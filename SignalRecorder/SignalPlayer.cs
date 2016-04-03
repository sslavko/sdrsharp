using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SDRSharp.Common;
using SDRSharp.Radio;

namespace SDRSharp.SignalRecorder
{
    public unsafe class SignalPlayer : IFrontendController, IFloatingConfigDialogProvider, ITunableSource, IIQStreamController
    {
        private bool _requestStop;
        private AutoResetEvent _isRunning;
        private readonly SignalPlayerUI _gui;

        public SignalPlayer()
        {
            _gui = new SignalPlayerUI();
        }

        // IFrontendController
        public void Open()
        {
        }

        public void Close()
        {
            Stop();
        }

        // IFloatingConfigDialogProvider
        public void HideSettingGUI()
        {
            _gui.Hide();
        }
        
        public void ShowSettingGUI(IWin32Window parent)
        {
            _gui.Show();
        }

        // ITunableSource
        public long Frequency
        {
            get { return _gui.Frequency; }
            set { }
        }

        // IIQStreamController
        public double Samplerate
        {
            get { return _gui.SampleRate; }
        }

        public void Start(SamplesAvailableDelegate callback)
        {
            if (string.IsNullOrEmpty(_gui.FileName))
                return;

            if (!File.Exists(_gui.FileName))
                return;

            _requestStop = false;

            SDRSharp.Radio.DSPThreadPool.QueueUserWorkItem(delegate
            {
                _isRunning = new AutoResetEvent(false);
                const int headerSize = 32;
                const int maxBufferSize = 4096*2;
                var bufferSize = maxBufferSize;
                var buffer = new Complex[bufferSize];
                fixed (Complex* ptr = &buffer[0])
                {
                    using (var strm = File.OpenRead(_gui.FileName))
                    using (var reader = new BinaryReader(strm))
                    {
                        strm.Seek(headerSize, SeekOrigin.Begin); // Skip header
                        var rewind = false;
                        while (true)
                        {
                            var start = DateTime.Now;
                            if (strm.Position > strm.Length - SignalRecorderPlugin.BytesPerSample *bufferSize)
                            {
                                bufferSize = (int)(strm.Length - strm.Position) / SignalRecorderPlugin.BytesPerSample;
                                rewind = true;
                            }

                            for (var n = 0; n < bufferSize; n++)
                            {
                                switch (SignalRecorderPlugin.BytesPerSample)
                                {
                                    case 2:
                                        byte r = reader.ReadByte();
                                        byte i = reader.ReadByte();
                                        buffer[n].Real = (r * 2 - 1) * SignalRecorderPlugin.MaxDataRange;
                                        buffer[n].Imag = (i * 2 - 1) * SignalRecorderPlugin.MaxDataRange;
                                        break;
                                    case 8:
                                        buffer[n].Real = reader.ReadSingle();
                                        buffer[n].Imag = reader.ReadSingle();
                                        break;
                                }
                            }

                            if (_requestStop)
                                break;

                            if (bufferSize > 0)
                                callback(this, ptr, bufferSize);

                            var t = DateTime.Now - start;
                            var s = TimeSpan.FromMilliseconds(bufferSize/_gui.SampleRate*1000);
                            if (s > t)
                                Thread.Sleep(s - t);

                            if (rewind)
                            {
                                rewind = false;
                                bufferSize = maxBufferSize;
                                strm.Seek(headerSize, SeekOrigin.Begin); // Skip header

                                if(PowerSpectrumPanel.PowerSpectrum != null)
                                    PowerSpectrumPanel.PowerSpectrum.ResetTime();
                            }
                        }
                        _isRunning.Set();
                    }
                }
            });
        }

        public void Stop()
        {
            _requestStop = true;
            if (_isRunning != null)
                _isRunning.WaitOne(1000);
        }
    }
}
