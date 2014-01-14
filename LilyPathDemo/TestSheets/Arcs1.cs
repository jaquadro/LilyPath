using System;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Arcs 1")]
    public class Arcs1Sheet : TestSheet
    {
        private Pen _thickPen;

        public override void Setup (GraphicsDevice device)
        {
            _thickPen = new Pen(Color.Blue, 15);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawArc(_thickPen, new Vector2(100, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5));
            drawBatch.DrawArc(_thickPen, new Vector2(100, 125), 50, 0, -(float)Math.PI);
            drawBatch.DrawArc(_thickPen, new Vector2(100, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5));

            drawBatch.DrawArc(_thickPen, new Vector2(100, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5));
            drawBatch.DrawArc(_thickPen, new Vector2(100, 325), 50, 0, (float)Math.PI);
            drawBatch.DrawArc(_thickPen, new Vector2(100, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5));

            drawBatch.DrawArc(_thickPen, new Vector2(250, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), 16);
            drawBatch.DrawArc(_thickPen, new Vector2(250, 125), 50, 0, -(float)Math.PI, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(250, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), 16);

            drawBatch.DrawArc(_thickPen, new Vector2(250, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5), 16);
            drawBatch.DrawArc(_thickPen, new Vector2(250, 325), 50, 0, (float)Math.PI, 16);
            drawBatch.DrawArc(_thickPen, new Vector2(250, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 16);

            drawBatch.DrawArc(_thickPen, new Vector2(400, 125), 75, -(float)(Math.PI * 0.25), -(float)(Math.PI * 0.5), 4);
            drawBatch.DrawArc(_thickPen, new Vector2(400, 125), 50, 0, -(float)Math.PI, 4);
            drawBatch.DrawArc(_thickPen, new Vector2(400, 175), 50, (float)(Math.PI * 0.25), -(float)(Math.PI * 1.5), 4);

            drawBatch.DrawArc(_thickPen, new Vector2(400, 275), 50, -(float)(Math.PI * 0.25), (float)(Math.PI * 1.5), 4);
            drawBatch.DrawArc(_thickPen, new Vector2(400, 325), 50, 0, (float)Math.PI, 4);
            drawBatch.DrawArc(_thickPen, new Vector2(400, 325), 75, (float)(Math.PI * 0.25), (float)(Math.PI * 0.5), 4);
        }
    }
}
