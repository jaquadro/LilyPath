using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Cubic Bezier Curves")]
    public class CubicBezierSheet : TestSheet
    {
        Pen _bluePen;
        Pen _pointPen;

        Vector2[] _wavePoints;
        Vector2[] _loopPoints;

        public override void Setup (GraphicsDevice device)
        {
            _bluePen = new Pen(Color.Blue, 15);
            _pointPen = new Pen(Color.Gray, 4);

            _wavePoints = new Vector2[] {
                new Vector2(50, 350), new Vector2(50, 400), new Vector2(150, 400), new Vector2(150, 350), new Vector2(150, 300),
                new Vector2(250, 300), new Vector2(250, 350), new Vector2(250, 400), new Vector2(350, 400), new Vector2(350, 350),
                new Vector2(350, 300), new Vector2(450, 300), new Vector2(450, 350),
            };

            _loopPoints = new Vector2[] {
                new Vector2(225, 75), new Vector2(250, 50), new Vector2(275, 50), new Vector2(300, 75),
                new Vector2(325, 100), new Vector2(325, 125), new Vector2(300, 150), new Vector2(275, 175),
                new Vector2(250, 175), new Vector2(225, 150), new Vector2(200, 125), new Vector2(200, 100),
            };
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawBezier(_bluePen, new Vector2(50, 100), new Vector2(50, 50), new Vector2(150, 50), new Vector2(150, 100));
            drawBatch.DrawBezier(_bluePen, new Vector2(50, 250), new Vector2(50, 150), new Vector2(150, 150), new Vector2(150, 250));

            drawBatch.DrawBeziers(_bluePen, _wavePoints, BezierType.Cubic);
            drawBatch.DrawPrimitivePath(Pen.Gray, _wavePoints);

            for (int i = 0; i < _wavePoints.Length; i++)
                drawBatch.DrawPoint(_pointPen, _wavePoints[i]);

            drawBatch.DrawBeziers(_bluePen, _loopPoints, BezierType.Cubic, PathType.Closed);
            drawBatch.DrawPrimitivePath(Pen.Gray, _loopPoints, PathType.Closed);

            for (int i = 0; i < _loopPoints.Length; i++)
                drawBatch.DrawPoint(_pointPen, _loopPoints[i]);
        }
    }
}
