using System;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Arcs 2")]
    public class Arcs2Sheet : TestSheet
    {
        private Pen _thickPen;

        public override void Setup (GraphicsDevice device)
        {
            _thickPen = new Pen(Color.Blue, 15);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawArc(_thickPen, new Vector2(50, 75), new Vector2(150, 75), 25);
            drawBatch.DrawArc(_thickPen, new Vector2(50, 125), new Vector2(150, 125), 50);
            drawBatch.DrawArc(_thickPen, new Vector2(50, 200), new Vector2(150, 200), 75);

            drawBatch.DrawArc(_thickPen, new Vector2(50, 225), new Vector2(150, 225), -75);
            drawBatch.DrawArc(_thickPen, new Vector2(50, 300), new Vector2(150, 300), -50);
            drawBatch.DrawArc(_thickPen, new Vector2(50, 350), new Vector2(150, 350), -25);

            drawBatch.DrawArc(_thickPen, new Vector2(175, 75), new Vector2(275, 75), 25, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(175, 125), new Vector2(275, 125), 50, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(175, 200), new Vector2(275, 200), 75, 16);

            drawBatch.DrawArc(_thickPen, new Vector2(175, 225), new Vector2(275, 225), -75, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(175, 300), new Vector2(275, 300), -50, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(175, 350), new Vector2(275, 350), -25, 16);

            drawBatch.DrawArc(_thickPen, new Vector2(325, 50), new Vector2(325, 150), -25);
            drawBatch.DrawArc(_thickPen, new Vector2(375, 50), new Vector2(375, 150), -50);
            drawBatch.DrawArc(_thickPen, new Vector2(450, 50), new Vector2(450, 150), -75);

            drawBatch.DrawArc(_thickPen, new Vector2(475, 50), new Vector2(475, 150), 75);
            drawBatch.DrawArc(_thickPen, new Vector2(550, 50), new Vector2(550, 150), 50);
            drawBatch.DrawArc(_thickPen, new Vector2(600, 50), new Vector2(600, 150), 25);

            drawBatch.DrawArc(_thickPen, new Vector2(325, 175), new Vector2(325, 275), -25, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(375, 175), new Vector2(375, 275), -50, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(450, 175), new Vector2(450, 275), -75, 16);

            drawBatch.DrawArc(_thickPen, new Vector2(475, 175), new Vector2(475, 275), 75, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(550, 175), new Vector2(550, 275), 50, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(600, 175), new Vector2(600, 275), 25, 16);

            drawBatch.DrawArc(_thickPen, new Vector2(325, 300), new Vector2(325, 400), -25, 4);
            drawBatch.DrawArc(_thickPen, new Vector2(375, 300), new Vector2(375, 400), -50, 4);
            drawBatch.DrawArc(_thickPen, new Vector2(450, 300), new Vector2(450, 400), -75, 4);

            drawBatch.DrawArc(_thickPen, new Vector2(475, 300), new Vector2(475, 400), 75, 4);
            drawBatch.DrawArc(_thickPen, new Vector2(550, 300), new Vector2(550, 400), 50, 4);
            drawBatch.DrawArc(_thickPen, new Vector2(600, 300), new Vector2(600, 400), 25, 4);
        }
    }
}
