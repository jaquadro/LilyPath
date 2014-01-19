using System;
using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Line Caps")]
    public class LineCapsSheet : TestSheet
    {
        private Pen _flatPen;
        private Pen _squarePen;
        private Pen _trianglePen;
        private Pen _invTrianglePen;
        private Pen _arrowPen;

        public override void Setup (GraphicsDevice device)
        {
            _flatPen = new Pen(Color.Blue, 15) { StartCap = LineCap.Flat, EndCap = LineCap.Flat };
            _squarePen = new Pen(Color.Red, 15) { StartCap = LineCap.Square, EndCap = LineCap.Square };
            _trianglePen = new Pen(Color.Green, 15) { StartCap = LineCap.Triangle, EndCap = LineCap.Triangle };
            _invTrianglePen = new Pen(Color.Purple, 15) { StartCap = LineCap.InvTriangle, EndCap = LineCap.InvTriangle };
            _arrowPen = new Pen(Color.Orange, 15) { StartCap = LineCap.Arrow, EndCap = LineCap.Arrow };
        }

        public override void Draw (DrawBatch drawBatch)
        {
            float space = 30;
            float macroSpace = 50;
            float length = 200;

            Vector2 o1 = new Vector2(macroSpace, macroSpace);
            Vector2 o2 = new Vector2(macroSpace * 2 + length, macroSpace);
            Vector2 o3 = new Vector2(macroSpace * 2 + length, macroSpace * 2 + length);
            Vector2 o4 = new Vector2(macroSpace, macroSpace * 2 + length);

            drawBatch.DrawPath(new GraphicsPath(_flatPen, new List<Vector2> { o1 + new Vector2(0, space * 0), o1 + new Vector2(length, space * 0) }));
            drawBatch.DrawPath(new GraphicsPath(_squarePen, new List<Vector2> { o1 + new Vector2(0, space * 1), o1 + new Vector2(length, space * 1) }));
            drawBatch.DrawPath(new GraphicsPath(_trianglePen, new List<Vector2> { o1 + new Vector2(0, space * 2), o1 + new Vector2(length, space * 2) }));
            drawBatch.DrawPath(new GraphicsPath(_invTrianglePen, new List<Vector2> { o1 + new Vector2(0, space * 3), o1 + new Vector2(length, space * 3) }));
            drawBatch.DrawPath(new GraphicsPath(_arrowPen, new List<Vector2> { o1 + new Vector2(0, space * 4), o1 + new Vector2(length, space * 4) }));

            drawBatch.DrawPath(new GraphicsPath(_flatPen, new List<Vector2> { o2 + new Vector2(space * 0, 0), o2 + new Vector2(space * 0, length) }));
            drawBatch.DrawPath(new GraphicsPath(_squarePen, new List<Vector2> { o2 + new Vector2(space * 1, 0), o2 + new Vector2(space * 1, length) }));
            drawBatch.DrawPath(new GraphicsPath(_trianglePen, new List<Vector2> { o2 + new Vector2(space * 2, 0), o2 + new Vector2(space * 2, length) }));
            drawBatch.DrawPath(new GraphicsPath(_invTrianglePen, new List<Vector2> { o2 + new Vector2(space * 3, 0), o2 + new Vector2(space * 3, length) }));
            drawBatch.DrawPath(new GraphicsPath(_arrowPen, new List<Vector2> { o2 + new Vector2(space * 4, 0), o2 + new Vector2(space * 4, length) }));

            drawBatch.DrawPath(new GraphicsPath(_flatPen, new List<Vector2> { o3 + new Vector2(length, space * 0), o3 + new Vector2(0, space * 0) }));
            drawBatch.DrawPath(new GraphicsPath(_squarePen, new List<Vector2> { o3 + new Vector2(length, space * 1), o3 + new Vector2(0, space * 1) }));
            drawBatch.DrawPath(new GraphicsPath(_trianglePen, new List<Vector2> { o3 + new Vector2(length, space * 2), o3 + new Vector2(0, space * 2 ) }));
            drawBatch.DrawPath(new GraphicsPath(_invTrianglePen, new List<Vector2> { o3 + new Vector2(length, space * 3), o3 + new Vector2(0, space * 3) }));
            drawBatch.DrawPath(new GraphicsPath(_arrowPen, new List<Vector2> { o3 + new Vector2(length, space * 4), o3 + new Vector2(0, space * 4) }));

            drawBatch.DrawPath(new GraphicsPath(_flatPen, new List<Vector2> { o4 + new Vector2(space * 0, length), o4 + new Vector2(space * 0, 0) }));
            drawBatch.DrawPath(new GraphicsPath(_squarePen, new List<Vector2> { o4 + new Vector2(space * 1, length), o4 + new Vector2(space * 1, 0) }));
            drawBatch.DrawPath(new GraphicsPath(_trianglePen, new List<Vector2> { o4 + new Vector2(space * 2, length), o4 + new Vector2(space * 2, 0) }));
            drawBatch.DrawPath(new GraphicsPath(_invTrianglePen, new List<Vector2> { o4 + new Vector2(space * 3, length), o4 + new Vector2(space * 3, 0) }));
            drawBatch.DrawPath(new GraphicsPath(_arrowPen, new List<Vector2> { o4 + new Vector2(space * 4, length), o4 + new Vector2(space * 4, 0) }));
        }
    }
}
