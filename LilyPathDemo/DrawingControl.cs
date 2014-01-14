using System;
using System.Diagnostics;
using System.Windows.Forms;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo
{
    public class DrawingControl : GraphicsDeviceControl
    {
        private DrawBatch _drawBatch;

        private Stopwatch _timer;
        private TimeSpan _elapsed;

        private TimeSpan _sampleSpan = TimeSpan.FromSeconds(1);
        private Stopwatch _stopWatch;
        private int _sampleFrames;

        private TestSheet _testSheet;

        public Color ClearColor { get; set; }

        public TestSheet Sheet
        {
            get { return _testSheet; }
            set
            {
                if (_testSheet != value) {
                    if (_testSheet != null)
                        _testSheet.TearDown();
                    _testSheet = value;
                }
            }
        }

        public float Fps { get; set; }

        public event EventHandler FpsUpdated;

        protected virtual void OnFpsUpdated ()
        {
            var ev = FpsUpdated;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }

        protected override void Initialize ()
        {
            ClearColor = Color.GhostWhite;

            _drawBatch = new DrawBatch(GraphicsDevice);

            Application.Idle += delegate { Invalidate(); };

            _stopWatch = Stopwatch.StartNew();
            _timer = Stopwatch.StartNew();
        }

        protected override void Draw ()
        {
            GameTime gameTime = new GameTime(_timer.Elapsed, _timer.Elapsed - _elapsed);
            _elapsed = _timer.Elapsed;

            if (_stopWatch.Elapsed > _sampleSpan) {
                Fps = (float)_sampleFrames / (float)_stopWatch.Elapsed.TotalSeconds;

                _stopWatch.Reset();
                _stopWatch.Start();
                _sampleFrames = 0;

                OnFpsUpdated();
            }

            if (Sheet != null)
                GraphicsDevice.Clear(Sheet.ClearColor);
            else
                GraphicsDevice.Clear(ClearColor);

            if (Sheet != null)
                Sheet.Apply(gameTime, _drawBatch);

            _sampleFrames++;
        }
    }
}
