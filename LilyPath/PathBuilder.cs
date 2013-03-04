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

            _geometryIndex--;

            BuildArcGeometryBuffer(_geometryBuffer[_geometryIndex], point, height, subdivisions);
        }

        /// <summary>
        /// Appends an arc between the current endpoint and a point defined by a center and arc angle.
        /// </summary>
        /// <param name="center">The center of a circle containing the path's current endpoint and destination point.</param>
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw counter-clockwise.</param>
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
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw counter-clockwise.</param>
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
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw counter-clockwise.</param>
        public void Addarc (Vector2 center, float radius, float startAngle, float arcAngle)
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
        /// <param name="arcAngle">The sweep of the arc in radians.  Positive values draw counter-clockwise.</param>
        /// <param name="subdivisions">The number of subdivisions in a circle of the same arc radius.</param>
        public void AddArc (Vector2 center, float radius, float startAngle, float arcAngle, int subdivisions)
        {
            Vector2 startPoint = new Vector2(center.X + radius * (float)Math.Cos(startAngle), center.Y + radius * (float)Math.Sin(startAngle));

            if (!LastPointEqual(startPoint))
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
            if (!LastPointEqual(p0))
                _geometryIndex--;

            BuildArcGeometryBuffer(p0, p1, height, subdivisions);
        }

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
                    arcAngle = ((endAngle - startAngle) < Math.PI)
                        ? endAngle - startAngle
                        : endAngle - (float)Math.PI * 2 - startAngle;
                }
                else {
                    arcAngle = ((endAngle - startAngle) > Math.PI)
                        ? endAngle - startAngle
                        : endAngle - (float)Math.PI * 2 - startAngle;
                }
            }
            else {
                if (-height < width / 2) {
                    arcAngle = ((endAngle - startAngle) < Math.PI)
                        ? endAngle - startAngle
                        : endAngle + (float)Math.PI * 2 - startAngle;
                }
                else {
                    arcAngle = ((endAngle - startAngle) > Math.PI)
                        ? endAngle - startAngle
                        : endAngle + (float)Math.PI * 2 - startAngle;
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

            Vector2 unitStart = new Vector2((float)Math.Cos(startAngle), (float)Math.Sin(startAngle));
            Vector2 unitStop = new Vector2((float)Math.Cos(stopAngle), (float)Math.Sin(stopAngle));

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
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(startAngle), center.Y - radius * (float)Math.Sin(startAngle));
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
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(stopAngle), center.Y - radius * (float)Math.Sin(stopAngle));
                    vertexCount++;
                }
            }
            else {
                if (startAngle - (startIndex * subLength) > 0.005f) {
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(startAngle), center.Y - radius * (float)Math.Sin(startAngle));
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
                    _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * (float)Math.Cos(stopAngle), center.Y - radius * (float)Math.Sin(stopAngle));
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
                _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);

            if (connect)
                _geometryBuffer[_geometryIndex++] = new Vector2(center.X + radius * unitCircle[0].X, center.Y - radius * unitCircle[0].Y);
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
                double angle = slice * i;
                unitCircle.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
            }

            lock (_circleCache) {
                _circleCache.Add(divisions, unitCircle);
                return unitCircle;
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
