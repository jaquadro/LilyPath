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

            //DrawPrimitiveShapes();
            DrawLineAlignment();
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

            _drawBatch.Begin();

            _drawBatch.DrawPrimitiveLine(new Point(50, 50), new Point(250, 50), Pens.Blue);
            _drawBatch.DrawPrimitivePath(wavy, Pens.Red);
            _drawBatch.DrawPrimitiveRectangle(new Rectangle(50, 160, 200, 100), Pens.Magenta);
            _drawBatch.DrawPrimitiveCircle(new Point(350, 100), 50, Pens.Black);
            _drawBatch.DrawPrimitiveCircle(new Point(350, 225), 50, 16, Pens.DarkGray);

            _drawBatch.End();
        }

        public void DrawLineAlignment ()
        {
            Pen insetPen = new Pen(Color.MediumTurquoise, 10) {
                Alignment = PenAlignment.Inset
            };
            Pen centerPen = new Pen(Color.MediumTurquoise, 10) {
                Alignment = PenAlignment.Center
            };
            Pen outsetPen = new Pen(Color.MediumTurquoise, 10) {
                Alignment = PenAlignment.Outset
            };

            GraphicsPath insetPath = new GraphicsPath(insetPen, StarPoints(new Vector2(125, 150), 5, 100, 50, false), PathType.Closed);
            GraphicsPath centerPath = new GraphicsPath(centerPen, StarPoints(new Vector2(350, 275), 5, 100, 50, false), PathType.Closed);
            GraphicsPath outsetPath = new GraphicsPath(outsetPen, StarPoints(new Vector2(125, 400), 5, 100, 50, false), PathType.Closed);

            _drawBatch.Begin();

            
            _drawBatch.DrawPath(insetPath);
            _drawBatch.DrawPrimitivePath(StarPoints(new Vector2(125, 150), 5, 100, 50, true), new Pen(Color.OrangeRed));
            _drawBatch.DrawPath(centerPath);
            _drawBatch.DrawPrimitivePath(StarPoints(new Vector2(350, 275), 5, 100, 50, true), new Pen(Color.OrangeRed));
            _drawBatch.DrawPath(outsetPath);
            _drawBatch.DrawPrimitivePath(StarPoints(new Vector2(125, 400), 5, 100, 50, true), new Pen(Color.OrangeRed));

            _drawBatch.End();
        }

        private List<Vector2> StarPoints (Vector2 center, int pointCount, float outerRadius, float innerRadius, bool close)
        {
            List<Vector2> points = new List<Vector2>();

            int limit = (close) ? pointCount * 2 + 1 : pointCount * 2;

            float rot = (float)((Math.PI * 2) / (pointCount * 2));
            for (int i = 0; i < limit; i++) {
                float si = (float)Math.Sin(-i * rot + Math.PI);
                float ci = (float)Math.Cos(-i * rot + Math.PI);

                if (i % 2 == 0)
                    points.Add(center + new Vector2(si, ci) * outerRadius);
                else
                    points.Add(center + new Vector2(si, ci) * innerRadius);
            }

            return points;
        }
    }
}
