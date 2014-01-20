using LilyPath;
using LilyPath.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Grid Shape")]
    public class GridShapeSheet : TestSheet
    {
        private DrawCache _cache;

        public override void Setup (GraphicsDevice device)
        {
            Pen pen = new Pen(new Color(Color.Blue, 92), 6);

            _cache = new Grid(12, 10).Compile(pen, 30, 30, 30 * 12, 30 * 10);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.FillRectangle(Brush.Gray, new Vector2(200, 15), 300, 300);
            drawBatch.DrawCache(_cache);
        }
    }
}
