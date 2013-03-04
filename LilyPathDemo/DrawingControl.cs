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

            drawBatch.DrawPrimitiveLine(Pens.Blue, new Point(50, 50), new Point(250, 50));
            drawBatch.DrawPrimitivePath(Pens.Red, wavy);
            drawBatch.DrawPrimitiveRectangle(Pens.Magenta, new Rectangle(50, 160, 200, 100));
            drawBatch.DrawPrimitiveCircle(Pens.Black, new Point(350, 100), 50);
            drawBatch.DrawPrimitiveCircle(Pens.DarkGray, new Point(350, 225), 50, 16);

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

            drawBatch.DrawLine(thickBlue, new Point(50, 50), new Point(250, 50));
            drawBatch.DrawPath(wavyPath);
            drawBatch.DrawRectangle(thickMagenta, new Rectangle(50, 160, 200, 100));
            drawBatch.DrawCircle(thickBlack, new Point(350, 100), 50);
            drawBatch.DrawCircle(thickDarkGray, new Point(350, 225), 50, 16);

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
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), StarPoints(new Vector2(125, 150), 5, 100, 50, true));
            drawBatch.DrawPath(centerPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), StarPoints(new Vector2(350, 275), 5, 100, 50, true));
            drawBatch.DrawPath(outsetPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), StarPoints(new Vector2(125, 400), 5, 100, 50, true));

            drawBatch.End();
        }

        [TestSheet("Filled Shapes")]
        public static void DrawFilledShapes (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.FillRectangle(Brushes.Green, new Rectangle(50, 50, 200, 100));
            drawBatch.FillCircle(Brushes.Blue, new Point(350, 100), 50);
            drawBatch.FillCircle(Brushes.Blue, new Point(500, 100), 50, 16);
            drawBatch.FillPath(Brushes.Gray, StarPoints(new Vector2(150, 300), 8, 100, 50, false));

            drawBatch.End();
        }

        [TestSheet("Primitive Arcs")]
        public static void DrawPrimitiveArcs (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(100, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5));
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(100, 125), 50, 0, (float)Math.PI);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(100, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5));

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(100, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5));
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(100, 325), 50, 0, -(float)Math.PI);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(100, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5));

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(250, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(250, 125), 50, 0, (float)Math.PI, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(250, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(250, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(250, 325), 50, 0, -(float)Math.PI, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(250, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 16);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(400, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(400, 125), 50, 0, (float)Math.PI, 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(400, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(400, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(400, 325), 50, 0, -(float)Math.PI, 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Point(400, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 4);

            drawBatch.End();
        }

        [TestSheet("Primitive Arcs 2")]
        public static void DrawPrimitiveArcs2 (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(50, 75), new Vector2(150, 75), 25);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(50, 125), new Vector2(150, 125), 50);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(50, 200), new Vector2(150, 200), 75);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(50, 225), new Vector2(150, 225), -75);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(50, 300), new Vector2(150, 300), -50);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(50, 350), new Vector2(150, 350), -25);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(175, 75), new Vector2(275, 75), 25, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(175, 125), new Vector2(275, 125), 50, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(175, 200), new Vector2(275, 200), 75, 16);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(175, 225), new Vector2(275, 225), -75, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(175, 300), new Vector2(275, 300), -50, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(175, 350), new Vector2(275, 350), -25, 16);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(325, 50), new Vector2(325, 150), -25);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(375, 50), new Vector2(375, 150), -50);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(450, 50), new Vector2(450, 150), -75);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(475, 50), new Vector2(475, 150), 75);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(550, 50), new Vector2(550, 150), 50);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(600, 50), new Vector2(600, 150), 25);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(325, 175), new Vector2(325, 275), -25, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(375, 175), new Vector2(375, 275), -50, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(450, 175), new Vector2(450, 275), -75, 16);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(475, 175), new Vector2(475, 275), 75, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(550, 175), new Vector2(550, 275), 50, 16);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(600, 175), new Vector2(600, 275), 25, 16);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(325, 300), new Vector2(325, 400), -25, 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(375, 300), new Vector2(375, 400), -50, 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(450, 300), new Vector2(450, 400), -75, 4);

            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(475, 300), new Vector2(475, 400), 75, 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(550, 300), new Vector2(550, 400), 50, 4);
            drawBatch.DrawPrimitiveArc(Pens.Blue, new Vector2(600, 300), new Vector2(600, 400), 25, 4);

            drawBatch.End();
        }

        [TestSheet("Arcs")]
        public static void DrawArcs (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawArc(thickPen, new Point(100, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5));
            drawBatch.DrawArc(thickPen, new Point(100, 125), 50, 0, (float)Math.PI);
            drawBatch.DrawArc(thickPen, new Point(100, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5));

            drawBatch.DrawArc(thickPen, new Point(100, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5));
            drawBatch.DrawArc(thickPen, new Point(100, 325), 50, 0, -(float)Math.PI);
            drawBatch.DrawArc(thickPen, new Point(100, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5));

            drawBatch.DrawArc(thickPen, new Point(250, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16);
            drawBatch.DrawArc(thickPen, new Point(250, 125), 50, 0, (float)Math.PI, 16);
            drawBatch.DrawArc(thickPen, new Point(250, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 16);

            drawBatch.DrawArc(thickPen, new Point(250, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 16);
            drawBatch.DrawArc(thickPen, new Point(250, 325), 50, 0, -(float)Math.PI, 16);
            drawBatch.DrawArc(thickPen, new Point(250, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 16);

            drawBatch.DrawArc(thickPen, new Point(400, 125), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4);
            drawBatch.DrawArc(thickPen, new Point(400, 125), 50, 0, (float)Math.PI, 4);
            drawBatch.DrawArc(thickPen, new Point(400, 175), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), 4);

            drawBatch.DrawArc(thickPen, new Point(400, 275), 50, (float)(Math.PI * 0.25), (float)(Math.PI * -1.5), 4);
            drawBatch.DrawArc(thickPen, new Point(400, 325), 50, 0, -(float)Math.PI, 4);
            drawBatch.DrawArc(thickPen, new Point(400, 325), 75, (float)(Math.PI * -0.25), (float)(Math.PI * -0.5), 4);

            drawBatch.End();
        }

        [TestSheet("Arcs 2")]
        public static void DrawArcs2 (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawArc(thickPen, new Vector2(50, 75), new Vector2(150, 75), 25);
            drawBatch.DrawArc(thickPen, new Vector2(50, 125), new Vector2(150, 125), 50);
            drawBatch.DrawArc(thickPen, new Vector2(50, 200), new Vector2(150, 200), 75);

            drawBatch.DrawArc(thickPen, new Vector2(50, 225), new Vector2(150, 225), -75);
            drawBatch.DrawArc(thickPen, new Vector2(50, 300), new Vector2(150, 300), -50);
            drawBatch.DrawArc(thickPen, new Vector2(50, 350), new Vector2(150, 350), -25);

            drawBatch.DrawArc(thickPen, new Vector2(175, 75), new Vector2(275, 75), 25, 16);
            drawBatch.DrawArc(thickPen, new Vector2(175, 125), new Vector2(275, 125), 50, 16);
            drawBatch.DrawArc(thickPen, new Vector2(175, 200), new Vector2(275, 200), 75, 16);

            drawBatch.DrawArc(thickPen, new Vector2(175, 225), new Vector2(275, 225), -75, 16);
            drawBatch.DrawArc(thickPen, new Vector2(175, 300), new Vector2(275, 300), -50, 16);
            drawBatch.DrawArc(thickPen, new Vector2(175, 350), new Vector2(275, 350), -25, 16);

            drawBatch.DrawArc(thickPen, new Vector2(325, 50), new Vector2(325, 150), -25);
            drawBatch.DrawArc(thickPen, new Vector2(375, 50), new Vector2(375, 150), -50);
            drawBatch.DrawArc(thickPen, new Vector2(450, 50), new Vector2(450, 150), -75);

            drawBatch.DrawArc(thickPen, new Vector2(475, 50), new Vector2(475, 150), 75);
            drawBatch.DrawArc(thickPen, new Vector2(550, 50), new Vector2(550, 150), 50);
            drawBatch.DrawArc(thickPen, new Vector2(600, 50), new Vector2(600, 150), 25);

            drawBatch.DrawArc(thickPen, new Vector2(325, 175), new Vector2(325, 275), -25, 16);
            drawBatch.DrawArc(thickPen, new Vector2(375, 175), new Vector2(375, 275), -50, 16);
            drawBatch.DrawArc(thickPen, new Vector2(450, 175), new Vector2(450, 275), -75, 16);

            drawBatch.DrawArc(thickPen, new Vector2(475, 175), new Vector2(475, 275), 75, 16);
            drawBatch.DrawArc(thickPen, new Vector2(550, 175), new Vector2(550, 275), 50, 16);
            drawBatch.DrawArc(thickPen, new Vector2(600, 175), new Vector2(600, 275), 25, 16);

            drawBatch.DrawArc(thickPen, new Vector2(325, 300), new Vector2(325, 400), -25, 4);
            drawBatch.DrawArc(thickPen, new Vector2(375, 300), new Vector2(375, 400), -50, 4);
            drawBatch.DrawArc(thickPen, new Vector2(450, 300), new Vector2(450, 400), -75, 4);

            drawBatch.DrawArc(thickPen, new Vector2(475, 300), new Vector2(475, 400), 75, 4);
            drawBatch.DrawArc(thickPen, new Vector2(550, 300), new Vector2(550, 400), 50, 4);
            drawBatch.DrawArc(thickPen, new Vector2(600, 300), new Vector2(600, 400), 25, 4);

            drawBatch.End();
        }

        [TestSheet("Primitive Closed Arcs")]
        public static void DrawPrimitiveClosedArcs (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(100, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(100, 125), 50, 0, (float)Math.PI, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(100, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment);

            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(250, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment, 16);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(250, 125), 50, 0, (float)Math.PI, ArcType.Segment, 16);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(250, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment, 16);

            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(400, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment, 4);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(400, 125), 50, 0, (float)Math.PI, ArcType.Segment, 4);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(400, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment, 4);

            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(100, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(100, 410), 50, 0, (float)Math.PI, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(100, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector);

            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(250, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector, 16);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(250, 410), 50, 0, (float)Math.PI, ArcType.Sector, 16);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(250, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector, 16);

            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(400, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector, 4);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(400, 410), 50, 0, (float)Math.PI, ArcType.Sector, 4);
            drawBatch.DrawPrimitiveClosedArc(Pens.Blue, new Point(400, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector, 4);

            drawBatch.End();
        }

        [TestSheet("Closed Arcs")]
        public static void DrawClosedArcs (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.DrawClosedArc(thickPen, new Point(100, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment);
            drawBatch.DrawClosedArc(thickPen, new Point(100, 125), 50, 0, (float)Math.PI, ArcType.Segment);
            drawBatch.DrawClosedArc(thickPen, new Point(100, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment);

            drawBatch.DrawClosedArc(thickPen, new Point(250, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment, 16);
            drawBatch.DrawClosedArc(thickPen, new Point(250, 125), 50, 0, (float)Math.PI, ArcType.Segment, 16);
            drawBatch.DrawClosedArc(thickPen, new Point(250, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment, 16);

            drawBatch.DrawClosedArc(thickPen, new Point(400, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment, 4);
            drawBatch.DrawClosedArc(thickPen, new Point(400, 125), 50, 0, (float)Math.PI, ArcType.Segment, 4);
            drawBatch.DrawClosedArc(thickPen, new Point(400, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment, 4);

            drawBatch.DrawClosedArc(thickPen, new Point(100, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector);
            drawBatch.DrawClosedArc(thickPen, new Point(100, 410), 50, 0, (float)Math.PI, ArcType.Sector);
            drawBatch.DrawClosedArc(thickPen, new Point(100, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector);

            drawBatch.DrawClosedArc(thickPen, new Point(250, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector, 16);
            drawBatch.DrawClosedArc(thickPen, new Point(250, 410), 50, 0, (float)Math.PI, ArcType.Sector, 16);
            drawBatch.DrawClosedArc(thickPen, new Point(250, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector, 16);

            drawBatch.DrawClosedArc(thickPen, new Point(400, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector, 4);
            drawBatch.DrawClosedArc(thickPen, new Point(400, 410), 50, 0, (float)Math.PI, ArcType.Sector, 4);
            drawBatch.DrawClosedArc(thickPen, new Point(400, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector, 4);

            drawBatch.End();
        }

        [TestSheet("Filled Arcs")]
        public static void DrawFilledArcs (DrawBatch drawBatch)
        {
            drawBatch.Begin(null, null, null, GetCommonRasterizerState(), Matrix.Identity);

            drawBatch.FillArc(Brushes.Blue, new Point(100, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment);
            drawBatch.FillArc(Brushes.Blue, new Point(100, 125), 50, 0, (float)Math.PI, ArcType.Segment);
            drawBatch.FillArc(Brushes.Blue, new Point(100, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment);

            drawBatch.FillArc(Brushes.Blue, new Point(250, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment, 16);
            drawBatch.FillArc(Brushes.Blue, new Point(250, 125), 50, 0, (float)Math.PI, ArcType.Segment, 16);
            drawBatch.FillArc(Brushes.Blue, new Point(250, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment, 16);

            drawBatch.FillArc(Brushes.Blue, new Point(400, 100), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Segment, 4);
            drawBatch.FillArc(Brushes.Blue, new Point(400, 125), 50, 0, (float)Math.PI, ArcType.Segment, 4);
            drawBatch.FillArc(Brushes.Blue, new Point(400, 200), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Segment, 4);

            drawBatch.FillArc(Brushes.Blue, new Point(100, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector);
            drawBatch.FillArc(Brushes.Blue, new Point(100, 410), 50, 0, (float)Math.PI, ArcType.Sector);
            drawBatch.FillArc(Brushes.Blue, new Point(100, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector);

            drawBatch.FillArc(Brushes.Blue, new Point(250, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector, 16);
            drawBatch.FillArc(Brushes.Blue, new Point(250, 410), 50, 0, (float)Math.PI, ArcType.Sector, 16);
            drawBatch.FillArc(Brushes.Blue, new Point(250, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector, 16);

            drawBatch.FillArc(Brushes.Blue, new Point(400, 335), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), ArcType.Sector, 4);
            drawBatch.FillArc(Brushes.Blue, new Point(400, 410), 50, 0, (float)Math.PI, ArcType.Sector, 4);
            drawBatch.FillArc(Brushes.Blue, new Point(400, 480), 50, (float)(Math.PI * -0.25), (float)(Math.PI * 1.5), ArcType.Sector, 4);

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
