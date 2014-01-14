using System;
using LilyPath;
using Microsoft.Xna.Framework;

namespace LilyPathDemo.TestSheets
{
    [TestName("Primitive Closed Arcs")]
    public class PrimitiveClosedArcsSheet : TestSheet
    {
        public override void Draw (DrawBatch drawBatch)
        {
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
        }
    }
}
