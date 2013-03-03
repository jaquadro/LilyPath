using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LilyPath;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo
{
    public class DrawingControl : GraphicsDeviceControl
    {
        private DrawBatch _drawBatch;

        public Color ClearColor { get; set; }

        public Action<DrawBatch> DrawAction { get; set; }

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

            if (DrawAction != null)
                DrawAction(_drawBatch);
        }

        [TestSheet("Primitive Shapes")]
        public static void DrawPrimitiveShapes (DrawBatch drawBatch)
        {
            List<Vector2> wavy = new List<Vector2>();
            for (int i = 0; i < 20; i++) {
                if (i % 2 == 0)
                    wavy.Add(new Vector2(50 + i * 10, 100));
                else
                    wavy.Add(new Vector2(50 + i * 10, 110));
            }

            

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawPrimitiveLine(new Point(50, 50), new Point(250, 50), Pens.Blue);
            drawBatch.DrawPrimitivePath(wavy, Pens.Red);
            drawBatch.DrawPrimitiveRectangle(new Rectangle(50, 160, 200, 100), Pens.Magenta);
            drawBatch.DrawPrimitiveCircle(new Point(350, 100), 50, Pens.Black);
            drawBatch.DrawPrimitiveCircle(new Point(350, 225), 50, 16, Pens.DarkGray);

            drawBatch.End();
        }

        [TestSheet("Outline Shapes")]
        public static void DrawOutlineShapes (DrawBatch drawBatch)
        {
            List<Vector2> wavy = new List<Vector2>();
            for (int i = 0; i < 20; i++) {
                if (i % 2 == 0)
                    wavy.Add(new Vector2(50 + i * 10, 100));
                else
                    wavy.Add(new Vector2(50 + i * 10, 110));
            }

            Pen thickBlue = new Pen(Color.Blue, 15);
            Pen thickRed = new Pen(Color.Red, 15) {
                EndCap = LineCap.Square,
                StartCap = LineCap.Square,
            };
            Pen thickMagenta = new Pen(Color.Magenta, 15);
            Pen thickBlack = new Pen(Color.Black, 15);
            Pen thickDarkGray = new Pen(Color.DarkGray, 15);

            GraphicsPath wavyPath = new GraphicsPath(thickRed, wavy);

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawLine(new Point(50, 50), new Point(250, 50), thickBlue);
            drawBatch.DrawPath(wavyPath);
            drawBatch.DrawRectangle(new Rectangle(50, 160, 200, 100), thickMagenta);
            drawBatch.DrawCircle(new Point(350, 100), 50, thickBlack);
            drawBatch.DrawCircle(new Point(350, 225), 50, 16, thickDarkGray);

            drawBatch.End();
        }

        [TestSheet("Pen Alignment")]
        public static void DrawLineAlignment (DrawBatch drawBatch)
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

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawPath(insetPath);
            drawBatch.DrawPrimitivePath(StarPoints(new Vector2(125, 150), 5, 100, 50, true), new Pen(Color.OrangeRed));
            drawBatch.DrawPath(centerPath);
            drawBatch.DrawPrimitivePath(StarPoints(new Vector2(350, 275), 5, 100, 50, true), new Pen(Color.OrangeRed));
            drawBatch.DrawPath(outsetPath);
            drawBatch.DrawPrimitivePath(StarPoints(new Vector2(125, 400), 5, 100, 50, true), new Pen(Color.OrangeRed));

            drawBatch.End();
        }

        [TestSheet("Filled Shapes")]
        public static void DrawFilledShapes (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.FillRectangle(new Rectangle(50, 50, 200, 100), Brushes.Green);
            drawBatch.FillCircle(new Point(350, 100), 50, Brushes.Blue);
            drawBatch.FillCircle(new Point(500, 100), 50, 16, Brushes.Blue);
            drawBatch.FillPath(StarPoints(new Vector2(150, 300), 8, 100, 50, false), Brushes.Gray);

            drawBatch.End();
        }

        [TestSheet("Primitive Arcs")]
        public static void DrawPrimitiveArcs (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawPrimitiveArc(new Point(100, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(100, 125), 50, 0, (float)Math.PI, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(100, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), Pens.Blue);

            drawBatch.DrawPrimitiveArc(new Point(100, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(100, 325), 50, 0, -(float)Math.PI, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(100, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), Pens.Blue);

            drawBatch.DrawPrimitiveArc(new Point(250, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(250, 125), 50, 0, (float)Math.PI, 16, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(250, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16, Pens.Blue);

            drawBatch.DrawPrimitiveArc(new Point(250, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 16, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(250, 325), 50, 0, -(float)Math.PI, 16, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(250, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 16, Pens.Blue);

            drawBatch.DrawPrimitiveArc(new Point(400, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(400, 125), 50, 0, (float)Math.PI, 4, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(400, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4, Pens.Blue);

            drawBatch.DrawPrimitiveArc(new Point(400, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 4, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(400, 325), 50, 0, -(float)Math.PI, 4, Pens.Blue);
            drawBatch.DrawPrimitiveArc(new Point(400, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 4, Pens.Blue);

            drawBatch.End();
        }

        [TestSheet("Arcs")]
        public static void DrawArcs (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawArc(new Point(100, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), thickPen);
            drawBatch.DrawArc(new Point(100, 125), 50, 0, (float)Math.PI, thickPen);
            drawBatch.DrawArc(new Point(100, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), thickPen);

            drawBatch.DrawArc(new Point(100, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), thickPen);
            drawBatch.DrawArc(new Point(100, 325), 50, 0, -(float)Math.PI, thickPen);
            drawBatch.DrawArc(new Point(100, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), thickPen);

            drawBatch.DrawArc(new Point(250, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16, thickPen);
            drawBatch.DrawArc(new Point(250, 125), 50, 0, (float)Math.PI, 16, thickPen);
            drawBatch.DrawArc(new Point(250, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16, thickPen);

            drawBatch.DrawArc(new Point(250, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 16, thickPen);
            drawBatch.DrawArc(new Point(250, 325), 50, 0, -(float)Math.PI, 16, thickPen);
            drawBatch.DrawArc(new Point(250, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 16, thickPen);

            drawBatch.DrawArc(new Point(400, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4, thickPen);
            drawBatch.DrawArc(new Point(400, 125), 50, 0, (float)Math.PI, 4, thickPen);
            drawBatch.DrawArc(new Point(400, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4, thickPen);

            drawBatch.DrawArc(new Point(400, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 4, thickPen);
            drawBatch.DrawArc(new Point(400, 325), 50, 0, -(float)Math.PI, 4, thickPen);
            drawBatch.DrawArc(new Point(400, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 4, thickPen);

            drawBatch.End();
        }

        [TestSheet("Primitive Closed Arcs")]
        public static void DrawPrimitiveClosedArcs (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawPrimitiveClosedArc(new Point(100, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), Pens.Blue, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(new Point(100, 125), 50, 0, (float)Math.PI, Pens.Blue, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(new Point(100, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), Pens.Blue, ArcType.Segment);

            drawBatch.DrawPrimitiveClosedArc(new Point(250, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16, Pens.Blue, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(new Point(250, 125), 50, 0, (float)Math.PI, 16, Pens.Blue, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(new Point(250, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16, Pens.Blue, ArcType.Segment);

            drawBatch.DrawPrimitiveClosedArc(new Point(400, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4, Pens.Blue, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(new Point(400, 125), 50, 0, (float)Math.PI, 4, Pens.Blue, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(new Point(400, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4, Pens.Blue, ArcType.Segment);

            drawBatch.DrawPrimitiveClosedArc(new Point(100, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), Pens.Blue, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(new Point(100, 410), 50, 0, (float)Math.PI, Pens.Blue, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(new Point(100, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), Pens.Blue, ArcType.Sector);

            drawBatch.DrawPrimitiveClosedArc(new Point(250, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16, Pens.Blue, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(new Point(250, 410), 50, 0, (float)Math.PI, 16, Pens.Blue, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(new Point(250, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16, Pens.Blue, ArcType.Sector);

            drawBatch.DrawPrimitiveClosedArc(new Point(400, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4, Pens.Blue, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(new Point(400, 410), 50, 0, (float)Math.PI, 4, Pens.Blue, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(new Point(400, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4, Pens.Blue, ArcType.Sector);

            drawBatch.End();
        }

        [TestSheet("Closed Arcs")]
        public static void DrawClosedArcs (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawClosedArc(new Point(100, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), thickPen, ArcType.Segment);
            drawBatch.DrawClosedArc(new Point(100, 125), 50, 0, (float)Math.PI, thickPen, ArcType.Segment);
            drawBatch.DrawClosedArc(new Point(100, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), thickPen, ArcType.Segment);

            drawBatch.DrawClosedArc(new Point(250, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16, thickPen, ArcType.Segment);
            drawBatch.DrawClosedArc(new Point(250, 125), 50, 0, (float)Math.PI, 16, thickPen, ArcType.Segment);
            drawBatch.DrawClosedArc(new Point(250, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16, thickPen, ArcType.Segment);

            drawBatch.DrawClosedArc(new Point(400, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4, thickPen, ArcType.Segment);
            drawBatch.DrawClosedArc(new Point(400, 125), 50, 0, (float)Math.PI, 4, thickPen, ArcType.Segment);
            drawBatch.DrawClosedArc(new Point(400, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4, thickPen, ArcType.Segment);

            drawBatch.DrawClosedArc(new Point(100, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), thickPen, ArcType.Sector);
            drawBatch.DrawClosedArc(new Point(100, 410), 50, 0, (float)Math.PI, thickPen, ArcType.Sector);
            drawBatch.DrawClosedArc(new Point(100, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), thickPen, ArcType.Sector);

            drawBatch.DrawClosedArc(new Point(250, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16, thickPen, ArcType.Sector);
            drawBatch.DrawClosedArc(new Point(250, 410), 50, 0, (float)Math.PI, 16, thickPen, ArcType.Sector);
            drawBatch.DrawClosedArc(new Point(250, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16, thickPen, ArcType.Sector);

            drawBatch.DrawClosedArc(new Point(400, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4, thickPen, ArcType.Sector);
            drawBatch.DrawClosedArc(new Point(400, 410), 50, 0, (float)Math.PI, 4, thickPen, ArcType.Sector);
            drawBatch.DrawClosedArc(new Point(400, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4, thickPen, ArcType.Sector);

            drawBatch.End();
        }

        private static RasterizerState GetCommonRasterizerState ()
        {
            return new RasterizerState() {
                FillMode = DemoState.FillMode,
                MultiSampleAntiAlias = DemoState.MultisampleAA,
            };
        }

        private static List<Vector2> StarPoints (Vector2 center, int pointCount, float outerRadius, float innerRadius, bool close)
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
