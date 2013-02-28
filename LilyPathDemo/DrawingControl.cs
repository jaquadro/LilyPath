using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LilyPath;
using System.Windows.Forms;

namespace LilyPathDemo
{
    public class DrawingControl : GraphicsDeviceControl
    {
        private DrawBatch _drawBatch;

        public Color ClearColor { get; set; }

        protected override void Initialize ()
        {
            ClearColor = Color.GhostWhite;

            Brushes.Initialize(GraphicsDevice);
            Pens.Initialize(GraphicsDevice);

            _drawBatch = new DrawBatch(GraphicsDevice);

            Application.Idle += delegate { Invalidate(); };
        }

        protected override void Draw ()
        {
            GraphicsDevice.Clear(ClearColor);

            _drawBatch.Begin();

            DrawPrimitiveShapes();

            _drawBatch.End();
        }

        public void DrawPrimitiveShapes ()
        {
            List<Vector2> wavy = new List<Vector2>();
            for (int i = 0; i < 20; i++) {
                if (i % 2 == 0)
                    wavy.Add(new Vector2(50 + i * 10, 100));
                else
                    wavy.Add(new Vector2(50 + i * 10, 110));
            }

            _drawBatch.DrawPrimitiveLine(new Point(50, 50), new Point(250, 50), Pens.Blue);
            _drawBatch.DrawPrimitivePath(wavy, Pens.Red);
            _drawBatch.DrawPrimitiveRectangle(new Rectangle(50, 160, 200, 100), Pens.Magenta);
            _drawBatch.DrawPrimitiveCircle(new Point(350, 100), 50, Pens.Black);
            _drawBatch.DrawPrimitiveCircle(new Point(350, 225), 50, 16, Pens.DarkGray);
        }
    }
}
