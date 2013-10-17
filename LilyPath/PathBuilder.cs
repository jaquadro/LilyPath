using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LilyPath
{
    /// <summary>
    /// An object for building up an ideal path from pieces.
    /// </summary>
    public class PathBuilder
    {
        private static Dictionary<int, List<Vector2>> _circleCache = new Dictionary<int, List<Vector2>>();

        private static readonly double[] _factorials = new double[] {
            1.0,
            1.0,
            2.0,
            6.0,
            24.0,
            120.0,
            720.0,
            5040.0,
            40320.0,
            362880.0,
            3628800.0,
            39916800.0,
            479001600.0,
            6227020800.0,
            87178291200.0,
            1307674368000.0,
            20922789888000.0,
            355687428096000.0,
            6402373705728000.0,
            121645100408832000.0,
            2432902008176640000.0,
            51090942171709440000.0,
            1124000727777607680000.0,
            25852016738884976640000.0,
            620448401733239439360000.0,
            15511210043330985984000000.0,
            403291461126605635584000000.0,
            10888869450418352160768000000.0,
            304888344611713860501504000000.0,
            8841761993739701954543616000000.0,
            265252859812191058636308480000000.0,
            8222838654177922817725562880000000.0,
            263130836933693530167218012160000000.0,
        };

        private Vector2[] _geometryBuffer;
        private int _geometryIndex;

        /// <summary>
        /// Creates a new <see cref="PathBuilder"/> object.
        /// </summary>
        public PathBuilder ()
            : this(256)
        { }

        /// <summary>
        /// Creates a new <see cref="PathBuilder"/> object with a given initial buffer size.
        /// </summary>
        /// <param name="initialBufferSize">The initial size of the internal vertex buffer.</param>
        public PathBuilder (int initialBufferSize)
        {
            _geometryBuffer = new Vector2[initialBufferSize];
            _geometryIndex = 0;
        }

        /// <summary>
        /// Gets the raw vertex buffer from the <see cref="PathBuilder"/>.
        /// </summary>
        public Vector2[] Buffer
        {
            get { return _geometryBuffer; }
        }

        /// <summary>
        /// Gets the number of vertices currently in the path and buffer.
        /// </summary>
        public int Count
        {
            get { return _geometryIndex; }
        }

        /// <summary>
        /// Appends a point to the end of the path.
        /// </summary>
        /// <param name="point">A point.</param>
        public void AddPoint (Vector2 point)
        {
            CheckBufferFreeSpace(1);

            if (!LastPointEqual(point))
                _geometryBuffer[_geometryIndex++] = point;
        }

        /// <summary>
        /// Appends a list of points to the end of the path.
        /// </summary>
        /// <param name="points">A list of points.</param>
        public void AddPath (IList<Vector2> points)
        {
            Vector2 lastPoint = (_geometryIndex > 0) ? _geometryBuffer[_geometryIndex - 1] : new Vector2(float.NaN, float.NaN);

            CheckBufferFreeSpace(points.Count);

            foreach (Vector2 point in points) {
                if (point != lastPoint)
                    _geometryBuffer[_geometryIndex++] = point;
                lastPoint = point;
            }
        }

        /// <summary>
        /// Appends all of the points within another <see cref="PathBuilder"/> object to the end of the path.
        /// </summary>
        /// <param name="path">An existing path.</param>
        public void AddPath (PathBuilder path)
        {
            if (path._geometryIndex == 0)
                return;

            if (path._geometryIndex == 1) {
                AddPoint(path._geometryBuffer[0]);
                return;
            }

            CheckBufferFreeSpace(path._geometryIndex);

            int startIndex = LastPointEqual(path._geometryBuffer[0]) ? 1 : 0;
            for (int i = startIndex; i < _geometryIndex; i++)
                _geometryBuffer[_geometryIndex++] = path._geometryBuffer[i];
        }

        /// <summary>
        /// Appends a point to the end of the path offset from the path's current endpoint by the given length and angle.
        /// </summary>
        /// <param name="length">The length of the line being added.</param>
        /// <param name="angle">The angle of the line in radians.  Positive values are clockwise.</param>
        /// <exception cref="InvalidOperationException">The path has no existing points.</exception>
        public void AddLine (float length, float angle)
        {
            if (_geometryIndex == 0)
                throw new InvalidOperationException("Cannot add a line from partial information to an empty path.");

            if (length == 0)
                return;

            Vector2 start = _geometryBuffer[_geometryIndex - 1];
            Vector2 end = new Vector2(start.X + length * (float)Math.Cos(angle), start.Y + length * (float)Math.Sin(angle));

            _geometryBuffer[_geometryIndex++] = end;
        }

        /// <summary>
        /// Appends an arc between the current endpoint and given point to the end of the path.
        /// </summary>
        /// <param name="point">The endpoint of the arc.</param>
        /// <param name="height">The furthest point on the arc from the line connecting the path's current endpoint and <paramref name="point"/>.</param>
        /// <exception cref="InvalidOperationException">The path has no existing points.</exception>
        public void AddArcByPoint (Vector2 point, float height)
        {
            if (_geometryIndex == 0)
                throw new InvalidOperationException("Cannot add an arc from partial information to an empty path.");

            float width = (point - _geometryBuffer[_geometryIndex - 1]).Length();
            float radius = (height / 2) + (width * width) / (height * 8);
            AddArcByPoint(point, height, DefaultSubdivisions(radius));
        }

        /// <summary>
        /// Appends an arc between the current endpoint and given point to the end of the path.
        /// </summary>
        /// <param name="point">The endpoint of the arc.</param>
        /// <param name="height">The furthest point on the arc from the line connecting the path's current endpoint and <paramref name="point"/>.</param>
        /// <param name="subdivisions">The number of subdivisions in a circle of the same arc radius.</param>
        /// <exception cref="InvalidOperationException">The path has no existing points.</exception>
        public void AddArcByPoint (Vector2 point, float height, int subdivisions)
        {
            if (_geometryIndex == 0)
                throw new InvalidOperationException("Cannot add an arc from partial information to an empty path.");

            if (_geometryBuffer[_geometryIndex - 1] == point)
                return;

            _geometryIndex--;

            BuildArcGeometryBuffer(_geometryBuffer[_geometryIndex], point, height, subdivisions);
        }

        /// <summary>
        /// Appends an arc between the current endpoint and a point defined by a center and arc angle.
        /// </summary>
        /// <param name="center">The center of a circle containing the path's current endpoint and destination point.</param>
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw clockwise.</param>
        /// <exception cref="InvalidOperationException">The path has no existing points.</exception>
        public void AddArcByAngle (Vector2 center, float arcAngle)
        {
            if (_geometryIndex == 0)
                throw new InvalidOperationException("Cannot add an arc from partial information to an empty path.");

            float radius = Math.Abs((_geometryBuffer[_geometryIndex - 1] - center).Length());
            AddArcByAngle(center, arcAngle, DefaultSubdivisions(radius));
        }

        /// <summary>
        /// Appends an arc between the current endpoint and a point defined by a center and arc angle.
        /// </summary>
        /// <param name="center">The center of a circle containing the path's current endpoint and destination point.</param>
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw clockwise.</param>
        /// <param name="subdivisions">The number of subdivisions in a circle of the same arc radius.</param>
        /// <exception cref="InvalidOperationException">The path has no existing points.</exception>
        public void AddArcByAngle (Vector2 center, float arcAngle, int subdivisions)
        {
            if (_geometryIndex == 0)
                throw new InvalidOperationException("Cannot add an arc from partial information to an empty path.");

            _geometryIndex--;

            float radius = Math.Abs((_geometryBuffer[_geometryIndex] - center).Length());
            float startAngle = PointToAngle(center, _geometryBuffer[_geometryIndex]);

            BuildArcGeometryBuffer(center, radius, subdivisions, startAngle, arcAngle);
        }

        /// <summary>
        /// Appends a fully-defined arc to the end of the path, connected by an additional line segment if the arc does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="center">The center coordinate of the the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="startAngle">The starting angle of the arc in radians, where 0 is 3 O'Clock.</param>
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw clockwise.</param>
        public void AddArc (Vector2 center, float radius, float startAngle, float arcAngle)
        {
            AddArc(center, radius, startAngle, arcAngle, DefaultSubdivisions(radius));
        }

        /// <summary>
        /// Appends a fully-defined arc to the end of the path, connected by an additional line segment if the arc does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="center">The center coordinate of the the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="startAngle">The starting angle of the arc in radians, where 0 is 3 O'Clock.</param>
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw clockwise.</param>
        /// <param name="subdivisions">The number of subdivisions in a circle of the same arc radius.</param>
        public void AddArc (Vector2 center, float radius, float startAngle, float arcAngle, int subdivisions)
        {
            Vector2 startPoint = new Vector2(center.X + radius * (float)Math.Cos(startAngle), center.Y + radius * (float)Math.Sin(startAngle));

            if (LastPointEqual(startPoint))
                _geometryIndex--;

            BuildArcGeometryBuffer(center, radius, subdivisions, startAngle, arcAngle);
        }

        /// <summary>
        /// Appends a fully-defined arc to the end of the path, connected by an additional line segment if the arc does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="p0">The starting point of the arc.</param>
        /// <param name="p1">The ending point of the arc.</param>
        /// <param name="height">The furthest point on the arc from the line connecting <paramref name="p0"/> and <paramref name="p1"/>.</param>
        public void AddArc (Vector2 p0, Vector2 p1, float height)
        {
            float width = (p1 - p0).Length();
            float radius = (height / 2) + (width * width) / (height * 8);
            AddArc(p0, p1, height, DefaultSubdivisions(radius));
        }

        /// <summary>
        /// Appends a fully-defined arc to the end of the path, connected by an additional line segment if the arc does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="p0">The starting point of the arc.</param>
        /// <param name="p1">The ending point of the arc.</param>
        /// <param name="height">The furthest point on the arc from the line connecting <paramref name="p0"/> and <paramref name="p1"/>.</param>
        /// <param name="subdivisions">The number of subdivisions in a circle of the same arc radius.</param>
        public void AddArc (Vector2 p0, Vector2 p1, float height, int subdivisions)
        {
            if (p0 == p1)
                return;

            if (LastPointEqual(p0))
                _geometryIndex--;

            BuildArcGeometryBuffer(p0, p1, height, subdivisions);
        }

        /// <summary>
        /// Appends a quadratic bezier curve to the end of the path, connected by an additional line segment if the curve does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="p0">The starting point of the curve.</param>
        /// <param name="p1">The first control point of the curve.</param>
        /// <param name="p2">The ending point of the curve.</param>
        public void AddBezier (Vector2 p0, Vector2 p1, Vector2 p2)
        {
            AddBezier(p0, p1, p2, DefaultBezierSubdivisions(p0, p1, p2));
        }

        /// <summary>
        /// Appends a quadratic Bezier curve to the end of the path, connected by an additional line segment if the curve does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="p0">The starting point of the curve.</param>
        /// <param name="p1">The first control point of the curve.</param>
        /// <param name="p2">The ending point of the curve.</param>
        /// <param name="subdivisions">The number of subdivisions in the curve.</param>
        public void AddBezier (Vector2 p0, Vector2 p1, Vector2 p2, int subdivisions)
        {
            if (LastPointClose(p0))
                _geometryIndex--;

            BuildQuadraticBezierGeometryBuffer(p0, p1, p2, subdivisions);
        }

        /// <summary>
        /// Appends a cubic Bezier curve to the end of the path, connected by an additional line segment if the curve does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="p0">The starting point of the curve.</param>
        /// <param name="p1">The first control point.</param>
        /// <param name="p2">The second control point.</param>
        /// <param name="p3">The ending point of the curve.</param>
        public void AddBezier (Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            AddBezier(p0, p1, p2, p3, DefaultBezierSubdivisions(p0, p1, p2, p3));
        }

        /// <summary>
        /// Appends a cubic Bezier curve to the end of the path, connected by an additional line segment if the curve does not
        /// begin at the path's current endpoint.
        /// </summary>
        /// <param name="p0">The starting point of the curve.</param>
        /// <param name="p1">The first control point.</param>
        /// <param name="p2">The second control point.</param>
        /// <param name="p3">The ending point of the curve.</param>
        /// <param name="subdivisions">The number of subdivisions in the curve.</param>
        public void AddBezier (Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int subdivisions)
        {
            if (LastPointClose(p0))
                _geometryIndex--;

            BuildCubicBezierGeometryBuffer(p0, p1, p2, p3, subdivisions);
        }

        /// <summary>
        /// Appends a series of Bezier curves to the end of the path, connected by an additional line segment if the first curve
        /// does not begin at the path's current endpoint.
        /// </summary>
        /// <param name="points">A list of points.</param>
        /// <param name="bezierType">The type of Bezier</param>
        /// <remarks><para>For quadratic Bezier curves, the number of points defined by the parameters should be a multiple of 2 plus 1.
        /// For cubic Bezier curves, the number of points defined by the parameters should be a multiple of 3 plus 1.  For each curve
        /// drawn after the first, the ending point of the previous curve is used as the starting point.</para></remarks>
        public void AddBeziers (IList<Vector2> points, BezierType bezierType)
        {
            AddBeziers(points, 0, points.Count, bezierType);
        }

        /// <summary>
        /// Appends a series of Bezier curves to the end of the path, connected by an additional line segment if the first curve
        /// does not begin at the path's current endpoint.
        /// </summary>
        /// <param name="points">A list of points.</param>
        /// <param name="offset">The index of the first point to use from the list.</param>
        /// <param name="length">The number of points to use from the list.</param>
        /// <param name="bezierType">The type of Bezier</param>
        /// <remarks><para>For quadratic Bezier curves, the number of points defined by the parameters should be a multiple of 2 plus 1.
        /// For cubic Bezier curves, the number of points defined by the parameters should be a multiple of 3 plus 1.  For each curve
        /// drawn after the first, the ending point of the previous curve is used as the starting point.</para></remarks>
        public void AddBeziers (IList<Vector2> points, int offset, int length, BezierType bezierType)
        {
            if (offset < 0 || points.Count < offset + length)
                throw new ArgumentOutOfRangeException("The offset and length are out of range for the given points argument.");

            switch (bezierType) {
                case BezierType.Quadratic:
                    if (length < 3)
                        throw new ArgumentOutOfRangeException("A quadratic bezier needs at least 3 points");
                    for (int i = offset + 2; i < offset + length; i += 2)
                        AddBezier(points[i - 2], points[i - 1], points[i]);
                    break;

                case BezierType.Cubic:
                    if (length < 4)
                        throw new ArgumentOutOfRangeException("A cubic bezier needs at least 4 points");
                    for (int i = offset + 3; i < offset + length; i += 3)
                        AddBezier(points[i - 3], points[i - 2], points[i - 1], points[i]);
                    break;
            }
        }

        /// <summary>
        /// Creates an open <see cref="GraphicsPath"/> from the path with a given <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <returns>A computed <see cref="GraphicsPath"/>.</returns>
        public GraphicsPath Stroke (Pen pen)
        {
            return Stroke(pen, PathType.Open);
        }

        /// <summary>
        /// Creates an open or closed <see cref="GraphicsPath"/> from the path with a given <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        /// <returns>A computed <see cref="GraphicsPath"/>.</returns>
        public GraphicsPath Stroke (Pen pen, PathType pathType)
        {
            return new GraphicsPath(pen, _geometryBuffer, pathType, 0, _geometryIndex);
        }

        /// <summary>
        /// Creates an open <see cref="GraphicsPath"/> from a transformed copy of the path with a given <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="transform">The transform matrix to apply to all of the points in the path.</param>
        /// <returns>A computed <see cref="GraphicsPath"/>.</returns>
        public GraphicsPath Stroke (Pen pen, Matrix transform)
        {
            return Stroke(pen, transform, PathType.Open);
        }

        /// <summary>
        /// Creates an open or closed <see cref="GraphicsPath"/> from a transformed copy of the path with a given <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="transform">The transform matrix to apply to all of the points in the path.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        /// <returns>A computed <see cref="GraphicsPath"/>.</returns>
        public GraphicsPath Stroke (Pen pen, Matrix transform, PathType pathType)
        {
            Vector2[] buffer = new Vector2[_geometryIndex];
            for (int i = 0; i < _geometryIndex; i++)
                buffer[i] = Vector2.Transform(_geometryBuffer[i], transform);

            return new GraphicsPath(pen, buffer, pathType, 0, _geometryIndex);
        }

        /// <summary>
        /// Resets the <see cref="PathBuilder"/> to empty.
        /// </summary>
        public void Reset ()
        {
            _geometryIndex = 0;
        }

        /*public IList<IList<Vector2>> OutlinePathFromStroke (Pen pen, PathType pathType)
        {
            return OutlinePathFromClosedStroke(pen);
        }

        private IList<IList<Vector2>> OutlinePathFromClosedStroke (Pen pen)
        {
            int count = _geometryIndex;

            int vertexCount = pen.MaximumVertexCount(count + 1);
            Vector2[] insetBuffer = new Vector2[vertexCount - count];
            Vector2[] outsetBuffer = new Vector2[vertexCount - count];

            if (IsClose(_geometryBuffer[0], _geometryBuffer[count - 1]))
                count--;

            InsetOutsetCount vioCount = AddJoint(pen, _geometryBuffer[count - 1], _geometryBuffer[0], _geometryBuffer[1], insetBuffer, 0, outsetBuffer, 0);
            int insetBufferIndex = vioCount.InsetCount;
            int outsetBufferIndex = vioCount.OutsetCount;

            for (int i = 0; i < count - 2; i++) {
                vioCount = AddJoint(pen, _geometryBuffer[i], _geometryBuffer[i + 1], _geometryBuffer[i + 2], insetBuffer, insetBufferIndex, outsetBuffer, outsetBufferIndex);
                insetBufferIndex += vioCount.InsetCount;
                outsetBufferIndex += vioCount.OutsetCount;
            }

            vioCount = AddJoint(pen, _geometryBuffer[count - 2], _geometryBuffer[count - 1], _geometryBuffer[0], insetBuffer, insetBufferIndex, outsetBuffer, outsetBufferIndex);
            insetBufferIndex += vioCount.InsetCount;
            outsetBufferIndex += vioCount.OutsetCount;

            return new List<IList<Vector2>> { insetBuffer, outsetBuffer };
        }

        private InsetOutsetCount AddJoint (Pen pen, Vector2 a, Vector2 b, Vector2 c, IList<Vector2> insetBuffer, int insetBufferIndex, IList<Vector2> outsetBuffer, int outsetBufferIndex)
        {
            InsetOutsetCount vioCount = new InsetOutsetCount();

            switch (pen.LineJoin) {
                case LineJoin.Miter:
                    vioCount = pen.ComputeMiter(a, b, c);
                    break;
                case LineJoin.Bevel:
                    vioCount = pen.ComputeBevel(a, b, c);
                    break;
            }

            for (int i = 0; i < vioCount.InsetCount; i++)
                insetBuffer[insetBufferIndex++] = pen.InsetResultBuffer[i];
            for (int i = 0; i < vioCount.OutsetCount; i++)
                outsetBuffer[outsetBufferIndex++] = pen.OutsetResultBuffer[i];

            return vioCount;
        }

        private bool IsClose (Vector2 a, Vector2 b)
        {
            return Math.Abs(a.X - b.X) < 0.001 && Math.Abs(a.Y - b.Y) < 0.001;
        }*/

        private void CheckBufferFreeSpace (int vertexCount)
        {
            if (_geometryBuffer.Length < _geometryIndex + vertexCount)
                Array.Resize(ref _geometryBuffer, (_geometryIndex + vertexCount) * 2);
        }

        private int BuildArcGeometryBuffer (Vector2 p0, Vector2 p1, float height, int subdivisions)
        {
            Vector2 edge01 = p1 - p0;
            Vector2 p01mid = Vector2.Lerp(p0, p1, 0.5f);

            float width = edge01.Length();
            float radius = (height / 2) + (width * width) / (height * 8);

            edge01.Normalize();
            Vector2 edge01t = new Vector2(-edge01.Y, edge01.X);
            Vector2 center = p01mid + edge01t * (radius - height);

            float startAngle = PointToAngle(center, p0);
            float endAngle = PointToAngle(center, p1);

            float arcAngle;
            if (height >= 0) {
                if (height < width / 2) {
                    arcAngle = (Math.Abs(endAngle - startAngle) < Math.PI)
                        ? endAngle - startAngle
                        : endAngle + (float)Math.PI * 2 - startAngle;
                }
                else {
                    arcAngle = ((endAngle - startAngle) > Math.PI)
                        ? endAngle - startAngle
                        : endAngle + (float)Math.PI * 2 - startAngle;
                }
            }
            else {
                if (-height < width / 2) {
                    arcAngle = (Math.Abs(endAngle - startAngle) < Math.PI)
                        ? endAngle - startAngle
                        : endAngle - (float)Math.PI * 2 - startAngle;
                }
                else {
                    arcAngle = ((endAngle - startAngle) > Math.PI)
                        ? startAngle - endAngle
                        : endAngle - (float)Math.PI * 2 - startAngle;
                }
            }

            return BuildArcGeometryBuffer(center, Math.Abs(radius), subdivisions, startAngle, arcAngle);
        }

        private int BuildArcGeometryBuffer (Vector2 center, float radius, int subdivisions, float startAngle, float arcAngle)
        {
            float stopAngle = startAngle + arcAngle;

            startAngle = ClampAngle(startAngle);
            stopAngle = ClampAngle(stopAngle);

            List<Vector2> unitCircle = CalculateCircleSubdivisions(subdivisions);

            float subLength = (float)(2 * Math.PI / subdivisions);

            Vector2 unitStart = new Vector2((float)Math.Cos(-startAngle), (float)Math.Sin(-startAngle));
            Vector2 unitStop = new Vector2((float)Math.Cos(-stopAngle), (float)Math.Sin(-stopAngle));

            int startIndex, stopIndex;
            int vertexCount = 0;

            if (arcAngle >= 0) {
                startIndex = (int)Math.Ceiling(startAngle / subLength);
                stopIndex = (int)Math.Floor(stopAngle / subLength);

                vertexCount = (stopIndex >= startIndex)
                    ? stopIndex - startIndex + 1
                    : (unitCircle.Count - startIndex) + stopIndex + 1;
            }
            else {
                startIndex = (int)Math.Floor(startAngle / subLength);
                stopIndex = (int)Math.Ceiling(stopAngle / subLength);

                vertexCount = (startIndex >= stopIndex)
                    ? startIndex - stopIndex + 1
                    : (unitCircle.Count - stopIndex) + startIndex + 1;
            }

            CheckBufferFreeSpace(vertexCount + 2);

            if (arcAngle >= 0) {
                if ((startIndex * subLength) - startAngle > 0.005f) {
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(-startAngle), center.Y - radius * (float)Math.Sin(-startAngle));
                    vertexCount++;
                }

                if (startIndex <= stopIndex) {
                    for (int i = startIndex; i <= stopIndex; i++)
                        _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }
                else {
                    for (int i = startIndex; i < unitCircle.Count; i++)
                        _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                    for (int i = 0; i <= stopIndex; i++)
                        _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }

                if (stopAngle - (stopIndex * subLength) > 0.005f) {
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(-stopAngle), center.Y - radius * (float)Math.Sin(-stopAngle));
                    vertexCount++;
                }
            }
            else {
                if (startAngle - (startIndex * subLength) > 0.005f) {
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(-startAngle), center.Y - radius * (float)Math.Sin(-startAngle));
                    vertexCount++;
                }

                if (stopIndex <= startIndex) {
                    for (int i = startIndex; i >= stopIndex; i--)
                        _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }
                else {
                    for (int i = startIndex; i >= 0; i--)
                        _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                    for (int i = unitCircle.Count - 1; i >= stopIndex; i--)
                        _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }

                if ((stopIndex * subLength) - stopAngle > 0.005f) {
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(-stopAngle), center.Y - radius * (float)Math.Sin(-stopAngle));
                    vertexCount++;
                }
            }

            return vertexCount;
        }

        private void BuildCircleGeometryBuffer (Vector2 center, float radius, int subdivisions, bool connect)
        {
            List<Vector2> unitCircle = CalculateCircleSubdivisions(subdivisions);

            CheckBufferFreeSpace(subdivisions + 1);

            for (int i = 0; i < subdivisions; i++)
                _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y + radius * unitCircle[i].Y);

            if (connect)
                _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[0].X, center.Y + radius * unitCircle[0].Y);
        }

        private static List<Vector2> CalculateCircleSubdivisions (int divisions)
        {
            lock (_circleCache) {
                if (_circleCache.ContainsKey(divisions))
                    return _circleCache[divisions];
            }

            if (divisions < 0)
                throw new ArgumentOutOfRangeException("divisions");

            double slice = Math.PI * 2 / divisions;

            List<Vector2> unitCircle = new List<Vector2>();

            for (int i = 0; i < divisions; i++) {
                double angle = -slice * i;
                unitCircle.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
            }

            lock (_circleCache) {
                _circleCache.Add(divisions, unitCircle);
                return unitCircle;
            }
        }

        private void BuildQuadraticBezierGeometryBuffer (Vector2 v0, Vector2 v1, Vector2 v2, int subdivisions)
        {
            CheckBufferFreeSpace(subdivisions + 1);

            float step = 1f / (subdivisions - 1);
            float t = 0;

            for (int i = 0; i < subdivisions; i++, t += step) {
                if (1 - t < 5e-6)
                    t = 1;

                float p0 = Bernstein(2, 0, t);
                float p1 = Bernstein(2, 1, t);
                float p2 = Bernstein(2, 2, t);

                float vx = p0 * v0.X + p1 * v1.X + p2 * v2.X;
                float vy = p0 * v0.Y + p1 * v1.Y + p2 * v2.Y;

                _geometryBuffer[_geometryIndex++] = new Vector2(vx, vy);
            }
        }

        private void BuildCubicBezierGeometryBuffer (Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3, int subdivisions)
        {
            CheckBufferFreeSpace(subdivisions + 1);

            float step = 1f / (subdivisions - 1);
            float t = 0;

            for (int i = 0; i < subdivisions; i++, t += step) {
                if (1 - t < 5e-6)
                    t = 1;

                float p0 = Bernstein(3, 0, t);
                float p1 = Bernstein(3, 1, t);
                float p2 = Bernstein(3, 2, t);
                float p3 = Bernstein(3, 3, t);

                float vx = p0 * v0.X + p1 * v1.X + p2 * v2.X + p3 * v3.X;
                float vy = p0 * v0.Y + p1 * v1.Y + p2 * v2.Y + p3 * v3.Y;

                _geometryBuffer[_geometryIndex++] = new Vector2(vx, vy);
            }
        }

        private static double Factorial (int n)
        {
            if (n < 0 || n > 32)
                throw new ArgumentOutOfRangeException("n", "n must be between 0 and 32.");

            return _factorials[n];
        }

        private static float Ni (int n, int i)
        {
            double a1 = Factorial(n);
            double a2 = Factorial(i);
            double a3 = Factorial(n - i);

            return (float)(a1 / (a2 * a3));
        }

        private static float Bernstein (int n, int i, float t)
        {
            float ti = (t == 0 && i == 0) ? 1f : (float)Math.Pow(t, i);
            float tni = (n == i && t == 1) ? 1f : (float)Math.Pow((1 - t), (n - i));

            return Ni(n, i) * ti * tni;
        }

        private static int DefaultBezierSubdivisions (Vector2 p0, Vector2 p1, Vector2 p2)
        {
            Vector2 p01 = Vector2.Lerp(p0, p1, .5f);
            Vector2 p12 = Vector2.Lerp(p1, p2, .5f);

            float dist = ApproxDistance(p0, p01) + ApproxDistance(p01, p12) + ApproxDistance(p12, p2);
            return (int)(dist * 0.10f);
        }

        private static int DefaultBezierSubdivisions (Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            Vector2 p01 = Vector2.Lerp(p0, p1, .5f);
            Vector2 p12 = Vector2.Lerp(p1, p2, .5f);
            Vector2 p23 = Vector2.Lerp(p2, p3, .5f);

            float dist = ApproxDistance(p0, p01) + ApproxDistance(p01, p12) + ApproxDistance(p12, p23) + ApproxDistance(p23, p3);
            return (int)(dist * 0.10f);
        }

        private static float ApproxDistance (Vector2 p0, Vector2 p1)
        {
            float dx = Math.Abs(p1.X - p0.X);
            float dy = Math.Abs(p1.Y - p0.Y);

            return ApproxDistance(dx, dy);
        }

        private static float ApproxDistance (float dx, float dy)
        {
            if (dy < dx) {
                float w = dy * 0.25f;
                return dx + w + w * 0.5f;
            }
            else {
                float w = dx * 0.25f;
                return dy + w + w * 0.5f;
            }
        }

        private static float PointToAngle (Vector2 center, Vector2 point)
        {
            double angle = Math.Atan2(point.Y - center.Y, point.X - center.X);
            if (angle < 0)
                angle += Math.PI * 2;

            return (float)angle;
        }

        private static float ClampAngle (float angle)
        {
            if (angle < 0)
                angle += (float)(Math.Ceiling(angle / (Math.PI * -2)) * Math.PI * 2);
            else if (angle >= (Math.PI * 2))
                angle -= (float)(Math.Floor(angle / (Math.PI * 2)) * Math.PI * 2);

            return angle;
        }

        private static int DefaultSubdivisions (float radius)
        {
            return (int)Math.Ceiling(radius / 1.5);
        }

        private bool LastPointEqual (Vector2 point)
        {
            return (_geometryIndex > 0 && _geometryBuffer[_geometryIndex - 1] == point);
        }

        private bool LastPointClose (Vector2 point)
        {
            return (_geometryIndex > 0 && PointsClose(_geometryBuffer[_geometryIndex - 1], point));
        }

        private static bool PointsClose (Vector2 a, Vector2 b)
        {
            return (Math.Abs(a.X - b.X) < 0.005 && Math.Abs(a.Y - b.Y) < 0.005);
        }
    }
}
