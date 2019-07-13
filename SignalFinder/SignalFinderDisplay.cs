using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace SDRSharp.SignalFinder
{
    public partial class SignalFinderDisplay : UserControl
    {
        private Timer _timer;
        private int _timerPeriod = 50;
        private Bitmap _drawingImage;
        private Bitmap _bufferImage;
        private Object _dataLock = new Object();
        private byte[] _colorBuffer;
        private Queue<float[]> _dataBuffer = new Queue<float[]>();

        public SignalFinderDisplay()
        {
            InitializeComponent();

            _timer = new Timer(state => Invalidate(), null, 100, _timerPeriod);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_drawingImage == null)
                return;

            if (_dataBuffer.Count > 0)
            {
                lock (_dataLock)
                {
                    var lines = _dataBuffer.Count;

                    using (Graphics g = Graphics.FromImage(_drawingImage))
                    {
                        // Shift image down
                        g.DrawImage(_drawingImage, 0, lines);

                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                        for (var n = lines - 1; n >= 0; n--)
                        {
                            var dataLine = _dataBuffer.Dequeue();
                            if (_bufferImage == null || _bufferImage.Width != dataLine.Length)
                                _bufferImage = new Bitmap(dataLine.Length, 1);

                            var rect = new Rectangle(0, 0, dataLine.Length, 1);
                            var bmpData = _bufferImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, _bufferImage.PixelFormat);
                            var bytes = Math.Abs(bmpData.Stride);
                            if (_colorBuffer == null || _colorBuffer.Length != bytes)
                                _colorBuffer = new byte[bytes];

                            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, _colorBuffer, 0, bytes);
                            for (var i = 0; i < dataLine.Length; i++)
                            {
                                var b = (int)(dataLine[i]);
                                if (b < 0)
                                    b = 0;
                                if (b > 0xff)
                                    b = 0xff;

                                _colorBuffer[i * 4] = (byte)(b * 0.5f);      // Blue
                                _colorBuffer[i * 4 + 1] = (byte)(b * 0.8f);  // Green
                                _colorBuffer[i * 4 + 2] = (byte)(b * 1f);  // Red
                                _colorBuffer[i * 4 + 3] = 0xff;     // Alpha
                            }
                            System.Runtime.InteropServices.Marshal.Copy(_colorBuffer, 0, bmpData.Scan0, bytes);
                            _bufferImage.UnlockBits(bmpData);
                            var destRect = new Rectangle(0, n, _drawingImage.Width, 1);
                            g.DrawImage(_bufferImage, destRect, rect, GraphicsUnit.Pixel);
                            //g.DrawImage(_bufferImage, 0, n);
                        }
                    }
                }
            }

            e.Graphics.DrawImageUnscaled(_drawingImage, 0, 0);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            CreateBitmap();
        }

        private void CreateBitmap()
        {
            if (_drawingImage == null || _drawingImage.Width != Width || _drawingImage.Height != Height)
                _drawingImage = new Bitmap(Width, Height);

            using (Graphics g = Graphics.FromImage(_drawingImage))
            {
                g.FillRectangle(Brushes.Black, 0, 0, _drawingImage.Width, _drawingImage.Height);
            }
        }

        public void AddDataLine(float[] dataLine)
        {
            var buf = new float[dataLine.Length];
            for (var n = 0; n < dataLine.Length; n++)
                buf[n] = dataLine[n];

            lock (_dataLock)
            {
                _dataBuffer.Enqueue(buf);
            }
        }
    }
}
