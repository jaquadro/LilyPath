using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Graphics Path Outline")]
    public class GraphicsPathOutlineSheet : TestSheet
    {
        private Pen _thickpen;
        private Pen _outlinePen;
        private Pen _outlinePenInset;
        private Pen _outlinePenOutset;

        private GraphicsPath _gpathf;
        private GraphicsPath _gpathr;
        private GraphicsPath _gpathfi;
        private GraphicsPath _gpathri;
        private GraphicsPath _gpathfo;
        private GraphicsPath _gpathro;
        private GraphicsPath _gpath2r;
        private GraphicsPath _gpath2f;
        private GraphicsPath _gpath2fi;
        private GraphicsPath _gpath2ri;
        private GraphicsPath _gpath2fo;
        private GraphicsPath _gpath2ro;
        private GraphicsPath _gpath3f;
        private GraphicsPath _gpath3r;
        private GraphicsPath _gpath4f;
        private GraphicsPath _gpath4r;

        public override void Setup (GraphicsDevice device)
        {
            _thickpen = new Pen(Color.Green, 15);

            _outlinePen = new Pen(Color.GreenYellow, 5);
            _outlinePenInset = new Pen(Color.GreenYellow, 5) { Alignment = PenAlignment.Inset };
            _outlinePenOutset = new Pen(Color.GreenYellow, 5) { Alignment = PenAlignment.Outset };

            List<Vector2> path1 = new List<Vector2>() {
                new Vector2(50, 50), new Vector2(100, 50), new Vector2(100, 100), new Vector2(50, 100),
            };

            _gpathf = new GraphicsPath(_thickpen, _outlinePen, path1, PathType.Closed);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpathr = new GraphicsPath(_thickpen, _outlinePen, path1, PathType.Closed);

            ShiftPath(path1, 100, 0);
            _gpathfi = new GraphicsPath(_thickpen, _outlinePenInset, path1, PathType.Closed);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpathri = new GraphicsPath(_thickpen, _outlinePenInset, path1, PathType.Closed);

            ShiftPath(path1, 100, 0);
            _gpathfo = new GraphicsPath(_thickpen, _outlinePenOutset, path1, PathType.Closed);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpathro = new GraphicsPath(_thickpen, _outlinePenOutset, path1, PathType.Closed);

            ShiftPath(path1, -500, 100);
            _gpath2r = new GraphicsPath(_thickpen, _outlinePen, path1);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpath2f = new GraphicsPath(_thickpen, _outlinePen, path1);

            ShiftPath(path1, 100, 0);
            _gpath2fi = new GraphicsPath(_thickpen, _outlinePenInset, path1);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpath2ri = new GraphicsPath(_thickpen, _outlinePenInset, path1);

            ShiftPath(path1, 100, 0);
            _gpath2fo = new GraphicsPath(_thickpen, _outlinePenOutset, path1);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpath2ro = new GraphicsPath(_thickpen, _outlinePenOutset, path1);

            ShiftPath(path1, -500, 100);
            _gpath3f = new GraphicsPath(_thickpen, _outlinePen, path1, PathType.Closed, StrokeType.Outline);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpath3r = new GraphicsPath(_thickpen, _outlinePen, path1, PathType.Closed, StrokeType.Outline);

            ShiftPath(path1, 100, 0);
            _gpath4f = new GraphicsPath(_thickpen, _outlinePen, path1, StrokeType.Outline);

            path1.Reverse();
            ShiftPath(path1, 100, 0);
            _gpath4r = new GraphicsPath(_thickpen, _outlinePen, path1, StrokeType.Outline);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawPath(_gpathf);
            drawBatch.DrawPath(_gpathr);
            drawBatch.DrawPath(_gpathfi);
            drawBatch.DrawPath(_gpathri);
            drawBatch.DrawPath(_gpathfo);
            drawBatch.DrawPath(_gpathro);
            drawBatch.DrawPath(_gpath2f);
            drawBatch.DrawPath(_gpath2r);
            drawBatch.DrawPath(_gpath2fi);
            drawBatch.DrawPath(_gpath2ri);
            drawBatch.DrawPath(_gpath2fo);
            drawBatch.DrawPath(_gpath2ro);
            drawBatch.DrawPath(_gpath3f);
            drawBatch.DrawPath(_gpath3r);
            drawBatch.DrawPath(_gpath4f);
            drawBatch.DrawPath(_gpath4r);
        }
    }
}
