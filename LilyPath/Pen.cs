using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LilyPath
{
    public enum LineCap
    {
        Flat,
        Square,
    }

    public enum PenAlignment
    {
        Center,
        Inset,
        Outset
    }

    public class PrimitivePen : Pen
    {
        public PrimitivePen (Color color)
            : base(color, 1)
        { }
    }

    public class Pen : IDisposable
    {
        public Color Color { get; set; }
        public Brush Brush { get; set; }
        public float Width { get; set; }

        public PenAlignment Alignment { get; set; }
        public LineCap StartCap { get; set; }
        public LineCap EndCap { get; set; }

        private Pen ()
        {
            Color = Color.White;
            Alignment = PenAlignment.Center;
            StartCap = LineCap.Flat;
            EndCap = LineCap.Flat;
        }

        public Pen (Brush brush, float width)
            : this()
        {
            Brush = brush;
            Width = width;
        }

        public Pen (Color color, float width)
            : this()
        {
            Color = color;
            Width = width;
        }

        public Pen (Brush brush)
            : this(brush, 1)
        {
        }

        public Pen (Color color)
            : this(color, 1)
        {
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose (bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (Brush != null)
                        Brush.Dispose();
                    DisposeManaged();
                }
                DisposeUnmanaged();
                _disposed = true;
            }
        }

        ~Pen ()
        {
            Dispose(false);
        }

        protected virtual void DisposeManaged () { }

        protected virtual void DisposeUnmanaged () { }

        #endregion

        internal int ComputeMiter (Vector2[] outputBuffer, int outputIndex, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            Vector2 edgeBC = new Vector2(c.X - b.X, c.Y - b.Y);
            edgeBC.Normalize();
            Vector2 edgeBCt = new Vector2(-edgeBC.Y, edgeBC.X);

            Vector2 point1, point2, point3, point4;

            switch (Alignment) {
                case PenAlignment.Center:
                    float w2 = Width / 2;

                    point2 = new Vector2(a.X + w2 * edgeABt.X, a.Y + w2 * edgeABt.Y);
                    point4 = new Vector2(a.X - w2 * edgeABt.X, a.Y - w2 * edgeABt.Y);

                    point1 = new Vector2(c.X + w2 * edgeBCt.X, c.Y + w2 * edgeBCt.Y);
                    point3 = new Vector2(c.X - w2 * edgeBCt.X, c.Y - w2 * edgeBCt.Y);
                    break;

                case PenAlignment.Inset:
                    point2 = new Vector2(a.X + Width * edgeABt.X, a.Y + Width * edgeABt.Y);
                    point4 = a;

                    point1 = new Vector2(c.X + Width * edgeBCt.X, c.Y + Width * edgeBCt.Y);
                    point3 = c;
                    break;

                case PenAlignment.Outset:
                    point2 = a;
                    point4 = new Vector2(a.X - Width * edgeABt.X, a.Y - Width * edgeABt.Y);

                    point1 = c;
                    point3 = new Vector2(c.X - Width * edgeBCt.X, c.Y - Width * edgeBCt.Y);
                    break;

                default:
                    point2 = Vector2.Zero;
                    point4 = Vector2.Zero;

                    point1 = Vector2.Zero;
                    point3 = Vector2.Zero;
                    break;
            }

            float offset01 = Vector2.Dot(edgeBCt, point1);
            float t0 = (offset01 - Vector2.Dot(edgeBCt, point2)) / Vector2.Dot(edgeBCt, edgeAB);
            Vector2 point0 = (!float.IsNaN(t0))
                ? new Vector2(point2.X + t0 * edgeAB.X, point2.Y + t0 * edgeAB.Y)
                : new Vector2((point2.X + point1.X) / 2, (point2.Y + point1.Y) / 2);

            float offset35 = Vector2.Dot(edgeBCt, point3);
            float t5 = (offset35 - Vector2.Dot(edgeBCt, point4)) / Vector2.Dot(edgeBCt, edgeAB);
            Vector2 point5 = (!float.IsNaN(t5))
                ? new Vector2(point4.X + t5 * edgeAB.X, point4.Y + t5 * edgeAB.Y)
                : new Vector2((point4.X + point3.X) / 2, (point4.Y + point3.Y) / 2);

            outputBuffer[outputIndex + 0] = point0;
            outputBuffer[outputIndex + 1] = point5;

            return 2;
        }

        internal int ComputeStartPoint (Vector2[] outputBuffer, int outputIndex, Vector2 a, Vector2 b)
        {
            float w2 = Width / 2;

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            switch (StartCap) {
                case LineCap.Flat:
                    break;

                case LineCap.Square:
                    a = new Vector2(a.X - w2 * edgeAB.X, a.Y - w2 * edgeAB.Y);
                    break;
            }

            switch (Alignment) {
                case PenAlignment.Center:
                    outputBuffer[outputIndex + 0] = new Vector2(a.X + w2 * edgeABt.X, a.Y + w2 * edgeABt.Y);
                    outputBuffer[outputIndex + 1] = new Vector2(a.X - w2 * edgeABt.X, a.Y - w2 * edgeABt.Y);
                    return 2;

                case PenAlignment.Inset:
                    outputBuffer[outputIndex + 0] = new Vector2(a.X + Width * edgeABt.X, a.Y + Width * edgeABt.Y);
                    outputBuffer[outputIndex + 1] = a;
                    return 2;

                case PenAlignment.Outset:
                    outputBuffer[outputIndex + 0] = a;
                    outputBuffer[outputIndex + 0] = new Vector2(a.X - Width * edgeABt.X, a.Y - Width * edgeABt.Y);
                    return 2;

                default:
                    return 0;
            }
        }

        internal int ComputeEndPoint (Vector2[] outputBuffer, int outputIndex, Vector2 a, Vector2 b)
        {
            float w2 = Width / 2;

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            switch (StartCap) {
                case LineCap.Flat:
                    break;

                case LineCap.Square:
                    b = new Vector2(b.X + w2 * edgeAB.X, b.Y + w2 * edgeAB.Y);
                    break;
            }

            switch (Alignment) {
                case PenAlignment.Center:
                    outputBuffer[outputIndex + 0] = new Vector2(b.X + w2 * edgeABt.X, b.Y + w2 * edgeABt.Y);
                    outputBuffer[outputIndex + 1] = new Vector2(b.X - w2 * edgeABt.X, b.Y - w2 * edgeABt.Y);
                    return 2;

                case PenAlignment.Inset:
                    outputBuffer[outputIndex + 0] = new Vector2(b.X + Width * edgeABt.X, b.Y + Width * edgeABt.Y);
                    outputBuffer[outputIndex + 1] = b;
                    return 2;

                case PenAlignment.Outset:
                    outputBuffer[outputIndex + 0] = b;
                    outputBuffer[outputIndex + 0] = new Vector2(b.X - Width * edgeABt.X, b.Y - Width * edgeABt.Y);
                    return 2;

                default:
                    return 0;
            }
        }
    }
}
