using LilyPath;
using LilyPath.Pens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Gradient Pens Alignment")]
    public class GradientPensAlignment : TestSheet
    {
        private Vector2[] _baseCoords = new Vector2[] { new Vector2(0, 0), new Vector2(25, 50), new Vector2(0, 100) };

        private Pen _gradWidthInner;
        private Pen _gradWidthCenter;
        private Pen _gradWidthOuter;

        private GraphicsPath[] _gPaths = new GraphicsPath[3];

        public override void Setup (GraphicsDevice device)
        {
            _gradWidthInner = new GradientPen(Color.Lime, Color.Cyan, 15) { Alignment = PenAlignment.Inset, StartCap = LineCap.Square, EndCap = LineCap.Square };
            _gradWidthCenter = new GradientPen(Color.Lime, Color.Cyan, 15) { StartCap = LineCap.Square, EndCap = LineCap.Square };
            _gradWidthOuter = new GradientPen(Color.Lime, Color.Cyan, 15) { Alignment = PenAlignment.Outset, StartCap = LineCap.Square, EndCap = LineCap.Square };

            Pen[] pens = new Pen[] { _gradWidthInner, _gradWidthCenter, _gradWidthOuter };
            for (int i = 0; i < _gPaths.Length; i++) {
                PathBuilder builder = new PathBuilder();
                foreach (Vector2 v in _baseCoords)
                    builder.AddPoint(v + Offset(i));
                _gPaths[i] = builder.Stroke(pens[i]);
            }
        }

        public override void Draw (DrawBatch drawBatch)
        {
            for (int i = 0; i < _gPaths.Length; i++) {
                GraphicsPath path = _gPaths[i];
                drawBatch.DrawPath(path);
                for (int j = 0; j < _baseCoords.Length - 1; j++)
                    drawBatch.DrawLine(PrimitivePen.Black, _baseCoords[j] + Offset(i), _baseCoords[j + 1] + Offset(i));
            }
        }

        private static Vector2 Offset (int i)
        {
            return new Vector2(100 + i * 50, 100);
        }
    }
}
