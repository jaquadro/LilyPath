using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Quadratic Bezier Curves")]
    public class QuadraticBezierSheet : TestSheet
    {
        Pen _bluePen;
        Pen _pointPen;

        Vector2[] _wavePoints1;
        Vector2[] _wavePoints2;
        Vector2[] _loopPoints;

        public override void Setup (GraphicsDevice device)
        {
            _bluePen = new Pen(Color.Blue, 15);
            _pointPen = new Pen(Color.Gray, 4);

            _wavePoints1 = new Vector2[] {
                new Vector2(150, 100), new Vector2(200, 150), new Vector2(250, 100), new Vector2(300, 50), new Vector2(350, 100),
                new Vector2(400, 150), new Vector2(450, 100), new Vector2(500, 50), new Vector2(550, 100),
            };

            _wavePoints2 = new Vector2[] {
                new Vector2(150, 200), new Vector2(200, 300), new Vector2(250, 200), new Vector2(300, 100), new Vector2(350, 200),
                new Vector2(400, 300), new Vector2(450, 200), new Vector2(500, 100), new Vector2(550, 200),
            };

           _loopPoints = new Vector2[] {
                new Vector2(250, 300), new Vector2(350, 300), new Vector2(350, 400), new Vector2(350, 500),
                new Vector2(250, 500), new Vector2(150, 500), new Vector2(150, 400), new Vector2(150, 300),
            };
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawBezier(_bluePen, new Vector2(50, 100), new Vector2(50, 50), new Vector2(100, 50));
            drawBatch.DrawBezier(_bluePen, new Vector2(50, 250), new Vector2(50, 150), new Vector2(100, 150));

            drawBatch.DrawBeziers(_bluePen, _wavePoints1, BezierType.Quadratic);
            drawBatch.DrawPrimitivePath(Pen.Gray, _wavePoints1);

            for (int i = 0; i < _wavePoints1.Length; i++)
                drawBatch.DrawPoint(_pointPen, _wavePoints1[i]);

            drawBatch.DrawBeziers(_bluePen, _wavePoints2, BezierType.Quadratic);
            drawBatch.DrawPrimitivePath(Pen.Gray, _wavePoints2);

            for (int i = 0; i < _wavePoints2.Length; i++)
                drawBatch.DrawPoint(_pointPen, _wavePoints2[i]);

            drawBatch.DrawBeziers(_bluePen, _loopPoints, BezierType.Quadratic, PathType.Closed);
            drawBatch.DrawPrimitivePath(Pen.Gray, _loopPoints, PathType.Closed);

            for (int i = 0; i < _loopPoints.Length; i++)
                drawBatch.DrawPoint(_pointPen, _loopPoints[i]);
        }
    }
}
