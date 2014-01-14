using LilyPath;
using Microsoft.Xna.Framework;

namespace LilyPathDemo.TestSheets
{
    [TestName("Primitive Arcs 2")]
    public class PrimitiveArcs2Sheet : TestSheet
    {
        public override void Draw (DrawBatch drawBatch)
        {
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
        }
    }
}
