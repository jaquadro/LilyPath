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

        private TimeSpan _sampleSpan = TimeSpan.FromSeconds(1);
        private Stopwatch _stopWatch;
        private int _sampleFrames;

        public Color ClearColor { get; set; }

        public Action<DrawBatch> DrawAction { get; set; }

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
        }

        protected override void Draw ()
        {
            if (_stopWatch.Elapsed > _sampleSpan) {
                Fps = (float)_sampleFrames / (float)_stopWatch.Elapsed.TotalSeconds;

                _stopWatch.Reset();
                _stopWatch.Start();
                _sampleFrames = 0;

                OnFpsUpdated();
            }

            GraphicsDevice.Clear(ClearColor);

            if (DrawAction != null)
                DrawAction(_drawBatch);

            _sampleFrames++;
        }
    }
}
