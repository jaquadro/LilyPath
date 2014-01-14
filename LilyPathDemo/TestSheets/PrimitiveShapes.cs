using System;
using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Primitive Shapes")]
    public class PrimitiveShapesSheet : TestSheet
    {
        private List<Vector2> _wavy = new List<Vector2>();

        public override void Setup (GraphicsDevice device)
        {
            for (int i = 0; i < 20; i++) {
                if (i % 2 == 0)
                    _wavy.Add(new Vector2(50 + i * 10, 100));
                else
                    _wavy.Add(new Vector2(50 + i * 10, 110));
            }
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawPrimitiveLine(Pen.Blue, new Vector2(50, 50), new Vector2(250, 50));
            drawBatch.DrawPrimitivePath(Pen.Red, _wavy);
            drawBatch.DrawPrimitiveRectangle(Pen.Magenta, new Rectangle(50, 160, 200, 100));
            drawBatch.DrawPrimitiveCircle(Pen.Black, new Vector2(350, 100), 50);
            drawBatch.DrawPrimitiveCircle(Pen.DarkGray, new Vector2(350, 225), 50, 16);
            drawBatch.DrawPrimitiveRectangle(Pen.Green, new Rectangle(50, 350, 200, 100), (float)Math.PI / 4f);
        }
    }
}
