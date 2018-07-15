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
        private int _scale = 256;

        private byte[] _angles = new byte[_anglesSize];
        private byte[,] _drawingMatrix = new byte[_matrixSize, _matrixSize];

        public PhaseInspectorDrawing()
        {
            InitializeComponent();

            _timer = new Timer(state => Invalidate(), null, 100, _timerPeriod);
        }

        public int ScaleFactor { set { _scale = 256 * value; } }

        public void Clear()
        {
            for (var n = 0; n < _anglesSize; n++)
                _angles[n] = 0;
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
            for (var y = 0; y < _matrixSize; y++)
            {
                for (var x = 0; x < _matrixSize; x++)
                {
                    if (_drawingMatrix[x, y] > 0)
                        _drawingMatrix[x, y]--;
                }
            }

            for(var n = 0; n < length; n++)
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

                if(_drawingMatrix[x, y] < 255)
                    _drawingMatrix[x, y]++;
            }

            /*for (var n = 0; n < _angles.Length; n++)
            {
                if (_angles[n] > 0)
                    _angles[n]--;
            }

            var bin = (_anglesSize - 1) / (Math.PI * 2);
            for (var n = 0; n < length; n++)
            {
                var angle = dataLine[n].Argument();
                var index = (int)((angle + Math.PI) * bin);
                if(_angles[index] < 255)
                    _angles[index]++;
            }*/
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
        protected override void OnPaint(PaintEventArgs e)
        {
            //lock (_lockBitmap)
            {
                if(_drawingImage == null)
                {
                    _drawingImage = new Bitmap(_matrixSize, _matrixSize);
                    using (var g = Graphics.FromImage(_drawingImage))
                        g.FillRectangle(Brushes.Black, 0, 0, _drawingImage.Width, _drawingImage.Height);
                }

                /*var data = _drawingImage.LockBits(
                    new Rectangle(0, 0, _matrixSize, _matrixSize), 
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
                        rgbValues[x * 3 + y * data.Stride + 1] = _drawingMatrix[x, y];
                    }
                }
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, data.Scan0, bytesCount);

                _drawingImage.UnlockBits(data);*/

                using (var g = Graphics.FromImage(_drawingImage))
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
                }

                /*using (var g = Graphics.FromImage(_drawingImage))
                {
                    g.FillRectangle(Brushes.Black, 0, 0, _drawingImage.Width, _drawingImage.Height);
                    for (var n = 0; n < _angles.Length; n++)
                    {
                        if (_angles[n] == 0)
                            continue;

                        g.DrawLine(Pens.ForestGreen, n, 256, n, 256 - _angles[n]);
                    }
                }*/

                e.Graphics.DrawImageUnscaled(_drawingImage, 0, 0);
            }
        }
    }
}
