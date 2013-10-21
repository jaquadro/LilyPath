using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using LilyPath.Utility;

namespace LilyPath
{
    /// <summary>
    /// A <see cref="Pen"/> that can only have a solid color and width of 1.
    /// </summary>
    public class PrimitivePen : Pen
    {
        /// <summary>
        /// Creates a new <see cref="PrimitivePen"/> with the given color.
        /// </summary>
        /// <param name="color">The pen color.</param>
        public PrimitivePen (Color color)
            : base(color, 1)
        { }
    }

    internal struct InsetOutsetCount
    {
        public readonly short InsetCount;
        public readonly short OutsetCount;
        public readonly bool CCW;

        public InsetOutsetCount (short insetCount, short outsetCount)
        {
            InsetCount = insetCount;
            OutsetCount = outsetCount;
            CCW = true;
        }

        public InsetOutsetCount (short insetCount, short outsetCount, bool ccw)
        {
            InsetCount = insetCount;
            OutsetCount = outsetCount;
            CCW = ccw;
        }
    }

    /// <summary>
    /// Objects used to draw paths.
    /// </summary>
    public class Pen : IDisposable
    {
        #region Default Pens

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Black { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Blue { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Cyan { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Green { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Magenta { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Red { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen White { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Yellow { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen LightGray { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Gray { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen DarkGray { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen LightBlue { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen LightCyan { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen LightGreen { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen LightYellow { get; private set; }

        static Pen ()
        {
            Black = new Pen(Brush.Black);
            Blue = new Pen(Brush.Blue);
            Cyan = new Pen(Brush.Cyan);
            Green = new Pen(Brush.Green);
            Magenta = new Pen(Brush.Magenta);
            Red = new Pen(Brush.Red);
            White = new Pen(Brush.White);
            Yellow = new Pen(Brush.Yellow);

            LightBlue = new Pen(Brush.LightBlue);
            LightCyan = new Pen(Brush.LightCyan);
            LightGreen = new Pen(Brush.LightGreen);
            LightYellow = new Pen(Brush.LightYellow);

            LightGray = new Pen(Brush.LightGray);
            Gray = new Pen(Brush.Gray);
            DarkGray = new Pen(Brush.DarkGray);
        }

        #endregion

        private float _joinLimit;
        private float _joinLimitCos2;

        /// <summary>
        /// Gets the solid color or blending color of the pen.
        /// </summary>
        public Color Color
        {
            get { return Brush.Color; }
        }

        /// <summary>
        /// Gets the <see cref="Brush"/> used to fill stroked paths.
        /// </summary>
        public Brush Brush { get; private set; }

        /// <summary>
        /// Gets or sets the width of the stroked path in graphical units (usually pixels).
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the stroked path relative to the ideal path being stroked.
        /// </summary>
        public PenAlignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets how the start of a stroked path is terminated.
        /// </summary>
        public LineCap StartCap { get; set; }

        /// <summary>
        /// Gets or sets how the end of a stroked path is terminated.
        /// </summary>
        public LineCap EndCap { get; set; }

        /// <summary>
        /// Gets or sets how the segments in the path are joined together.
        /// </summary>
        public LineJoin LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the limit of the thickness of the join on a mitered corner.
        /// </summary>
        /// <remarks><para>The miter length is the distance from the intersection of the line walls on the inside of the join to the intersection of the line walls outside of the join. The miter length can be large when the angle between two lines is small. The miter limit is the maximum allowed ratio of miter length to stroke width. The default value is 10.0f.</para>
        /// <para>If the miter length of the join of the intersection exceeds the limit of the join, then the join will be beveled to keep it within the limit of the join of the intersection.</para></remarks>
        public float MiterLimit { get; set; }

        /// <summary>
        /// Gets or sets the angle difference threshold in radians under which joins will be mitered instead of beveled or rounded.  
        /// Defaults to PI / 8 (11.25 degrees).
        /// </summary>
        public float JoinLimit
        {
            get { return _joinLimit; }
            set
            {
                _joinLimit = value;
                _joinLimitCos2 = (float)Math.Cos(_joinLimit);
                _joinLimitCos2 *= _joinLimitCos2;
            }
        }

        /// <summary>
        /// Gets or sets whether this pen "owns" the brush used to construct it, and should therefor dispose the brush
        /// along with itself.
        /// </summary>
        public bool OwnsBrush { get; set; }

        private Pen ()
        {
            //Color = Color.White;
            Alignment = PenAlignment.Center;
            StartCap = LineCap.Flat;
            EndCap = LineCap.Flat;
            MiterLimit = 10f;
            JoinLimit = (float)(Math.PI / 8);
        }

        /// <summary>
        /// Creates a new <see cref="Pen"/> with the given brush and width.
        /// </summary>
        /// <param name="brush">The <see cref="Brush"/> used to stroke the pen.</param>
        /// <param name="width">The width of the paths drawn by the pen.</param>
        /// <param name="ownsBrush"><c>true</c> if the pen should be responsible for disposing the <see cref="Brush"/>, <c>false</c> otherwise.</param>
        public Pen (Brush brush, float width, bool ownsBrush)
            : this()
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            Brush = brush;
            Width = width;
            OwnsBrush = ownsBrush;
        }

        /// <summary>
        /// Creates a new <see cref="Pen"/> with the given brush and width.
        /// </summary>
        /// <param name="brush">The <see cref="Brush"/> used to stroke the pen.</param>
        /// <param name="width">The width of the paths drawn by the pen.</param>
        /// <remarks>By default, the pen will not take resposibility for disposing the <see cref="Brush"/>.</remarks>
        public Pen (Brush brush, float width)
            : this(brush, width, false)
        { }

        /// <summary>
        /// Creates a new <see cref="Pen"/> with the given color and width.
        /// </summary>
        /// <param name="color">The color used to stroke the pen.</param>
        /// <param name="width">The width of the paths drawn by the pen.</param>
        public Pen (Color color, float width)
            : this(new SolidColorBrush(color), width, true)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Pen"/> with the given brush and a width of 1.
        /// </summary>
        /// <param name="brush">The <see cref="Brush"/> used to stroke the pen.</param>
        /// <remarks>By default, the pen will not take resposibility for disposing the <see cref="Brush"/>.</remarks>
        public Pen (Brush brush)
            : this(brush, 1, false)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Pen"/> with the given brush and a width of 1.
        /// </summary>
        /// <param name="brush">The <see cref="Brush"/> used to stroke the pen.</param>
        /// <param name="ownsBrush"><c>true</c> if the pen should be responsible for disposing the <see cref="Brush"/>, <c>false</c> otherwise.</param>
        public Pen (Brush brush, bool ownsBrush)
            : this(brush, 1, ownsBrush)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Pen"/> with the given color and a width of 1.
        /// </summary>
        /// <param name="color">The color used to stroke the pen.</param>
        public Pen (Color color)
            : this(color, 1)
        {
        }

        #region IDisposable

        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="Pen"/> object.
        /// </summary>
        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose (bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (OwnsBrush && Brush != null)
                        Brush.Dispose();
                    DisposeManaged();
                }
                DisposeUnmanaged();
                _disposed = true;
            }
        }

        /// <summary>
        /// Attempts to dispose unmanaged resources.
        /// </summary>
        ~Pen ()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the managed resources used by the <see cref="Pen"/>.
        /// </summary>
        protected virtual void DisposeManaged () { }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Pen"/>.
        /// </summary>
        protected virtual void DisposeUnmanaged () { }

        #endregion

        /// <summary>
        /// Queries the <see cref="Pen"/> for its color at a coordinate relative to the stroke width of the pen and length of the path.
        /// </summary>
        /// <param name="widthPosition">A value between 0 and 1 interpolated across the stroke width.</param>
        /// <param name="lengthPosition">A value between 0 and 1 interpolated between the start and end points of a path.</param>
        /// <returns>A color value.</returns>
        protected internal virtual Color ColorAt (float widthPosition, float lengthPosition)
        {
            return Brush.Color;
        }

        internal Color ColorAt (Vector2 uv)
        {
            return ColorAt(uv.X, uv.Y);
        }

        internal int StartPointVertexBound ()
        {
            switch (StartCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    return 2;
            }

            return 0;
        }

        internal int StartPointVertexBound (Vector2 a, Vector2 b)
        {
            switch (StartCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    return 2;
            }

            return 0;
        }

        internal int EndPointVertexBound ()
        {
            switch (StartCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    return 2;
            }

            return 0;
        }

        internal int EndPointVertexBound (Vector2 a, Vector2 b)
        {
            switch (EndCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    return 2;
            }

            return 0;
        }

        internal int LineJoinVertexBound ()
        {
            switch (LineJoin) {
                case LineJoin.Miter:
                    return 3;
                case LineJoin.Bevel:
                    return 3;
            }

            return 0;
        }

        internal int LineJoinVertexBound (Vector2 a, Vector2 b, Vector2 c)
        {
            switch (LineJoin) {
                case LineJoin.Miter:
                    return 2;
                case LineJoin.Bevel:
                    return 3;
            }

            return 0;
        }

        internal int MaximumVertexCount (int pointCount)
        {
            int expected = 0;

            int joinCount = Math.Max(0, pointCount - 1);
            switch (LineJoin) {
                case LineJoin.Bevel:
                    expected += joinCount * 3;
                    break;

                case LineJoin.Miter:
                    expected += joinCount * 3;
                    break;

                //case LineJoin.Round:
                //    expected += (int)Math.Ceiling(joinCount * (Width / 6f + 2));
                //    break;
            }

            switch (StartCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    expected += 2;
                    break;
            }

            switch (EndCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    expected += 2;
                    break;
            }

            return expected;
        }

        internal int MaximumIndexCount (int pointCount)
        {
            int extra = 0;

            int joinCount = Math.Max(0, pointCount - 1);
            switch (LineJoin) {
                case LineJoin.Bevel:
                    extra += joinCount;
                    break;

                case LineJoin.Miter:
                    extra += joinCount;
                    break;

                //case LineJoin.Round:
                //    extra += (int)Math.Ceiling(joinCount * (Width / 6f));
                //    break;
            }

            switch (StartCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    break;
            }

            switch (EndCap) {
                case LineCap.Flat:
                case LineCap.Square:
                    break;
            }

            return extra * 3 + (pointCount - 1) * 6;
        }

        internal InsetOutsetCount ComputeMiter (Vector2 a, Vector2 b, Vector2 c, PenWorkspace ws)
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

            Vector2 point0, point5;

            float tdiv = Vector2.Dot(edgeBCt, edgeAB);

            if (Math.Abs(tdiv) < .0005f) {
                point0 = new Vector2((point2.X + point1.X) / 2, (point2.Y + point1.Y) / 2);
                point5 = new Vector2((point4.X + point3.X) / 2, (point4.Y + point3.Y) / 2);
            }
            else {
                float offset01 = Vector2.Dot(edgeBCt, point1);
                float t0 = (offset01 - Vector2.Dot(edgeBCt, point2)) / tdiv;

                float offset35 = Vector2.Dot(edgeBCt, point3);
                float t5 = (offset35 - Vector2.Dot(edgeBCt, point4)) / tdiv;

                point0 = new Vector2(point2.X + t0 * edgeAB.X, point2.Y + t0 * edgeAB.Y);
                point5 = new Vector2(point4.X + t5 * edgeAB.X, point4.Y + t5 * edgeAB.Y);
            }

            double miterLimit = MiterLimit * Width;
            if ((point0 - point5).LengthSquared() > miterLimit * miterLimit)
                return ComputeBevel(a, b, c, ws);

            ws.XYInsetBuffer[0] = point0;
            ws.XYOutsetBuffer[0] = point5;

            ws.UVInsetBuffer[0] = new Vector2(0, 0);
            ws.UVOutsetBuffer[0] = new Vector2(1, 0);

            return new InsetOutsetCount(1, 1);
        }

        internal InsetOutsetCount ComputeBevel (Vector2 a, Vector2 b, Vector2 c, PenWorkspace ws)
        {
            Vector2 edgeBA = new Vector2(a.X - b.X, a.Y - b.Y);
            Vector2 edgeBC = new Vector2(c.X - b.X, c.Y - b.Y);
            double dot = Vector2.Dot(edgeBA, edgeBC);
            if (dot < 0) {
                double den = edgeBA.LengthSquared() * edgeBC.LengthSquared();
                double cos2 = (dot * dot) / den;

                if (cos2 > _joinLimitCos2)
                    return ComputeMiter(a, b, c, ws);
            }

            Vector2 edgeAB = new Vector2(b.X - a.X, b.Y - a.Y);
            edgeAB.Normalize();
            Vector2 edgeABt = new Vector2(-edgeAB.Y, edgeAB.X);

            edgeBC.Normalize();
            Vector2 edgeBCt = new Vector2(-edgeBC.Y, edgeBC.X);

            Vector2 pointA = a;
            Vector2 pointC = c;

            short vertexCount = 0;

            if (Cross2D(edgeAB, edgeBC) > 0) {
                switch (Alignment) {
                    case PenAlignment.Center:
                        float w2 = Width / 2;
                        pointA = new Vector2(a.X - w2 * edgeABt.X, a.Y - w2 * edgeABt.Y);
                        pointC = new Vector2(c.X - w2 * edgeBCt.X, c.Y - w2 * edgeBCt.Y);

                        ws.XYInsetBuffer[0] = new Vector2(b.X + w2 * edgeABt.X, b.Y + w2 * edgeABt.Y);
                        ws.XYInsetBuffer[1] = new Vector2(b.X + w2 * edgeBCt.X, b.Y + w2 * edgeBCt.Y);

                        vertexCount = 2;
                        break;

                    case PenAlignment.Inset:
                        ws.XYInsetBuffer[0] = new Vector2(b.X + Width * edgeABt.X, b.Y + Width * edgeABt.Y);
                        ws.XYInsetBuffer[1] = new Vector2(b.X + Width * edgeBCt.X, b.Y + Width * edgeBCt.Y);

                        vertexCount = 2;
                        break;

                    case PenAlignment.Outset:
                        pointA = new Vector2(a.X - Width * edgeABt.X, a.Y - Width * edgeABt.Y);
                        pointC = new Vector2(c.X - Width * edgeBCt.X, c.Y - Width * edgeBCt.Y);

                        ws.XYInsetBuffer[0] = b;

                        vertexCount = 1;
                        break;
                }

                Vector2 point5;

                float tdiv = Vector2.Dot(edgeBCt, edgeAB);
                if (Math.Abs(tdiv) < 0.0005f) {
                    point5 = new Vector2((pointA.X + pointC.X) / 2, (pointA.Y + pointC.Y) / 2);
                }
                else {
                    float offset35 = Vector2.Dot(edgeBCt, pointC);
                    float t5 = (offset35 - Vector2.Dot(edgeBCt, pointA)) / tdiv;

                    point5 = new Vector2(pointA.X + t5 * edgeAB.X, pointA.Y + t5 * edgeAB.Y);
                }

                ws.XYOutsetBuffer[0] = point5;

                ws.UVOutsetBuffer[0] = new Vector2(1, 0);
                for (int i = 0; i < vertexCount; i++)
                    ws.UVInsetBuffer[i] = new Vector2(0, 0);

                return new InsetOutsetCount(vertexCount, 1, false);
            }
            else {
                switch (Alignment) {
                    case PenAlignment.Center:
                        float w2 = Width / 2;
                        pointA = new Vector2(a.X + w2 * edgeABt.X, a.Y + w2 * edgeABt.Y);
                        pointC = new Vector2(c.X + w2 * edgeBCt.X, c.Y + w2 * edgeBCt.Y);

                        ws.XYOutsetBuffer[0] = new Vector2(b.X - w2 * edgeABt.X, b.Y - w2 * edgeABt.Y);
                        ws.XYOutsetBuffer[1] = new Vector2(b.X - w2 * edgeBCt.X, b.Y - w2 * edgeBCt.Y);

                        vertexCount = 2;
                        break;

                    case PenAlignment.Inset:
                        pointA = new Vector2(a.X + Width * edgeABt.X, a.Y + Width * edgeABt.Y);
                        pointC = new Vector2(c.X + Width * edgeBCt.X, c.Y + Width * edgeBCt.Y);

                        ws.XYOutsetBuffer[0] = b;

                        vertexCount = 1;
                        break;

                    case PenAlignment.Outset:
                        ws.XYOutsetBuffer[0] = new Vector2(b.X - Width * edgeABt.X, b.Y - Width * edgeABt.Y);
                        ws.XYOutsetBuffer[1] = new Vector2(b.X - Width * edgeBCt.X, b.Y - Width * edgeBCt.Y);

                        vertexCount = 2;
                        break;
                }

                Vector2 point0;

                float tdiv = Vector2.Dot(edgeBCt, edgeAB);
                if (Math.Abs(tdiv) < 0.0005f) {
                    point0 = new Vector2((pointA.X + pointC.X) / 2, (pointA.Y + pointC.Y) / 2);
                }
                else {
                    float offset01 = Vector2.Dot(edgeBCt, pointC);
                    float t0 = (offset01 - Vector2.Dot(edgeBCt, pointA)) / tdiv;

                    point0 = new Vector2(pointA.X + t0 * edgeAB.X, pointA.Y + t0 * edgeAB.Y);
                }

                ws.XYInsetBuffer[0] = point0;

                ws.UVInsetBuffer[0] = new Vector2(0, 0);
                for (int i = 0; i < vertexCount; i++)
                    ws.UVOutsetBuffer[i] = new Vector2(1, 0);

                return new InsetOutsetCount(1, vertexCount, true);
            }
        }

        private float Cross2D (Vector2 u, Vector2 v)
        {
            return (u.Y * v.X) - (u.X * v.Y);
        }

        private bool TriangleIsCCW (Vector2 a, Vector2 b, Vector2 c)
        {
            return Cross2D(b - a, c - b) < 0;
        }

        internal int ComputeStartPoint (Vector2 a, Vector2 b, PenWorkspace ws)
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

            ws.UVBuffer[0] = new Vector2(0, 0);
            ws.UVBuffer[1] = new Vector2(1, 0);

            switch (Alignment) {
                case PenAlignment.Center:
                    ws.XYBuffer[0] = new Vector2(a.X + w2 * edgeABt.X, a.Y + w2 * edgeABt.Y);
                    ws.XYBuffer[1] = new Vector2(a.X - w2 * edgeABt.X, a.Y - w2 * edgeABt.Y);
                    return 2;

                case PenAlignment.Inset:
                    ws.XYBuffer[0] = new Vector2(a.X + Width * edgeABt.X, a.Y + Width * edgeABt.Y);
                    ws.XYBuffer[1] = a;
                    return 2;

                case PenAlignment.Outset:
                    ws.XYBuffer[0] = a;
                    ws.XYBuffer[1] = new Vector2(a.X - Width * edgeABt.X, a.Y - Width * edgeABt.Y);
                    return 2;

                default:
                    return 0;
            }
        }

        internal int ComputeEndPoint (Vector2 a, Vector2 b, PenWorkspace ws)
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

            ws.UVBuffer[0] = new Vector2(0, 0);
            ws.UVBuffer[1] = new Vector2(1, 0);

            switch (Alignment) {
                case PenAlignment.Center:
                    ws.XYBuffer[0] = new Vector2(b.X + w2 * edgeABt.X, b.Y + w2 * edgeABt.Y);
                    ws.XYBuffer[1] = new Vector2(b.X - w2 * edgeABt.X, b.Y - w2 * edgeABt.Y);
                    return 2;

                case PenAlignment.Inset:
                    ws.XYBuffer[0] = new Vector2(b.X + Width * edgeABt.X, b.Y + Width * edgeABt.Y);
                    ws.XYBuffer[1] = b;
                    return 2;

                case PenAlignment.Outset:
                    ws.XYBuffer[0] = b;
                    ws.XYBuffer[1] = new Vector2(b.X - Width * edgeABt.X, b.Y - Width * edgeABt.Y);
                    return 2;

                default:
                    return 0;
            }
        }
    }
}
