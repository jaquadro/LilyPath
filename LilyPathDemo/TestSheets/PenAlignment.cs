using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Pen Alignment")]
    public class PenAlignmentSheet : TestSheet
    {
        private List<Vector2> _starPoints1;
        private List<Vector2> _starPoints2;
        private List<Vector2> _starPoints3;

        private Pen _insetPen;
        private Pen _centerPen;
        private Pen _outsetPen;

        private GraphicsPath _insetPath;
        private GraphicsPath _centerPath;
        private GraphicsPath _outsetPath;

        public override void Setup (GraphicsDevice device)
        {
            _starPoints1 = StarPoints(new Vector2(125, 150), 5, 100, 50, 0, false);
            _starPoints2 = StarPoints(new Vector2(350, 275), 5, 100, 50, 0, false);
            _starPoints3 = StarPoints(new Vector2(125, 400), 5, 100, 50, 0, false);

            _insetPen = new Pen(Color.MediumTurquoise, 10) {
                Alignment = PenAlignment.Inset
            };
            _centerPen = new Pen(Color.MediumTurquoise, 10) {
                Alignment = PenAlignment.Center
            };
            _outsetPen = new Pen(Color.MediumTurquoise, 10) {
                Alignment = PenAlignment.Outset
            };

            _insetPath = new GraphicsPath(_insetPen, _starPoints1, PathType.Closed);
            _centerPath = new GraphicsPath(_centerPen, _starPoints2, PathType.Closed);
            _outsetPath = new GraphicsPath(_outsetPen, _starPoints3, PathType.Closed);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawPath(_insetPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), _starPoints1, PathType.Closed);
            drawBatch.DrawPath(_centerPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), _starPoints2, PathType.Closed);
            drawBatch.DrawPath(_outsetPath);
            drawBatch.DrawPrimitivePath(new Pen(Color.OrangeRed), _starPoints3, PathType.Closed);
        }
    }
}
