using System;
using System.Collections.Generic;
using LilyPath;
using LilyPath.Pens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo
{
    class TestSheets
    {
        private static void SetupDrawBatch (DrawBatch drawBatch)
        {
            drawBatch.Begin(DrawSortMode.Deferred, null, null, null, GetCommonRasterizerState(), null, Matrix.Identity);
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

            SetupDrawBatch(drawBatch);

            drawBatch.DrawPrimitiveLine(Pen.Blue, new Vector2(50, 50), new Vector2(250, 50));
            drawBatch.DrawPrimitivePath(Pen.Red, wavy);
            drawBatch.DrawPrimitiveRectangle(Pen.Magenta, new Rectangle(50, 160, 200, 100));
            drawBatch.DrawPrimitiveCircle(Pen.Black, new Vector2(350, 100), 50);
            drawBatch.DrawPrimitiveCircle(Pen.DarkGray, new Vector2(350, 225), 50, 16);
            drawBatch.DrawPrimitiveRectangle(Pen.Green, new Rectangle(50, 350, 200, 100), (float)Math.PI / 4f);

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
            Pen thickGreen = new Pen(Color.Green, 15);

            GraphicsPath wavyPath = new GraphicsPath(thickRed, wavy);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawLine(thickBlue, new Vector2(50, 50), new Vector2(250, 50));
            drawBatch.DrawPath(wavyPath);
            drawBatch.DrawRectangle(thickMagenta, new Rectangle(50, 160, 200, 100));
            drawBatch.DrawCircle(thickBlack, new Vector2(350, 100), 50);
            drawBatch.DrawCircle(thickDarkGray, new Vector2(350, 225), 50, 16);
            drawBatch.DrawRectangle(thickGreen, new Rectangle(50, 350, 200, 100), (float)Math.PI / 4f);

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

            GraphicsPath insetPath = new GraphicsPath(insetPen, StarPoints(new Vector2(125, 150), 5, 100, 50, 0, false), PathType.Closed);
            GraphicsPath centerPath = new GraphicsPath(centerPen, StarPoints(new Vector2(350, 275), 5, 100, 50, 0, false), PathType.Closed);
            GraphicsPath outsetPath = new GraphicsPath(outsetPen, StarPoints(new Vector2(125, 400), 5, 100, 50, 0, false), PathType.Closed);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawPath(insetPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), StarPoints(new Vector2(125, 150), 5, 100, 50, 0, true));
            drawBatch.DrawPath(centerPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), StarPoints(new Vector2(350, 275), 5, 100, 50, 0, true));
            drawBatch.DrawPath(outsetPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), StarPoints(new Vector2(125, 400), 5, 100, 50, 0, true));

            drawBatch.End();
        }

        [TestSheet("Filled Shapes")]
        public static void DrawFilledShapes (DrawBatch drawBatch)
        {
            SetupDrawBatch(drawBatch);

            drawBatch.FillRectangle(Brush.Green, new Rectangle(50, 50, 200, 100));
            drawBatch.FillCircle(Brush.Blue, new Vector2(350, 100), 50);
            drawBatch.FillCircle(Brush.Blue, new Vector2(500, 100), 50, 16);
            drawBatch.FillPath(Brush.Gray, StarPoints(new Vector2(150, 300), 8, 100, 50, 0, false));
            drawBatch.FillRectangle(Brush.Green, new Rectangle(300, 250, 200, 100), (float)Math.PI / 4f);

            drawBatch.End();
        }

        [TestSheet("Primitive Arcs")]
        public static void DrawPrimitiveArcs (DrawBatch drawBatch)
        {
            SetupDrawBatch(drawBatch);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(100, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5));
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(100, 125), 50, 0, -(float)Math.PI);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(100, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5));

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(100, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5));
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(100, 325), 50, 0, (float)Math.PI);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(100, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5));

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(250, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(250, 125), 50, 0, -(float)Math.PI, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(250, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), 16);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(250, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5), 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(250, 325), 50, 0, (float)Math.PI, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(250, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(400, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(400, 125), 50, 0, -(float)Math.PI, 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(400, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), 4);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(400, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5), 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(400, 325), 50, 0, (float)Math.PI, 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(400, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4);

            drawBatch.End();
        }

        [TestSheet("Primitive Arcs 2")]
        public static void DrawPrimitiveArcs2 (DrawBatch drawBatch)
        {
            SetupDrawBatch(drawBatch);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(50, 75), new Vector2(150, 75), 25);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(50, 125), new Vector2(150, 125), 50);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(50, 200), new Vector2(150, 200), 75);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(50, 225), new Vector2(150, 225), -75);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(50, 300), new Vector2(150, 300), -50);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(50, 350), new Vector2(150, 350), -25);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(275, 75), new Vector2(175, 75), -25, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(275, 125), new Vector2(175, 125), -50, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(275, 200), new Vector2(175, 200), -75, 16);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(275, 225), new Vector2(175, 225), 75, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(275, 300), new Vector2(175, 300), 50, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(275, 350), new Vector2(175, 350), 25, 16);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(325, 50), new Vector2(325, 150), -25);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(375, 50), new Vector2(375, 150), -50);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(450, 50), new Vector2(450, 150), -75);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(475, 50), new Vector2(475, 150), 75);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(550, 50), new Vector2(550, 150), 50);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(600, 50), new Vector2(600, 150), 25);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(325, 275), new Vector2(325, 175), 25, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(375, 275), new Vector2(375, 175), 50, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(450, 275), new Vector2(450, 175), 75, 16);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(475, 275), new Vector2(475, 175), -75, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(550, 275), new Vector2(550, 175), -50, 16);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(600, 275), new Vector2(600, 175), -25, 16);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(325, 300), new Vector2(325, 400), -25, 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(375, 300), new Vector2(375, 400), -50, 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(450, 300), new Vector2(450, 400), -75, 4);

            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(475, 300), new Vector2(475, 400), 75, 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(550, 300), new Vector2(550, 400), 50, 4);
            drawBatch.DrawPrimitiveArc(Pen.Blue, new Vector2(600, 300), new Vector2(600, 400), 25, 4);

            drawBatch.End();
        }

        [TestSheet("Arcs")]
        public static void DrawArcs (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawArc(thickPen, new Vector2(100, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5));
            drawBatch.DrawArc(thickPen, new Vector2(100, 125), 50, 0, -(float)Math.PI);
            drawBatch.DrawArc(thickPen, new Vector2(100, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5));

            drawBatch.DrawArc(thickPen, new Vector2(100, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5));
            drawBatch.DrawArc(thickPen, new Vector2(100, 325), 50, 0, (float)Math.PI);
            drawBatch.DrawArc(thickPen, new Vector2(100, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5));

            drawBatch.DrawArc(thickPen, new Vector2(250, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), 16);
            drawBatch.DrawArc(thickPen, new Vector2(250, 125), 50, 0, -(float)Math.PI, 16);
            drawBatch.DrawArc(thickPen, new Vector2(250, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), 16);

            drawBatch.DrawArc(thickPen, new Vector2(250, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5), 16);
            drawBatch.DrawArc(thickPen, new Vector2(250, 325), 50, 0, (float)Math.PI, 16);
            drawBatch.DrawArc(thickPen, new Vector2(250, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16);

            drawBatch.DrawArc(thickPen, new Vector2(400, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), 4);
            drawBatch.DrawArc(thickPen, new Vector2(400, 125), 50, 0, -(float)Math.PI, 4);
            drawBatch.DrawArc(thickPen, new Vector2(400, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), 4);

            drawBatch.DrawArc(thickPen, new Vector2(400, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5), 4);
            drawBatch.DrawArc(thickPen, new Vector2(400, 325), 50, 0, (float)Math.PI, 4);
            drawBatch.DrawArc(thickPen, new Vector2(400, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4);

            drawBatch.End();
        }

        [TestSheet("Arcs 2")]
        public static void DrawArcs2 (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            SetupDrawBatch(drawBatch);

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
            SetupDrawBatch(drawBatch);

            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(100, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(100, 125), 50, 0, -(float)Math.PI, ArcType.Segment);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(100, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment);

            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(250, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment, 16);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(250, 125), 50, 0, -(float)Math.PI, ArcType.Segment, 16);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(250, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment, 16);

            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(400, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment, 4);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(400, 125), 50, 0, -(float)Math.PI, ArcType.Segment, 4);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(400, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment, 4);

            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(100, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(100, 410), 50, 0, -(float)Math.PI, ArcType.Sector);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(100, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector);

            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(250, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector, 16);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(250, 410), 50, 0, -(float)Math.PI, ArcType.Sector, 16);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(250, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector, 16);

            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(400, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector, 4);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(400, 410), 50, 0, -(float)Math.PI, ArcType.Sector, 4);
            drawBatch.DrawPrimitiveClosedArc(Pen.Blue, new Vector2(400, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector, 4);

            drawBatch.End();
        }

        [TestSheet("Closed Arcs")]
        public static void DrawClosedArcs (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Blue, 15);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawClosedArc(thickPen, new Vector2(100, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment);
            drawBatch.DrawClosedArc(thickPen, new Vector2(100, 125), 50, 0, -(float)Math.PI, ArcType.Segment);
            drawBatch.DrawClosedArc(thickPen, new Vector2(100, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment);

            drawBatch.DrawClosedArc(thickPen, new Vector2(250, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment, 16);
            drawBatch.DrawClosedArc(thickPen, new Vector2(250, 125), 50, 0, -(float)Math.PI, ArcType.Segment, 16);
            drawBatch.DrawClosedArc(thickPen, new Vector2(250, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment, 16);

            drawBatch.DrawClosedArc(thickPen, new Vector2(400, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment, 4);
            drawBatch.DrawClosedArc(thickPen, new Vector2(400, 125), 50, 0, -(float)Math.PI, ArcType.Segment, 4);
            drawBatch.DrawClosedArc(thickPen, new Vector2(400, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment, 4);

            drawBatch.DrawClosedArc(thickPen, new Vector2(100, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector);
            drawBatch.DrawClosedArc(thickPen, new Vector2(100, 410), 50, 0, -(float)Math.PI, ArcType.Sector);
            drawBatch.DrawClosedArc(thickPen, new Vector2(100, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector);

            drawBatch.DrawClosedArc(thickPen, new Vector2(250, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector, 16);
            drawBatch.DrawClosedArc(thickPen, new Vector2(250, 410), 50, 0, -(float)Math.PI, ArcType.Sector, 16);
            drawBatch.DrawClosedArc(thickPen, new Vector2(250, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector, 16);

            drawBatch.DrawClosedArc(thickPen, new Vector2(400, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector, 4);
            drawBatch.DrawClosedArc(thickPen, new Vector2(400, 410), 50, 0, -(float)Math.PI, ArcType.Sector, 4);
            drawBatch.DrawClosedArc(thickPen, new Vector2(400, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector, 4);

            drawBatch.End();
        }

        [TestSheet("Filled Arcs")]
        public static void DrawFilledArcs (DrawBatch drawBatch)
        {
            SetupDrawBatch(drawBatch);

            drawBatch.FillArc(Brush.Blue, new Vector2(100, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment);
            drawBatch.FillArc(Brush.Blue, new Vector2(100, 125), 50, 0, -(float)Math.PI, ArcType.Segment);
            drawBatch.FillArc(Brush.Blue, new Vector2(100, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment);

            drawBatch.FillArc(Brush.Blue, new Vector2(250, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment, 16);
            drawBatch.FillArc(Brush.Blue, new Vector2(250, 125), 50, 0, -(float)Math.PI, ArcType.Segment, 16);
            drawBatch.FillArc(Brush.Blue, new Vector2(250, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment, 16);

            drawBatch.FillArc(Brush.Blue, new Vector2(400, 100), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Segment, 4);
            drawBatch.FillArc(Brush.Blue, new Vector2(400, 125), 50, 0, -(float)Math.PI, ArcType.Segment, 4);
            drawBatch.FillArc(Brush.Blue, new Vector2(400, 200), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Segment, 4);

            drawBatch.FillArc(Brush.Blue, new Vector2(100, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector);
            drawBatch.FillArc(Brush.Blue, new Vector2(100, 410), 50, 0, -(float)Math.PI, ArcType.Sector);
            drawBatch.FillArc(Brush.Blue, new Vector2(100, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector);

            drawBatch.FillArc(Brush.Blue, new Vector2(250, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector, 16);
            drawBatch.FillArc(Brush.Blue, new Vector2(250, 410), 50, 0, -(float)Math.PI, ArcType.Sector, 16);
            drawBatch.FillArc(Brush.Blue, new Vector2(250, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector, 16);

            drawBatch.FillArc(Brush.Blue, new Vector2(400, 335), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), ArcType.Sector, 4);
            drawBatch.FillArc(Brush.Blue, new Vector2(400, 410), 50, 0, -(float)Math.PI, ArcType.Sector, 4);
            drawBatch.FillArc(Brush.Blue, new Vector2(400, 480), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), ArcType.Sector, 4);

            drawBatch.End();
        }

        private static Texture2D _xor6;

        [TestSheet("Texture Fill")]
        public static void DrawTextureFill (DrawBatch drawBatch)
        {
            if (_xor6 == null)
                _xor6 = BuildXorTexture(drawBatch.GraphicsDevice, 6);

            TextureBrush brush1 = new TextureBrush(_xor6);
            TextureBrush brush2 = new TextureBrush(_xor6) {
                Transform = Matrix.CreateTranslation(-50f / _xor6.Width, -175f / _xor6.Height, 0)
            };
            TextureBrush brush3 = new TextureBrush(_xor6) {
                Transform = Matrix.CreateScale(.25f, .5f, 1f)
            };
            TextureBrush brush4 = new TextureBrush(_xor6) {
                Transform = Matrix.CreateRotationZ((float)Math.PI / 4)
            };
            TextureBrush brush5 = new TextureBrush(_xor6, .5f);
            TextureBrush brush6 = new TextureBrush(_xor6) {
                Color = Color.Purple
            };

            SetupDrawBatch(drawBatch);

            drawBatch.FillRectangle(brush1, new Rectangle(50, 50, 200, 100));
            drawBatch.FillRectangle(brush2, new Rectangle(50, 175, 200, 100));
            drawBatch.FillRectangle(brush3, new Rectangle(50, 300, 200, 100));
            drawBatch.FillRectangle(brush4, new Rectangle(50, 425, 200, 100));
            drawBatch.FillCircle(brush5, new Vector2(350, 100), 50);
            drawBatch.FillCircle(brush6, new Vector2(350, 225), 50);

            drawBatch.End();
        }

        [TestSheet("Primitive Ellipses")]
        public static void DrawPrimitiveEllipses (DrawBatch drawBatch)
        {
            SetupDrawBatch(drawBatch);

            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(50, 50, 50, 50));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(125, 50, 100, 50));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(250, 50, 150, 50));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(50, 125, 50, 100));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(125, 125, 100, 100));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(250, 125, 150, 100));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(50, 250, 50, 150));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(125, 250, 100, 150));
            drawBatch.DrawPrimitiveEllipse(Pen.Blue, new Rectangle(250, 250, 150, 150));

            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(425, 50, 100, 50), 0);
            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(425, 150, 100, 50), (float)Math.PI / 8);
            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(425, 250, 100, 50), (float)Math.PI / 4);
            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(425, 350, 100, 50), (float)Math.PI / 8 * 3);

            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(50, 425, 50, 100), 0);
            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(150, 425, 50, 100), (float)-Math.PI / 8);
            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(250, 425, 50, 100), (float)-Math.PI / 4);
            drawBatch.DrawPrimitiveEllipse(Pen.Red, new Rectangle(350, 425, 50, 100), (float)-Math.PI / 8 * 3);

            drawBatch.End();
        }

        [TestSheet("Ellipses")]
        public static void DrawEllipses (DrawBatch drawBatch)
        {
            Pen bluePen = new Pen(Color.Blue, 10);
            Pen redPen = new Pen(Color.Red, 10);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawEllipse(bluePen, new Rectangle(50, 50, 50, 50));
            drawBatch.DrawEllipse(bluePen, new Rectangle(125, 50, 100, 50));
            drawBatch.DrawEllipse(bluePen, new Rectangle(250, 50, 150, 50));
            drawBatch.DrawEllipse(bluePen, new Rectangle(50, 125, 50, 100));
            drawBatch.DrawEllipse(bluePen, new Rectangle(125, 125, 100, 100));
            drawBatch.DrawEllipse(bluePen, new Rectangle(250, 125, 150, 100));
            drawBatch.DrawEllipse(bluePen, new Rectangle(50, 250, 50, 150));
            drawBatch.DrawEllipse(bluePen, new Rectangle(125, 250, 100, 150));
            drawBatch.DrawEllipse(bluePen, new Rectangle(250, 250, 150, 150));

            drawBatch.DrawEllipse(redPen, new Rectangle(425, 50, 100, 50), 0);
            drawBatch.DrawEllipse(redPen, new Rectangle(425, 150, 100, 50), (float)Math.PI / 8);
            drawBatch.DrawEllipse(redPen, new Rectangle(425, 250, 100, 50), (float)Math.PI / 4);
            drawBatch.DrawEllipse(redPen, new Rectangle(425, 350, 100, 50), (float)Math.PI / 8 * 3);

            drawBatch.DrawEllipse(redPen, new Rectangle(50, 425, 50, 100), 0);
            drawBatch.DrawEllipse(redPen, new Rectangle(150, 425, 50, 100), (float)-Math.PI / 8);
            drawBatch.DrawEllipse(redPen, new Rectangle(250, 425, 50, 100), (float)-Math.PI / 4);
            drawBatch.DrawEllipse(redPen, new Rectangle(350, 425, 50, 100), (float)-Math.PI / 8 * 3);

            drawBatch.End();
        }

        [TestSheet("Filled Ellipses")]
        public static void DrawFilledEllipses (DrawBatch drawBatch)
        {
            SetupDrawBatch(drawBatch);

            drawBatch.FillEllipse(Brush.Blue, new Rectangle(50, 50, 50, 50));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(125, 50, 100, 50));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(250, 50, 150, 50));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(50, 125, 50, 100));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(125, 125, 100, 100));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(250, 125, 150, 100));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(50, 250, 50, 150));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(125, 250, 100, 150));
            drawBatch.FillEllipse(Brush.Blue, new Rectangle(250, 250, 150, 150));

            drawBatch.FillEllipse(Brush.Red, new Rectangle(425, 50, 100, 50), 0);
            drawBatch.FillEllipse(Brush.Red, new Rectangle(425, 150, 100, 50), (float)Math.PI / 8);
            drawBatch.FillEllipse(Brush.Red, new Rectangle(425, 250, 100, 50), (float)Math.PI / 4);
            drawBatch.FillEllipse(Brush.Red, new Rectangle(425, 350, 100, 50), (float)Math.PI / 8 * 3);

            drawBatch.FillEllipse(Brush.Red, new Rectangle(50, 425, 50, 100), 0);
            drawBatch.FillEllipse(Brush.Red, new Rectangle(150, 425, 50, 100), (float)-Math.PI / 8);
            drawBatch.FillEllipse(Brush.Red, new Rectangle(250, 425, 50, 100), (float)-Math.PI / 4);
            drawBatch.FillEllipse(Brush.Red, new Rectangle(350, 425, 50, 100), (float)-Math.PI / 8 * 3);

            drawBatch.End();
        }

        [TestSheet("Quadratic Bezier Curves")]
        public static void DrawQuadBeziers (DrawBatch drawBatch)
        {
            Pen bluePen = new Pen(Color.Blue, 15);
            Pen pointPen = new Pen(Color.Gray, 4);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawBezier(bluePen, new Vector2(50, 100), new Vector2(50, 50), new Vector2(100, 50));
            drawBatch.DrawBezier(bluePen, new Vector2(50, 250), new Vector2(50, 150), new Vector2(100, 150));

            Vector2[] wavePoints1 = new Vector2[] {
                new Vector2(150, 100), new Vector2(200, 150), new Vector2(250, 100), new Vector2(300, 50), new Vector2(350, 100),
                new Vector2(400, 150), new Vector2(450, 100), new Vector2(500, 50), new Vector2(550, 100),
            };

            drawBatch.DrawBeziers(bluePen, wavePoints1, BezierType.Quadratic);
            drawBatch.DrawPrimitivePath(Pen.Gray, wavePoints1);

            for (int i = 0; i < wavePoints1.Length; i++)
                drawBatch.DrawPoint(pointPen, wavePoints1[i]);

            Vector2[] wavePoints2 = new Vector2[] {
                new Vector2(150, 200), new Vector2(200, 300), new Vector2(250, 200), new Vector2(300, 100), new Vector2(350, 200),
                new Vector2(400, 300), new Vector2(450, 200), new Vector2(500, 100), new Vector2(550, 200),
            };

            drawBatch.DrawBeziers(bluePen, wavePoints2, BezierType.Quadratic);
            drawBatch.DrawPrimitivePath(Pen.Gray, wavePoints2);

            for (int i = 0; i < wavePoints2.Length; i++)
                drawBatch.DrawPoint(pointPen, wavePoints2[i]);

            Vector2[] loopPoints = new Vector2[] {
                new Vector2(250, 300), new Vector2(350, 300), new Vector2(350, 400), new Vector2(350, 500),
                new Vector2(250, 500), new Vector2(150, 500), new Vector2(150, 400), new Vector2(150, 300),
            };

            drawBatch.DrawBeziers(bluePen, loopPoints, BezierType.Quadratic, PathType.Closed);
            drawBatch.DrawPrimitivePath(Pen.Gray, loopPoints, PathType.Closed);

            for (int i = 0; i < loopPoints.Length; i++)
                drawBatch.DrawPoint(pointPen, loopPoints[i]);

            drawBatch.End();
        }

        [TestSheet("Cubic Bezier Curves")]
        public static void DrawCubicBeziers (DrawBatch drawBatch)
        {
            Pen bluePen = new Pen(Color.Blue, 15);
            Pen pointPen = new Pen(Color.Gray, 4);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawBezier(bluePen, new Vector2(50, 100), new Vector2(50, 50), new Vector2(150, 50), new Vector2(150, 100));
            drawBatch.DrawBezier(bluePen, new Vector2(50, 250), new Vector2(50, 150), new Vector2(150, 150), new Vector2(150, 250));

            Vector2[] wavePoints = new Vector2[] {
                new Vector2(50, 350), new Vector2(50, 400), new Vector2(150, 400), new Vector2(150, 350), new Vector2(150, 300),
                new Vector2(250, 300), new Vector2(250, 350), new Vector2(250, 400), new Vector2(350, 400), new Vector2(350, 350),
                new Vector2(350, 300), new Vector2(450, 300), new Vector2(450, 350),
            };

            drawBatch.DrawBeziers(bluePen, wavePoints, BezierType.Cubic);
            drawBatch.DrawPrimitivePath(Pen.Gray, wavePoints);

            for (int i = 0; i < wavePoints.Length; i++)
                drawBatch.DrawPoint(pointPen, wavePoints[i]);

            Vector2[] loopPoints = new Vector2[] {
                new Vector2(225, 75), new Vector2(250, 50), new Vector2(275, 50), new Vector2(300, 75),
                new Vector2(325, 100), new Vector2(325, 125), new Vector2(300, 150), new Vector2(275, 175),
                new Vector2(250, 175), new Vector2(225, 150), new Vector2(200, 125), new Vector2(200, 100),
            };

            drawBatch.DrawBeziers(bluePen, loopPoints, BezierType.Cubic, PathType.Closed);
            drawBatch.DrawPrimitivePath(Pen.Gray, loopPoints, PathType.Closed);

            for (int i = 0; i < loopPoints.Length; i++)
                drawBatch.DrawPoint(pointPen, loopPoints[i]);

            drawBatch.End();
        }

        [TestSheet("Gradient Pens")]
        public static void DrawGradientPens (DrawBatch drawBatch)
        {
            SetupDrawBatch(drawBatch);

            Pen gradWidth = new GradientPen(Color.Yellow, Color.Blue, 15);

            GraphicsPath widthStar = new GraphicsPath(gradWidth, StarPoints(new Vector2(325, 75), 5, 50, 25, 0, false), PathType.Open);

            drawBatch.DrawLine(gradWidth, new Vector2(25, 25), new Vector2(125, 125));
            drawBatch.DrawCircle(gradWidth, new Vector2(200, 75), 50);
            drawBatch.DrawPath(widthStar);

            drawBatch.End();
        }

        private static GraphicsPath _outerFlower;
        private static GraphicsPath _innerFlower;
        private static GraphicsPath[] _lillypads;

        [TestSheet("Water Lily")]
        public static void DrawWaterLily (DrawBatch drawBatch)
        {
            Vector2 center = new Vector2(300, 250);

            if (_outerFlower == null) {
                Pen pen = new Pen(Color.Pink, 15) { Alignment = PenAlignment.Outset };
                _outerFlower = CreateFlowerGP(pen, center, 10, 150, 100, 0);
            }

            if (_innerFlower == null) {
                Pen pen = new Pen(Color.HotPink, 10) { Alignment = PenAlignment.Outset };
                _innerFlower = CreateFlowerGP(pen, center, 15, 100, 50, 0);
            }

            if (_lillypads == null) {
                _lillypads = new GraphicsPath[3];
                Pen pen = new Pen(Color.Green, 15) { Alignment = PenAlignment.Center, LineJoin = LineJoin.Bevel };

                _lillypads[0] = CreateLillyPadGP(pen, new Vector2(400, 400), 125, 0);
                _lillypads[1] = CreateLillyPadGP(pen, new Vector2(200, 250), 150, 3);
                _lillypads[2] = CreateLillyPadGP(pen, new Vector2(450, 150), 100, 2);
            }

            SetupDrawBatch(drawBatch);

            foreach (GraphicsPath path in _lillypads)
                drawBatch.DrawPath(path);
            drawBatch.DrawPath(_outerFlower);
            drawBatch.DrawPath(_innerFlower);

            drawBatch.End();
        }

        private static PathBuilder _lilly2_padBuilder;
        private static GraphicsPath _lilly2_pad;
        private static GraphicsPath _lilly2_outerFlower;
        private static GraphicsPath _lilly2_innerFlower;

        [TestSheet("Water Lily 2")]
        public static void DrawWaterLily2 (DrawBatch drawBatch)
        {
            Vector2 center = new Vector2(200, 200);

            if (_lilly2_pad == null) {
                Pen pen = new Pen(Color.Green, 15) { Alignment = PenAlignment.Center };
                _lilly2_padBuilder = BuildLillyPad(center, 150, 0);
                _lilly2_pad = _lilly2_padBuilder.Stroke(pen, PathType.Closed);
            }

            if (_lilly2_outerFlower == null) {
                Pen pen = new Pen(Color.White * 0.75f, 15) { Alignment = PenAlignment.Outset };
                _lilly2_outerFlower = CreateFlowerGP(pen, center, 8, 120, 100, (float)(Math.PI / 8));
            }

            if (_lilly2_innerFlower == null) {
                Pen pen = new Pen(Color.MediumPurple * 0.5f, 10) { Alignment = PenAlignment.Outset };
                _lilly2_innerFlower = CreateFlowerGP(pen, center, 16, 105, 60, 0);
            }

            SetupDrawBatch(drawBatch);

            Vector2 origin = new Vector2(200, 200);
            float startAngle = (float)(Math.PI / 16) * 25; // 11:20
            float arcLength = (float)(Math.PI / 16) * 30;

            drawBatch.FillCircle(new SolidColorBrush(Color.SkyBlue), origin, 175);
            drawBatch.FillArc(new SolidColorBrush(Color.LimeGreen), origin, 150, startAngle, arcLength, ArcType.Sector);
            drawBatch.DrawClosedArc(new Pen(Color.Green, 15), origin, 150, startAngle, arcLength, ArcType.Sector);
            drawBatch.DrawPath(_lilly2_outerFlower);
            drawBatch.DrawPath(_lilly2_innerFlower);

            drawBatch.End();
        }

        [TestSheet("Graphics Path 1")]
        public static void DrawGraphicsPath1 (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Green, 15);

            List<Vector2> path1 = new List<Vector2>() {
                new Vector2(50, 50), new Vector2(100, 50), new Vector2(100, 100), new Vector2(50, 100),
            };

            GraphicsPath gpathf = new GraphicsPath(thickPen, path1, PathType.Closed);

            path1.Reverse();
            for (int i = 0; i < path1.Count; i++)
                path1[i] = new Vector2(path1[i].X + 100, path1[i].Y);

            GraphicsPath gpathr = new GraphicsPath(thickPen, path1, PathType.Closed);

            for (int i = 0; i < path1.Count; i++)
                path1[i] = new Vector2(path1[i].X, path1[i].Y + 100);

            GraphicsPath gpath2r = new GraphicsPath(thickPen, path1);

            path1.Reverse();
            for (int i = 0; i < path1.Count; i++)
                path1[i] = new Vector2(path1[i].X - 100, path1[i].Y);

            GraphicsPath gpath2f = new GraphicsPath(thickPen, path1);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawPath(gpathf);
            drawBatch.DrawPath(gpathr);
            drawBatch.DrawPath(gpath2f);
            drawBatch.DrawPath(gpath2r);

            drawBatch.End();
        }

        [TestSheet("Graphics Path Outline")]
        public static void DrawGraphicsPathOutline (DrawBatch drawBatch)
        {
            Pen thickPen = new Pen(Color.Green, 15);

            Pen outlinePen = new Pen(Color.GreenYellow, 5);
            Pen outlinePenInset = new Pen(Color.GreenYellow, 5) { Alignment = PenAlignment.Inset };
            Pen outlinePenOutset = new Pen(Color.GreenYellow, 5) { Alignment = PenAlignment.Outset };

            List<Vector2> path1 = new List<Vector2>() {
                new Vector2(50, 50), new Vector2(100, 50), new Vector2(100, 100), new Vector2(50, 100),
            };

            GraphicsPath gpathf = new GraphicsPath(thickPen, outlinePen, path1, PathType.Closed);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpathr = new GraphicsPath(thickPen, outlinePen, path1, PathType.Closed);

            ShiftPath(path1, 100, 0);
            GraphicsPath gpathfi = new GraphicsPath(thickPen, outlinePenInset, path1, PathType.Closed);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpathri = new GraphicsPath(thickPen, outlinePenInset, path1, PathType.Closed);

            ShiftPath(path1, 100, 0);
            GraphicsPath gpathfo = new GraphicsPath(thickPen, outlinePenOutset, path1, PathType.Closed);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpathro = new GraphicsPath(thickPen, outlinePenOutset, path1, PathType.Closed);

            ShiftPath(path1, -500, 100);
            GraphicsPath gpath2r = new GraphicsPath(thickPen, outlinePen, path1);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpath2f = new GraphicsPath(thickPen, outlinePen, path1);

            ShiftPath(path1, 100, 0);
            GraphicsPath gpath2fi = new GraphicsPath(thickPen, outlinePenInset, path1);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpath2ri = new GraphicsPath(thickPen, outlinePenInset, path1);

            ShiftPath(path1, 100, 0);
            GraphicsPath gpath2fo = new GraphicsPath(thickPen, outlinePenOutset, path1);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpath2ro = new GraphicsPath(thickPen, outlinePenOutset, path1);

            ShiftPath(path1, -500, 100);
            GraphicsPath gpath3f = new GraphicsPath(thickPen, outlinePen, path1, PathType.Closed, StrokeType.Outline);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpath3r = new GraphicsPath(thickPen, outlinePen, path1, PathType.Closed, StrokeType.Outline);

            ShiftPath(path1, 100, 0);
            GraphicsPath gpath4f = new GraphicsPath(thickPen, outlinePen, path1, StrokeType.Outline);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            GraphicsPath gpath4r = new GraphicsPath(thickPen, outlinePen, path1, StrokeType.Outline);

            SetupDrawBatch(drawBatch);

            drawBatch.DrawPath(gpathf);
            drawBatch.DrawPath(gpathr);
            drawBatch.DrawPath(gpathfi);
            drawBatch.DrawPath(gpathri);
            drawBatch.DrawPath(gpathfo);
            drawBatch.DrawPath(gpathro);
            drawBatch.DrawPath(gpath2f);
            drawBatch.DrawPath(gpath2r);
            drawBatch.DrawPath(gpath2fi);
            drawBatch.DrawPath(gpath2ri);
            drawBatch.DrawPath(gpath2fo);
            drawBatch.DrawPath(gpath2ro);
            drawBatch.DrawPath(gpath3f);
            drawBatch.DrawPath(gpath3r);
            drawBatch.DrawPath(gpath4f);
            drawBatch.DrawPath(gpath4r);

            drawBatch.End();
        }

        private static List<Vector2> ShiftPath (List<Vector2> path, float x, float y)
        {
            for (int i = 0; i < path.Count; i++)
                path[i] = new Vector2(path[i].X + x, path[i].Y + y);
            return path;
        }

        private static RasterizerState GetCommonRasterizerState ()
        {
            return new RasterizerState() {
                FillMode = DemoState.FillMode,
                MultiSampleAntiAlias = DemoState.MultisampleAA,
            };
        }

        private static GraphicsPath CreateFlowerGP (Pen pen, Vector2 center, int petalCount, float petalLength, float petalWidth, float rotation)
        {
            List<Vector2> points = StarPoints(center, petalCount / 2, petalLength, petalLength, rotation, false);

            PathBuilder builder = new PathBuilder();
            builder.AddPoint(center);

            foreach (Vector2 point in points) {
                builder.AddArcByPoint(point, petalWidth / 2);
                builder.AddArcByPoint(center, petalWidth / 2);
            }

            return builder.Stroke(pen, PathType.Closed);
        }

        private static PathBuilder BuildLillyPad (Vector2 center, int radius, float rotation)
        {
            float segment = (float)(Math.PI * 2 / 32);

            PathBuilder builder = new PathBuilder();

            builder.AddPoint(center);
            builder.AddLine(radius, segment * 25 + rotation);
            builder.AddArcByAngle(center, segment * 30, radius / 2);

            return builder;
        }

        private static GraphicsPath CreateLillyPadGP (Pen pen, Vector2 center, int radius, float rotation)
        {
            float segment = (float)(Math.PI * 2 / 32);

            PathBuilder builder = new PathBuilder();

            builder.AddPoint(center);
            builder.AddLine(radius, segment * 25 + rotation);
            builder.AddArcByAngle(center, segment * 30, radius / 2);

            return builder.Stroke(pen, PathType.Closed);
        }

        private static List<Vector2> StarPoints (Vector2 center, int pointCount, float outerRadius, float innerRadius, float rotation, bool close)
        {
            List<Vector2> points = new List<Vector2>();

            int limit = (close) ? pointCount * 2 + 1 : pointCount * 2;

            float rot = (float)((Math.PI * 2) / (pointCount * 2));
            for (int i = 0; i < limit; i++) {
                float si = (float)Math.Sin(-i * rot + Math.PI + rotation);
                float ci = (float)Math.Cos(-i * rot + Math.PI + rotation);

                if (i % 2 == 0)
                    points.Add(center + new Vector2(si, ci) * outerRadius);
                else
                    points.Add(center + new Vector2(si, ci) * innerRadius);
            }

            return points;
        }

        private static Texture2D BuildXorTexture (GraphicsDevice device, int bits)
        {
            if (bits < 1 || bits > 8)
                throw new ArgumentException("Xor texture must have between 1 and 8 bits", "bits");

            Texture2D tex = new Texture2D(device, 1 << bits, 1 << bits);
            Color[] data = new Color[tex.Width * tex.Height];

            for (int y = 0; y < tex.Height; y++) {
                for (int x = 0; x < tex.Width; x++) {
                    float lum = ((x << (8 - bits)) ^ (y << (8 - bits))) / 255f;
                    data[y * tex.Width + x] = new Color(lum, lum, lum);
                }
            }

            tex.SetData(data);

            return tex;
        }
    }
}
