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
    public unsafe class SignalPlayer : IFrontendController
    {
        private bool _requestStop;
        private AutoResetEvent _isRunning;
        private readonly SignalPlayerUI _gui;

        public SignalPlayer()
        {
            _gui = new SignalPlayerUI();
        }

        public void Close()
        {
            Stop();
        }

        public long Frequency
        {
            get { return _gui.Frequency; }
            set {  }
        }

        public void HideSettingGUI()
        {
            _gui.Hide();
        }

        public bool IsSoundCardBased
        {
            get { return false; }
        }

        public void Open()
        {
        }

        public double Samplerate
        {
            get { return _gui.SampleRate; }
        }

        public void ShowSettingGUI(IWin32Window parent)
        {
            _gui.Show();
        }

        public string SoundCardHint
        {
            get { return string.Empty; }
        }
        public void Start(SamplesAvailableDelegate callback)
        {
            if (string.IsNullOrEmpty(_gui.FileName))
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
                            if (strm.Position > strm.Length - 8*bufferSize)
                            {
                                bufferSize = (int) (strm.Length - strm.Position)/8;
                                rewind = true;
                            }

                            for (var n = 0; n < bufferSize; n++)
                            {
                                buffer[n].Real = reader.ReadSingle();
                                buffer[n].Imag = reader.ReadSingle();
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

                                // Insert one second of silence so that we get a line in waterfall graph
                                /*for (var n = 0; n < maxBufferSize; n++)
                                {
                                    buffer[n].Real = 0.0001f; buffer[n].Imag = 0.0f;
                                }
                                for (var i = 0; i < Samplerate/10/maxBufferSize; i++)
                                {
                                    callback(this, ptr, bufferSize);
                                    Thread.Sleep((int)(bufferSize / _gui.SampleRate * 1000)); 
                                }*/

                                if(PowerSpectrumPanel.PowerSpectrum != null)
                                    PowerSpectrumPanel.PowerSpectrum.ResetTime();
                            }
                        }
                        _isRunning.Set();
                    }
                }
            });//.Start();
        }

        public void Stop()
        {
            _requestStop = true;
            if (_isRunning != null)
                _isRunning.WaitOne(1000);
        }
    }
}
