using System;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Ellipses")]
    public class EllipsesSheet : TestSheet
    {
        private Pen _bluePen;
        private Pen _redPen;

        public override void Setup (GraphicsDevice device)
        {
            _bluePen = new Pen(Color.Blue, 10);
            _redPen = new Pen(Color.Red, 10);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawEllipse(_bluePen, new Rectangle(50, 50, 50, 50));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(125, 50, 100, 50));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(250, 50, 150, 50));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(50, 125, 50, 100));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(125, 125, 100, 100));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(250, 125, 150, 100));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(50, 250, 50, 150));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(125, 250, 100, 150));
            drawBatch.DrawEllipse(_bluePen, new Rectangle(250, 250, 150, 150));

            drawBatch.DrawEllipse(_redPen, new Rectangle(425, 50, 100, 50), 0);
            drawBatch.DrawEllipse(_redPen, new Rectangle(425, 150, 100, 50), (float)Math.PI / 8);
            drawBatch.DrawEllipse(_redPen, new Rectangle(425, 250, 100, 50), (float)Math.PI / 4);
            drawBatch.DrawEllipse(_redPen, new Rectangle(425, 350, 100, 50), (float)Math.PI / 8 * 3);

            drawBatch.DrawEllipse(_redPen, new Rectangle(50, 425, 50, 100), 0);
            drawBatch.DrawEllipse(_redPen, new Rectangle(150, 425, 50, 100), (float)-Math.PI / 8);
            drawBatch.DrawEllipse(_redPen, new Rectangle(250, 425, 50, 100), (float)-Math.PI / 4);
            drawBatch.DrawEllipse(_redPen, new Rectangle(350, 425, 50, 100), (float)-Math.PI / 8 * 3);
        }
    }
}
