using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace SDRSharp.SpectrumAnalyzer
{
    public partial class SpectrumAnalyzerDrawing : UserControl
    {
        private Bitmap _bufferImage;
        private Bitmap _drawingImage;
        private Timer _timer;
        private int _timerPeriod = 100;
        private Dictionary<long, float> _dataPoints;
        private object _lockBitmap = new object();
        private long _minFreq;
        private long _maxFreq;
        private long _step;
        private Font _gridFont;
        private Pen _gridPen;
        private Point _mousePos;
        private float _freqHover;

        public delegate void OnFreqChangeHandler(float frequency);
        public event OnFreqChangeHandler OnFreqChange;

        public SpectrumAnalyzerDrawing()
        {
            InitializeComponent();

            _dataPoints = new Dictionary<long, float>();
            _timer = new Timer(state => Invalidate(), null, 100, _timerPeriod);
            _gridFont = new Font("Arial", 8f);
            _gridPen = new Pen(Color.Gray, 1.0f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_bufferImage == null)
                return;

            lock (_lockBitmap)
            {
                if (_drawingImage == null || _drawingImage.Size != _bufferImage.Size)
                    _drawingImage = new Bitmap(_bufferImage.Width, _bufferImage.Height);

                using(var g = Graphics.FromImage(_drawingImage))
                {
                    g.DrawImageUnscaled(_bufferImage, 0, 0); // Grid and frequences
                    g.FillRectangle(Brushes.Black, 0, 0, 19, Height - 20);

                    var range = (_maxFreq - _minFreq);
                    var mhzPerPixel = range / (Width - 40.0f);

                    KeyValuePair<long, float>[] dataPoints;
                    lock (_dataPoints)
                        dataPoints = _dataPoints.ToArray();

                    float maxVal = float.MinValue;
                    float minVal = float.MaxValue;
                    for (var n = 0; n < dataPoints.Length; n++)
                    {
                        if (maxVal < dataPoints[n].Value)
                            maxVal = dataPoints[n].Value;

                        if (minVal > dataPoints[n].Value)
                            minVal = dataPoints[n].Value;
                    }

                    var scale = (Height - 20f) / (maxVal - minVal);
                    if (float.IsNaN(scale) || float.IsInfinity(scale))
                        scale = 1.0f;

                    long lastX = 20;
                    int lastY = dataPoints.Length > 0 ? Height - (int)((dataPoints[0].Value - minVal) * scale) - 20 : 0;
                    for (var n = 1; n < dataPoints.Length; n++)
                    {
                        var x = 20 + (int)((dataPoints[n].Key / 1000000f - _minFreq) / mhzPerPixel);
                        if (lastX != x)
                        {
                            int y = Height - (int)((dataPoints[n].Value - minVal) * scale) - 20;
                            g.DrawLine(Pens.Cyan, lastX, lastY, x, y);  // Live data
                            lastX = x;
                            lastY = y;
                        }
                    }

                    //var dbRange = maxVal - minVal;
                    var minValStr = ((int)minVal).ToString();
                    var maxValStr = ((int)maxVal).ToString();
                    var minSize = g.MeasureString(minValStr, _gridFont);
                    var maxSize = g.MeasureString(maxValStr, _gridFont);

                    g.DrawString(minValStr, _gridFont, Brushes.White, 20 - minSize.Width, Height - 20 - minSize.Height);
                    g.DrawString(maxValStr, _gridFont, Brushes.White, 20 - maxSize.Width, 0);

                    if (_mousePos.X > 20)
                    {
                        g.DrawLine(Pens.Green, _mousePos.X, 0, _mousePos.X, Height - 20);
                        _freqHover = _minFreq + (_mousePos.X - 20) * mhzPerPixel;
                        g.DrawString(_freqHover.ToString(), _gridFont, Brushes.White, 25, 10);
                    }
                }

                e.Graphics.DrawImageUnscaled(_drawingImage, 0, 0);
            }
        }

        public KeyValuePair<long, float>[] GetDataPoints()
        {
            lock(_dataPoints)
                return _dataPoints.ToArray();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            CreateBitmap();
        }

        public void AddDataPoint(long freq, float val)
        {
            lock (_dataPoints)
                _dataPoints.Add(freq, val);
        }

        public void SetRanges(long minFreq, long maxFreq, long step)
        {
            _dataPoints.Clear();

            _minFreq = minFreq;
            _maxFreq = maxFreq;
            _step = step;

            CreateBitmap();
        }

        private void CreateBitmap()
        {
            lock (_lockBitmap)
            {
                if (_bufferImage == null || _bufferImage.Width != Width || _bufferImage.Height != Height)
                    _bufferImage = new Bitmap(Width, Height);

                using (var g = Graphics.FromImage(_bufferImage))
                {
                    g.FillRectangle(Brushes.Black, 0, 0, Width, Height);
                    g.DrawLine(Pens.White, 20, Height - 20, Width, Height - 20);
                    g.DrawLine(Pens.White, 20, 0, 20, Height - 20);

                    var textSize = g.MeasureString(string.Format("{0,0:0,000}GHz", _maxFreq), _gridFont);
                    var numTexts = Width / (int)((textSize.Width + 20));
                    if (numTexts <= 0)
                        numTexts = 1;

                    var freqIncrement = (_maxFreq - _minFreq) /(float) numTexts;
                    var pixelIncrement = (Width - 40.0f) / numTexts;
                    for (var n = 0; n <= numTexts; n++)
                    {
                        if(n > 0)
                            g.DrawLine(_gridPen, 20 + n * pixelIncrement, Height - 20, 20 + n * pixelIncrement, 0);

                        g.DrawString(string.Format("{0,0:0,000}GHz", _minFreq + n * freqIncrement), _gridFont, Brushes.White, n * pixelIncrement, Height - 20);
                    }
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.X >= 20 && e.X <= Width - 20 && e.Y < Height - 20)
            {
                _mousePos.X = e.X;
                _mousePos.Y = e.Y;
            }
            else
            {
                _mousePos.X = _mousePos.Y = -1;
            }
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            _mousePos.X = _mousePos.Y = -1;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            OnFreqChange(_freqHover);
        }
    }
}
