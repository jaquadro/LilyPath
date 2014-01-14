using System;
using LilyPath;
using Microsoft.Xna.Framework;

namespace LilyPathDemo.TestSheets
{
    [TestName("Filled Shapes")]
    public class FilledShapesSheet : TestSheet
    {
        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.FillRectangle(Brush.Green, new Rectangle(50, 50, 200, 100));
            drawBatch.FillCircle(Brush.Blue, new Vector2(350, 100), 50);
            drawBatch.FillCircle(Brush.Blue, new Vector2(500, 100), 50, 16);
            drawBatch.FillPath(Brush.Gray, StarPoints(new Vector2(150, 300), 8, 100, 50, 0, false));
            drawBatch.FillRectangle(Brush.Green, new Rectangle(300, 250, 200, 100), (float)Math.PI / 4f);
        }
    }
}
