using LilyPath;
using LilyPath.Pens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Gradient Pens")]
    public class GradientPensSheet : TestSheet
    {
        private Pen _gradWidth;
        private Pen _gradLength;

        private GraphicsPath _widthStar;
        private GraphicsPath _lengthStar;

        public override void Setup (GraphicsDevice device)
        {
            _gradWidth = new GradientPen(Color.Lime, Color.Blue, 15);
            _gradLength = new PathGradientPen(Color.Lime, Color.Blue, 15);

            PathBuilder pathBuilder = new PathBuilder() { CalculateLengths = true };
            pathBuilder.AddPath(StarPoints(new Vector2(325, 75), 5, 50, 25, 0, false));

            _widthStar = pathBuilder.Stroke(_gradWidth, PathType.Open);
            _lengthStar = pathBuilder.Stroke(_gradLength, Matrix.CreateTranslation(0, 125, 0), PathType.Open);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawLine(_gradWidth, new Vector2(25, 25), new Vector2(125, 125));
            drawBatch.DrawCircle(_gradWidth, new Vector2(200, 75), 50);
            drawBatch.DrawPath(_widthStar);

            drawBatch.DrawLine(_gradLength, new Vector2(25, 150), new Vector2(125, 250));
            drawBatch.DrawCircle(_gradLength, new Vector2(200, 200), 50);
            drawBatch.DrawPath(_lengthStar);
        }
    }
}
