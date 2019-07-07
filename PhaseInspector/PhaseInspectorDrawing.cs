using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SDRSharp.Radio;
using Timer = System.Threading.Timer;
using System.Collections.Generic;

namespace SDRSharp.PhaseInspector
{
    public unsafe partial class PhaseInspectorDrawing : UserControl
    {
        private Bitmap _drawingImage;
        private Timer _timer;
        private int _timerPeriod = 100;
        private object _lockBitmap = new object();
        private const int _matrixSize = 512;
        private const int _anglesSize = 1024;
        private float _scale = 1;

        private uint[] _angles = new uint[_anglesSize];
        private uint[,] _drawingMatrix = new uint[_matrixSize, _matrixSize];
        private uint _dataCount = 1;
        private Queue<Complex> _rawData = new Queue<Complex>();

        public PhaseInspectorDrawing()
        {
            InitializeComponent();

            _timer = new Timer(state => Invalidate(), null, 100, _timerPeriod);
        }

        public float ScaleFactor
        {
            set
            {
                _scale = value * 1000;
                Clear();
            }
        }

        public void Clear()
        {
            for (var n = 0; n < _anglesSize; n++)
                _angles[n] = 0;

            /*for (var y = 0; y < _matrixSize; y++)
            {
                for (var x = 0; x < _matrixSize; x++)
                {
                    _drawingMatrix[x, y] = 0;
                }
            }*/
            _dataCount = 1;
        }

        /*
        static byte[] rgbValues;
        public void AddPhaseLine(float[] phaseLine)
        {
            lock (_lockBitmap)
            {
                if (_bufferImage == null || _bufferImage.Width != phaseLine.Length || _bufferImage.Height != Height)
                {
                    _bufferImage = new Bitmap(phaseLine.Length, Height);
                    rgbValues = null;
                }

                using (Graphics g = Graphics.FromImage(_bufferImage))
                    g.DrawImageUnscaled(_bufferImage, 0, 1);

                var data = _bufferImage.LockBits(new Rectangle(0, 0, _bufferImage.Width, _bufferImage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, _bufferImage.PixelFormat);
                if(rgbValues == null)
                    rgbValues = new byte[data.Stride];

                System.Runtime.InteropServices.Marshal.Copy(data.Scan0, rgbValues, 0, data.Stride);

                for (int n = 0; n < phaseLine.Length; n++)
                {
                    rgbValues[n * 3] = (byte)((phaseLine[n] + Math.PI) * 40.5);
                }
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, data.Scan0, data.Stride);
                _bufferImage.UnlockBits(data);
            }
        }*/

        /*public void AddPhaseLine(Complex* phaseLine, int length)
        {
            if (_drawingImage == null)
                return;

            lock (_lockBitmap)
            {
                var scale = Math.Min(Width, Height) / 2.0f;
                var center = new Point(Width / 2, Height / 2);
                var color = Color.FromArgb(1, Color.ForestGreen);
                for(int n = 0; n < length; n++)
                {
                    _drawingImage.SetPixel(
                        (int)(phaseLine[n].Real * scale) + center.X, 
                        (int)(phaseLine[n].Imag * scale) + center.Y, 
                        color);
                }
            }
        }*/

