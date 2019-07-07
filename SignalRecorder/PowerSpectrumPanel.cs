using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace SDRSharp.SignalRecorder
{
    public partial class PowerSpectrumPanel : UserControl
    {
        public static PowerSpectrumPanel PowerSpectrum { get; set; }

        private Bitmap _bufferImage;
        private Bitmap _drawingImage;
        private Queue<float> _buffer;
        private Timer _timer;
        private ulong _timeCounter;
        private Pen _gridPen;
        private Font _gridFont;
        private int _timerPeriod = 100;
        private SignalRecorderPanel _controlPanel;

        public PowerSpectrumPanel(SignalRecorderPanel controlPanel)
        {
            InitializeComponent();
            _controlPanel = controlPanel;
            _buffer = new Queue<float>();
            _timer = new Timer(state => Invalidate(), null, 100, _timerPeriod);
            _gridPen = new Pen(Color.Gray, 1.0f) {DashStyle = DashStyle.Dash};
            _gridFont = new Font("Arial", 8f);

            PowerSpectrum = this;
        }

        public void Draw(float dataPoint)
        {
            lock(_buffer)
                _buffer.Enqueue(dataPoint);
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
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private int _lastY = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_bufferImage == null)
                return;

            if (_drawingImage == null || _drawingImage.Size != _bufferImage.Size)
                _drawingImage = new Bitmap(_bufferImage.Width, _bufferImage.Height);

            var scale = (_bufferImage.Height - 20.0f) / (_controlPanel.Range - _controlPanel.Offset); // dB per pixel
            var count = _buffer.Count;
            if (count != 0)
            {
                using (var g = Graphics.FromImage(_bufferImage))
                {
                    g.DrawImageUnscaled(_bufferImage, - count, 0);

                    g.FillRectangle(Brushes.Black, _bufferImage.Width - count, 0, count, _bufferImage.Height);
                    g.FillRectangle(Brushes.Black, 0, 0, 50, _bufferImage.Height);

                    float p;
                    for (var n = count; n > 0; n--)
                    {
                        if (((_timeCounter++) % PointsPerSecond) == 0)
                        {
                            g.DrawLine(_timeCounter == 1 ? Pens.DodgerBlue : _gridPen, _bufferImage.Width - n, 20, _bufferImage.Width - n, _bufferImage.Height);
                            g.DrawString(((_timeCounter - 1)/PointsPerSecond).ToString(CultureInfo.InvariantCulture), _gridFont, Brushes.Silver, _bufferImage.Width - n - DefaultFont.Height, 0,
                                new StringFormat(StringFormatFlags.DirectionVertical));
                        }

                        lock(_buffer)
                            p = _buffer.Dequeue();

                        var y = (int) ((-p - _controlPanel.Offset) * scale + 10.5);

                        if (y < 10)
                            y = 10;

                        if (y >= Height)
                            y = Height - 1;

                        g.DrawLine(Pens.Yellow, _bufferImage.Width - n - 1, _lastY, _bufferImage.Width - n, y);

                        _lastY = y;
                    }
                }
            }

            using (var g = Graphics.FromImage(_drawingImage))
            {
                g.DrawImageUnscaled(_bufferImage, 0, 0);
                g.DrawLine(Pens.White, 50, 0, 50, _bufferImage.Height);

                var grid = (_bufferImage.Height - 20.0f)/15;
                for (var j = 0; j <= 15; j++)
                {
                    var y = (int) (_bufferImage.Height - 10 - j*grid + 0.5);
                    g.DrawLine(_gridPen, 51, y, _bufferImage.Width, y);
                    g.DrawString(string.Format("{0}", -Math.Round(_controlPanel.Range - j*(_controlPanel.Range - _controlPanel.Offset)/15.0f, 1)), _gridFont, Brushes.Silver, 10, y - _gridFont.Height/2);
                }

                var ySquelch = (int) ((-_controlPanel.SquelchValue - _controlPanel.Offset)*scale + 10.5);
                if (ySquelch >= 0 && ySquelch < _bufferImage.Height)
                    g.DrawLine(Pens.Red, 51, ySquelch, _bufferImage.Width, ySquelch);
            }
            e.Graphics.DrawImageUnscaled(_drawingImage, 0, 0);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _bufferImage = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(_bufferImage))
                g.FillRectangle(Brushes.Black, 0, 0, Width, Height);
        }

        public ulong PointsPerSecond { get; set; }

        public void Restart()
        {
            _timeCounter = 0;
            if (_bufferImage == null)
                return;

            using (var g = Graphics.FromImage(_bufferImage))
                g.FillRectangle(Brushes.Black, 0, 0, Width, Height);
        }

        public void ResetTime()
        {
            _timeCounter = 0;
        }
    }
}
