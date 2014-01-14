using System;
using LilyPath;
using Microsoft.Xna.Framework;

namespace LilyPathDemo.TestSheets
{
    [TestName("Primitive Ellipses")]
    public class PrimitiveEllipsesSheet : TestSheet
    {
        public override void Draw (DrawBatch drawBatch)
        {
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
        }
    }
}