        public void AddDataLine(Complex* dataLine, int length)
        {
            //_dataCount++;

            /*for (var y = 0; y < _matrixSize; y++)
            {
                for (var x = 0; x < _matrixSize; x++)
                {
                    _drawingMatrix[x, y]++;
                }
            }*/

            /*for(var n = 0; n < length; n++)
            {
                var x = (int)(dataLine[n].Real * _scale + _matrixSize / 2.0f + 0.5f);
                var y = (int)(dataLine[n].Imag * _scale + _matrixSize / 2.0f + 0.5f);

                if (x < 0)
                    x = 0;

                if (y < 0)
                    y = 0;

                if (x > _matrixSize - 1)
                    x = _matrixSize - 1;

                if (y > _matrixSize - 1)
                    y = _matrixSize - 1;

                _drawingMatrix[x, y]++;
            }*/

            /*for (var n = 0; n < _angles.Length; n++)
            {
                if (_angles[n] > 0)
                    _angles[n]--;
            }*/

            /*var binWidth = (_anglesSize - 1) / (Math.PI * 2);
            for (var n = 0; n < length; n++)
            {
                var angle = dataLine[n].Argument();
                var index = (int)((angle + Math.PI) * binWidth);
                _angles[index]++;
            }*/

            lock (_lockBitmap)
            {
                var re = 0.0f;
                var im = 0.0f;
                for (var n = 0; n < length; n++)
                {
                    re += dataLine[n].Real;
                    im += dataLine[n].Imag;
                };
                _rawData.Enqueue(new Complex(re / length, im / length));

                for(int n = 0; n < length; n++)
                {
                    if(dataLine[n].Real == dataLine[0].Imag)
                    {
                        int x = 1;
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
                _timer.Change(0, _timerPeriod);
            else
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        byte[] rgbValues = null;
        Complex prevPoint;
        protected override void OnPaint(PaintEventArgs e)
        {
            //lock (_lockBitmap)
            {
                /*if(_drawingImage == null)
                {
                    _drawingImage = new Bitmap(_matrixSize, _matrixSize);
                    using (var g = Graphics.FromImage(_drawingImage))
                        g.FillRectangle(Brushes.Black, 0, 0, _drawingImage.Width, _drawingImage.Height);
                }

                var data = _drawingImage.LockBits(
                    new Rectangle(0, 0, _drawingImage.Width, _drawingImage.Height), 
                    System.Drawing.Imaging.ImageLockMode.ReadWrite, 
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                int bytesCount = Math.Abs(data.Stride) * _drawingImage.Height;
                if(rgbValues == null)
                    rgbValues = new byte[bytesCount];

                System.Runtime.InteropServices.Marshal.Copy(data.Scan0, rgbValues, 0, bytesCount);
                for(var y = 0; y < _matrixSize; y++)
                {
                    for(var x = 0; x < _matrixSize; x++)
                    {
                        rgbValues[x * 3 + y * data.Stride + 1] = (byte)(255.0f * _drawingMatrix[x, y] / _dataCount);
                    }
                }
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, data.Scan0, bytesCount);

                _drawingImage.UnlockBits(data);*/

                /*using (var g = Graphics.FromImage(_drawingImage))
                {
                    g.FillRectangle(Brushes.Black, 0, 0, _drawingImage.Width, _drawingImage.Height);
                    for (var y = 0; y < _matrixSize; y++)
                    {
                        for (var x = 0; x < _matrixSize; x++)
                        {
                            if (_drawingMatrix[x, y] == 0)
                                continue;

                            var pen = new Pen(Color.FromArgb(0, _drawingMatrix[x, y], 0));
                            g.DrawLine(pen, _drawingImage.Width/2, _drawingImage.Height/2, x, y);
                        }
                    }
                }*/

                /*if (_drawingImage == null)
                {
                    _drawingImage = new Bitmap(_anglesSize, 256);
                }

                using (var g = Graphics.FromImage(_drawingImage))
                {
                    g.FillRectangle(Brushes.Black, 0, 0, _drawingImage.Width, _drawingImage.Height);
                    for (var n = 0; n < _angles.Length; n++)
                    {
                        var y = 256.0f * _dataCount / _angles[n] * _scale;
                        g.DrawLine(Pens.ForestGreen, n, 255, n, 256 - y);
                    }
                }*/

                if (_drawingImage == null)
                {
                    _drawingImage = new Bitmap(Width, Height);
                    using (var g = Graphics.FromImage(_drawingImage))
                    {
                        g.FillRectangle(Brushes.Black, 0, 0, _drawingImage.Width, _drawingImage.Height);
                    }
                }

                lock (_lockBitmap)
                {
                    if(_rawData.Count > 0)
                    {
                        using (var g = Graphics.FromImage(_drawingImage))
                        {
                            g.DrawImageUnscaled(_drawingImage, -_rawData.Count, 0);
                            g.FillRectangle(Brushes.Black, _drawingImage.Width - _rawData.Count, 0, _drawingImage.Width, _drawingImage.Height);

                            int index = 0;
                            Complex? dataPoint = null;
                            var x = prevPoint == null ? 0 : _drawingImage.Width - _rawData.Count;
                            if (x < 0)
                                x = 0;
                            if (x > _drawingImage.Width - 1)
                                x = _drawingImage.Width - 1;

                            var y1Re = prevPoint == null ? 0 : (int)(_drawingImage.Height / 2.0f - prevPoint.Real * _scale + 0.5f);
                            var y1Im = prevPoint == null ? 0 : (int)(_drawingImage.Height / 2.0f - prevPoint.Imag * _scale + 0.5f);
                            if (y1Re < 0)
                                y1Re = 0;
                            if (y1Re > _drawingImage.Height)
                                y1Re = _drawingImage.Height;
                            if (y1Im < 0)
                                y1Im = 0;
                            if (y1Im > _drawingImage.Height)
                                y1Im = _drawingImage.Height;

                            while (_rawData.Count > 0)
                            {
                                dataPoint = _rawData.Dequeue();
                                var y2Re = (int)(_drawingImage.Height / 2.0f - dataPoint.Value.Real * _scale + 0.5f);
                                var y2Im = (int)(_drawingImage.Height / 2.0f - dataPoint.Value.Imag * _scale + 0.5f);
                                if (y2Re < 0)
                                    y2Re = 0;
                                if (y2Re > _drawingImage.Height)
                                    y2Re = _drawingImage.Height;
                                if (y2Im < 0)
                                    y2Im = 0;
                                if (y2Im > _drawingImage.Height)
                                    y2Im = _drawingImage.Height;

                                g.DrawLine(Pens.Green, x + index, y1Re, x + index + 1, y2Re);
                                g.DrawLine(Pens.Blue, x + index, y1Im, x + index + 1, y2Im);

                                y1Re = y2Re;
                                y1Im = y2Im;

                                index++;
                            }
                            prevPoint = dataPoint.Value;
                        }
                    }
                }
                e.Graphics.DrawImageUnscaled(_drawingImage, 0, 0);
            }
        }
    }
}
