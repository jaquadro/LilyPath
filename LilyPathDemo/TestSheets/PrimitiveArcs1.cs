using System;
using LilyPath;
using Microsoft.Xna.Framework;

namespace LilyPathDemo.TestSheets
{
    [TestName("Primitive Arcs 1")]
    public class PrimitiveArcs1Sheet : TestSheet
    {
        public override void Draw (DrawBatch drawBatch)
        {
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
        }
    }
}
