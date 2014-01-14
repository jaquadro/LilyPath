using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Graphics Path")]
    public class GraphicsPathSheet : TestSheet
    {
        private Pen _thickPen;

        private GraphicsPath _gpathf;
        private GraphicsPath _gpathr;
        private GraphicsPath _gpath2f;
        private GraphicsPath _gpath2r;

        public override void Setup (GraphicsDevice device)
        {
            _thickPen = new Pen(Color.Green, 15);

            List<Vector2> path1 = new List<Vector2>() {
                new Vector2(50, 50), new Vector2(100, 50), new Vector2(100, 100), new Vector2(50, 100),
            };

            _gpathf = new GraphicsPath(_thickPen, path1, PathType.Closed);

            path1.Reverse();
            for (int i = 0; i < path1.Count; i++)
                path1[i] = new Vector2(path1[i].X + 100, path1[i].Y);

            _gpathr = new GraphicsPath(_thickPen, path1, PathType.Closed);

            for (int i = 0; i < path1.Count; i++)
                path1[i] = new Vector2(path1[i].X, path1[i].Y + 100);

            _gpath2r = new GraphicsPath(_thickPen, path1);

            path1.Reverse();
            for (int i = 0; i < path1.Count; i++)
                path1[i] = new Vector2(path1[i].X - 100, path1[i].Y);

            _gpath2f = new GraphicsPath(_thickPen, path1);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawPath(_gpathf);
            drawBatch.DrawPath(_gpathr);
            drawBatch.DrawPath(_gpath2f);
            drawBatch.DrawPath(_gpath2r);
        }
    }
}
