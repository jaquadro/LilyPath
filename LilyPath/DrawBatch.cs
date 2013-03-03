using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    public enum ArcType
    {
        Segment,
        Sector
    }

    public class DrawBatch
    {
        private struct DrawingInfo
        {
            public Texture2D Texture;
            public PrimitiveType Primitive;
            public int IndexCount;
            public int VertexCount;
        }

        private GraphicsDevice _device;
        private bool _inDraw;

        private DrawingInfo[] _infoBuffer;
        private short[] _indexBuffer;
        private VertexPositionColorTexture[] _vertexBuffer;

        private Vector2[] _computeBuffer;
        private Vector2[] _geometryBuffer;

        private int _infoBufferIndex;
        private int _indexBufferIndex;
        private int _vertexBufferIndex;

        private Triangulator _triangulator;

        private BlendState _blendState;
        private SamplerState _samplerState;
        private DepthStencilState _depthStencilState;
        private RasterizerState _rasterizerState;
        private Matrix _transform;

        private static BasicEffect _effect;

        private Texture2D _defaultTexture;

        public DrawBatch (GraphicsDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            _device = device;

            _infoBuffer = new DrawingInfo[2048];
            _indexBuffer = new short[32768];
            _vertexBuffer = new VertexPositionColorTexture[8192];
            _computeBuffer = new Vector2[64];
            _geometryBuffer = new Vector2[256];

            _effect = new BasicEffect(device);
            _effect.TextureEnabled = true;
            _effect.VertexColorEnabled = true;

            _defaultTexture = new Texture2D(device, 1, 1);
            _defaultTexture.SetData<Color>(new Color[] { Color.White * .6f });
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _device; }
        }

        public void Begin ()
        {
            Begin(null, null, null, null, Matrix.Identity);
        }

        public void Begin (BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Matrix transform)
        {
            if (_inDraw)
                throw new InvalidOperationException("DrawBatch already inside Begin/End pair");

            _inDraw = true;

            _blendState = blendState;
            _samplerState = samplerState;
            _depthStencilState = depthStencilState;
            _rasterizerState = rasterizerState;
            _transform = transform;

            _infoBufferIndex = 0;
            _indexBufferIndex = 0;
            _vertexBufferIndex = 0;
        }

        public void End ()
        {
            FlushBuffer();

            _inDraw = false;
        }

        public void DrawRectangle (Rectangle rect, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(8, 24);

            AddInfo(PrimitiveType.TriangleList, 8, 24, pen.Brush);

            Vector2 a = new Vector2(rect.Left, rect.Top);
            Vector2 b = new Vector2(rect.Right, rect.Top);
            Vector2 c = new Vector2(rect.Right, rect.Bottom);
            Vector2 d = new Vector2(rect.Left, rect.Bottom);

            int baseVertexIndex = _vertexBufferIndex;

            AddMiteredJoint(a, b, c, pen);
            AddMiteredJoint(b, c, d, pen);
            AddMiteredJoint(c, d, a, pen);
            AddMiteredJoint(d, a, b, pen);

            AddSegment(baseVertexIndex + 0, baseVertexIndex + 2);
            AddSegment(baseVertexIndex + 2, baseVertexIndex + 4);
            AddSegment(baseVertexIndex + 4, baseVertexIndex + 6);
            AddSegment(baseVertexIndex + 6, baseVertexIndex + 0);
        }

        public void DrawPrimitiveRectangle (Rectangle rect, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(4, 8);

            AddInfo(PrimitiveType.LineList, 4, 8, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            _vertexBuffer[_vertexBufferIndex++] = new VertexPositionColorTexture(new Vector3(rect.X, rect.Y, 0), pen.Color, new Vector2(rect.X, rect.Y));
            _vertexBuffer[_vertexBufferIndex++] = new VertexPositionColorTexture(new Vector3(rect.Right, rect.Y, 0), pen.Color, new Vector2(rect.Right, rect.Y));
            _vertexBuffer[_vertexBufferIndex++] = new VertexPositionColorTexture(new Vector3(rect.Right, rect.Bottom, 0), pen.Color, new Vector2(rect.Right, rect.Bottom));
            _vertexBuffer[_vertexBufferIndex++] = new VertexPositionColorTexture(new Vector3(rect.X, rect.Bottom, 0), pen.Color, new Vector2(rect.X, rect.Bottom));

            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + 2);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + 2);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + 3);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + 3);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex);
        }

        public void DrawPoint (Point point, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(4, 6);

            AddInfo(PrimitiveType.TriangleList, 4, 6, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            float w2 = pen.Width / 2;
            AddVertex(new Vector2(point.X - w2, point.Y - w2), pen);
            AddVertex(new Vector2(point.X + w2, point.Y - w2), pen);
            AddVertex(new Vector2(point.X - w2, point.Y + w2), pen);
            AddVertex(new Vector2(point.X + w2, point.Y + w2), pen);

            AddSegment(baseVertexIndex + 0, baseVertexIndex + 2);
        }

        public void DrawLine (Point p0, Point p1, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(4, 6);

            AddInfo(PrimitiveType.TriangleList, 4, 6, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            AddStartPoint(new Vector2(p0.X, p0.Y), new Vector2(p1.X, p1.Y), pen);
            AddEndPoint(new Vector2(p0.X, p0.Y), new Vector2(p1.X, p1.Y), pen);

            AddSegment(baseVertexIndex + 0, baseVertexIndex + 2);
        }

        public void DrawPrimitiveLine (Point p0, Point p1, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(2, 2);

            AddInfo(PrimitiveType.LineList, 2, 2, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            _vertexBuffer[_vertexBufferIndex++] = new VertexPositionColorTexture(new Vector3(p0.X, p0.Y, 0), pen.Color, new Vector2(p0.X, p0.Y));
            _vertexBuffer[_vertexBufferIndex++] = new VertexPositionColorTexture(new Vector3(p1.X, p1.Y, 0), pen.Color, new Vector2(p1.X, p1.Y));

            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex);
            _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + 1);
        }

        public void DrawPrimitivePath (IList<Vector2> points, Pen pen)
        {
            DrawPrimitivePath(points, pen, 0, points.Count, PathType.Open);
        }

        public void DrawPrimitivePath (IList<Vector2> points, Pen pen, PathType pathType)
        {
            DrawPrimitivePath(points, pen, 0, points.Count, pathType);
        }

        public void DrawPrimitivePath (IList<Vector2> points, Pen pen, int offset, int count, PathType pathType)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            if (offset + count > points.Count)
                throw new ArgumentOutOfRangeException("points", "The offset and count exceed the bounds of the list");

            RequestBufferSpace(count, (pathType == PathType.Open) ? count * 2 - 2 : count * 2);

            AddInfo(PrimitiveType.LineList, count, (pathType == PathType.Open) ? count * 2 - 2 : count * 2, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            for (int i = 0; i < count; i++) {
                Vector2 pos = new Vector2(points[offset + i].X, points[offset + i].Y);
                _vertexBuffer[_vertexBufferIndex++] = new VertexPositionColorTexture(new Vector3(pos, 0), pen.Color, Vector2.Zero);
            }

            for (int i = 1; i < count; i++) {
                _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + i - 1);
                _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + i);
            }

            if (pathType == PathType.Closed) {
                _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex + count - 1);
                _indexBuffer[_indexBufferIndex++] = (short)(baseVertexIndex);
            }
        }

        public void DrawPath (GraphicsPath path)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(path.VertexCount, path.IndexCount);

            AddInfo(PrimitiveType.TriangleList, path.VertexCount, path.IndexCount, path.Pen.Brush);

            if (path.VertexTextureData != null) {
                for (int i = 0; i < path.VertexCount; i++) {
                    _vertexBuffer[_vertexBufferIndex + i] = new VertexPositionColorTexture(
                        new Vector3(path.VertexPositionData[i], 0),
                        path.VertexColorData[i],
                        path.VertexTextureData[i]);
                }
            }
            else {
                for (int i = 0; i < path.VertexCount; i++) {
                    _vertexBuffer[_vertexBufferIndex + i] = new VertexPositionColorTexture(
                        new Vector3(path.VertexPositionData[i], 0),
                        path.VertexColorData[i],
                        Vector2.Zero);
                }
            }

            for (int i = 0; i < path.IndexCount; i++) {
                _indexBuffer[_indexBufferIndex + i] = (short)(path.IndexData[i] + _vertexBufferIndex);
            }

            _vertexBufferIndex += path.VertexCount;
            _indexBufferIndex += path.IndexCount;
        }

        public void DrawCircle (Point center, float radius, Pen pen)
        {
            DrawCircle(center, radius, (int)Math.Ceiling(radius / 1.5), pen);
        }

        public void DrawCircle (Point center, float radius, int subdivisions, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            BuildCircleGeometryBuffer(center, radius, subdivisions, false);
            AddClosedPath(_geometryBuffer, 0, subdivisions, pen);
        }

        public void DrawPrimitiveCircle (Point center, float radius, Pen pen)
        {
            DrawPrimitiveCircle(center, radius, (int)Math.Ceiling(radius / 1.5), pen);
        }

        public void DrawPrimitiveCircle (Point center, float radius, int subdivisions, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            BuildCircleGeometryBuffer(center, radius, subdivisions, false);
            DrawPrimitivePath(_geometryBuffer, pen, 0, subdivisions, PathType.Closed);
        }

        private void BuildCircleGeometryBuffer (Point center, float radius, int subdivisions, bool connect)
        {
            List<Vector2> unitCircle = CalculateCircleSubdivisions(subdivisions);

            if (_geometryBuffer.Length < subdivisions + 1)
                Array.Resize(ref _geometryBuffer, (subdivisions + 1) * 2);

            for (int i = 0; i < subdivisions; i++)
                _geometryBuffer[i] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);

            if (connect)
                _geometryBuffer[subdivisions] = new Vector2(center.X + radius * unitCircle[0].X, center.Y - radius * unitCircle[0].Y);
        }

        public void DrawArc (Point center, float radius, float startAngle, float arcAngle, Pen pen)
        {
            DrawArc(center, radius, startAngle, arcAngle, (int)Math.Ceiling(radius / 1.5), pen);
        }

        public void DrawArc (Point center, float radius, float startAngle, float arcAngle, int subdivisions, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            int vertexCount = BuildArcGeometryBuffer(center, radius, subdivisions, startAngle, arcAngle);
            if (vertexCount > 1)
                AddPath(_geometryBuffer, 0, vertexCount, pen);
        }

        public void DrawPrimitiveArc (Point center, float radius, float startAngle, float arcAngle, Pen pen)
        {
            DrawPrimitiveArc(center, radius, startAngle, arcAngle, (int)Math.Ceiling(radius / 1.5), pen);
        }

        public void DrawPrimitiveArc (Point center, float radius, float startAngle, float arcAngle, int subdivisions, Pen pen)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            int vertexCount = BuildArcGeometryBuffer(center, radius, subdivisions, startAngle, arcAngle);
            if (vertexCount > 1)
                DrawPrimitivePath(_geometryBuffer, pen, 0, vertexCount, PathType.Open);
        }

        public void DrawPrimitiveClosedArc (Point center, float radius, float startAngle, float arcAngle, Pen pen, ArcType arcType)
        {
            DrawPrimitiveClosedArc(center, radius, startAngle, arcAngle, (int)Math.Ceiling(radius / 1.5), pen, arcType);
        }

        public void DrawPrimitiveClosedArc (Point center, float radius, float startAngle, float arcAngle, int subdivisions, Pen pen, ArcType arcType)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            int vertexCount = BuildArcGeometryBuffer(center, radius, subdivisions, startAngle, arcAngle);
            if (vertexCount > 1) {
                if (arcType == ArcType.Sector) {
                    if (_geometryBuffer.Length < vertexCount + 1)
                        Array.Resize(ref _geometryBuffer, (vertexCount + 1) * 2);

                    _geometryBuffer[vertexCount++] = new Vector2(center.X, center.Y);
                }

                DrawPrimitivePath(_geometryBuffer, pen, 0, vertexCount, PathType.Closed);
            }
        }

        public void DrawClosedArc (Point center, float radius, float startAngle, float arcAngle, Pen pen, ArcType arcType)
        {
            DrawClosedArc(center, radius, startAngle, arcAngle, (int)Math.Ceiling(radius / 1.5), pen, arcType);
        }

        public void DrawClosedArc (Point center, float radius, float startAngle, float arcAngle, int subdivisions, Pen pen, ArcType arcType)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            int vertexCount = BuildArcGeometryBuffer(center, radius, subdivisions, startAngle, arcAngle);
            if (vertexCount > 1) {
                if (arcType == ArcType.Sector) {
                    if (_geometryBuffer.Length < vertexCount + 1)
                        Array.Resize(ref _geometryBuffer, (vertexCount + 1) * 2);

                    _geometryBuffer[vertexCount++] = new Vector2(center.X, center.Y);
                }

                AddClosedPath(_geometryBuffer, 0, vertexCount, pen);
            }
        }

        private float ClampAngle (float angle)
        {
            if (angle < 0)
                angle += (float)(Math.Ceiling(angle / (Math.PI * -2)) * Math.PI * 2);
            else if (angle >= (Math.PI * 2))
                angle -= (float)(Math.Floor(angle / (Math.PI * 2)) * Math.PI * 2);

            return angle;
        }

        private int BuildArcGeometryBuffer (Point center, float radius, int subdivisions, float startAngle, float arcAngle)
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
            int bufIndex = 0;

            if (_geometryBuffer.Length < vertexCount + 2)
                Array.Resize(ref _geometryBuffer, (vertexCount + 2) * 2);

            if (arcAngle >= 0) {
                if ((startIndex * subLength) - startAngle > 0.005f) {
                    _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * (float)Math.Cos(startAngle), center.Y - radius * (float)Math.Sin(startAngle));
                    vertexCount++;
                }

                if (startIndex <= stopIndex) {
                    for (int i = startIndex; i <= stopIndex; i++)
                        _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }
                else {
                    for (int i = startIndex; i < unitCircle.Count; i++)
                        _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                    for (int i = 0; i <= stopIndex; i++)
                        _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }

                if (stopAngle - (stopIndex * subLength) > 0.005f) {
                    _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * (float)Math.Cos(stopAngle), center.Y - radius * (float)Math.Sin(stopAngle));
                    vertexCount++;
                }
            }
            else {
                if (startAngle - (startIndex * subLength) > 0.005f) {
                    _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * (float)Math.Cos(startAngle), center.Y - radius * (float)Math.Sin(startAngle));
                    vertexCount++;
                }

                if (stopIndex <= startIndex) {
                    for (int i = startIndex; i >= stopIndex; i--)
                        _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }
                else {
                    for (int i = startIndex; i >= 0; i--)
                        _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                    for (int i = unitCircle.Count - 1; i >= stopIndex; i--)
                        _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * unitCircle[i].X, center.Y - radius * unitCircle[i].Y);
                }

                if ((stopIndex * subLength) - stopAngle > 0.005f) {
                    _geometryBuffer[bufIndex++] = new Vector2(center.X + radius * (float)Math.Cos(stopAngle), center.Y - radius * (float)Math.Sin(stopAngle));
                    vertexCount++;
                }
            }

            return vertexCount;
        }

        public void FillCircle (Point center, float radius, Brush brush)
        {
            FillCircle(center, radius, (int)Math.Ceiling(radius / 1.5), brush);
        }

        public void FillCircle (Point center, float radius, int subdivisions, Brush brush)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(subdivisions + 1, subdivisions * 3);
            AddInfo(PrimitiveType.TriangleList, subdivisions + 1, subdivisions * 3, brush);

            BuildCircleGeometryBuffer(center, radius, subdivisions, true);

            int baseVertexIndex = _vertexBufferIndex;

            for (int i = 0; i < subdivisions; i++)
                AddVertex(_geometryBuffer[i], brush);

            AddVertex(new Vector2(center.X, center.Y), brush);

            for (int i = 0; i < subdivisions - 1; i++)
                AddTriangle(baseVertexIndex + subdivisions, baseVertexIndex + i, baseVertexIndex + i + 1);

            AddTriangle(baseVertexIndex + subdivisions, baseVertexIndex + subdivisions - 1, baseVertexIndex);
        }

        public void FillArc (Point center, float radius, float startAngle, float arcAngle, Brush brush, ArcType arcType)
        {
            FillArc(center, radius, startAngle, arcAngle, (int)Math.Ceiling(radius / 1.5), brush, arcType);
        }

        public void FillArc (Point center, float radius, float startAngle, float arcAngle, int subdivisions, Brush brush, ArcType arcType)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            int vertexCount = BuildArcGeometryBuffer(center, radius, subdivisions, startAngle, arcAngle);

            RequestBufferSpace(vertexCount + 1, (vertexCount - 1) * 3);
            AddInfo(PrimitiveType.TriangleList, vertexCount + 1, (vertexCount - 1) * 3, brush);

            int baseVertexIndex = _vertexBufferIndex;

            for (int i = 0; i < vertexCount; i++)
                AddVertex(_geometryBuffer[i], brush);

            switch (arcType) {
                case ArcType.Sector:
                    AddVertex(new Vector2(center.X, center.Y), brush);
                    break;
                case ArcType.Segment:
                    AddVertex(new Vector2((_geometryBuffer[0].X + _geometryBuffer[vertexCount - 1].X) / 2, 
                        (_geometryBuffer[0].Y + _geometryBuffer[vertexCount - 1].Y) / 2), brush); 
                    break;
            }

            if (arcAngle < 0) {
                for (int i = 0; i < vertexCount - 1; i++)
                    AddTriangle(baseVertexIndex + vertexCount, baseVertexIndex + i, baseVertexIndex + i + 1);
            }
            else {
                for (int i = vertexCount - 1; i > 0; i--)
                    AddTriangle(baseVertexIndex + vertexCount, baseVertexIndex + i, baseVertexIndex + i - 1);
            }
        }

        public void FillRectangle (Rectangle rect, Brush brush)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            RequestBufferSpace(4, 6);
            AddInfo(PrimitiveType.TriangleList, 4, 6, brush);

            int baseVertexIndex = _vertexBufferIndex;

            AddVertex(new Vector2(rect.Left, rect.Top), brush);
            AddVertex(new Vector2(rect.Right, rect.Top), brush);
            AddVertex(new Vector2(rect.Left, rect.Bottom), brush);
            AddVertex(new Vector2(rect.Right, rect.Bottom), brush);

            AddTriangle(baseVertexIndex + 0, baseVertexIndex + 1, baseVertexIndex + 2);
            AddTriangle(baseVertexIndex + 1, baseVertexIndex + 3, baseVertexIndex + 2);
        }

        public void FillPath (IList<Vector2> points, Brush brush)
        {
            FillPath(points, 0, points.Count, brush);
        }

        public void FillPath (IList<Vector2> points, int offset, int count, Brush brush)
        {
            if (!_inDraw)
                throw new InvalidOperationException();

            if (_triangulator == null)
                _triangulator = new Triangulator();

            _triangulator.Triangulate(points, offset, count);

            RequestBufferSpace(count, _triangulator.ComputedIndexCount);
            AddInfo(PrimitiveType.TriangleList, count, _triangulator.ComputedIndexCount, brush);

            int baseVertexIndex = _vertexBufferIndex;

            for (int i = 0; i < count; i++) {
                AddVertex(points[offset + i], brush);
            }

            for (int i = 0; i < _triangulator.ComputedIndexCount; i++) {
                _indexBuffer[_indexBufferIndex + i] = (short)(_triangulator.ComputedIndexes[i] + baseVertexIndex);
            }

            _indexBufferIndex += _triangulator.ComputedIndexCount;
        }

        private void SetRenderState ()
        {
            _device.BlendState = (_blendState != null)
                ? _blendState : BlendState.AlphaBlend;

            _device.DepthStencilState = (_depthStencilState != null)
                ? _depthStencilState : DepthStencilState.None;

            _device.RasterizerState = (_rasterizerState != null)
                ? _rasterizerState : RasterizerState.CullCounterClockwise;

            _device.SamplerStates[0] = (_samplerState != null)
                ? _samplerState : SamplerState.PointWrap;

            _effect.Projection = Matrix.CreateOrthographicOffCenter(0, _device.Viewport.Width, _device.Viewport.Height, 0, -1, 1);
            _effect.World = _transform;
            _effect.CurrentTechnique.Passes[0].Apply();
        }

        private void AddMiteredJoint (Vector2 a, Vector2 b, Vector2 c, Pen pen)
        {
            pen.ComputeMiter(_computeBuffer, 0, a, b, c);

            AddVertex(_computeBuffer[0], pen);
            AddVertex(_computeBuffer[1], pen);
        }

        private void AddStartPoint (Vector2 a, Vector2 b, Pen pen)
        {
            pen.ComputeStartPoint(_computeBuffer, 0, a, b);

            AddVertex(_computeBuffer[0], pen);
            AddVertex(_computeBuffer[1], pen);
        }

        private void AddEndPoint (Vector2 a, Vector2 b, Pen pen)
        {
            pen.ComputeEndPoint(_computeBuffer, 0, a, b);

            AddVertex(_computeBuffer[0], pen);
            AddVertex(_computeBuffer[1], pen);
        }

        private void AddInfo (PrimitiveType primitiveType, int vertexCount, int indexCount, Brush brush)
        {
            _infoBuffer[_infoBufferIndex].Primitive = primitiveType;
            _infoBuffer[_infoBufferIndex].Texture = brush != null ? brush.Texture : _defaultTexture;
            _infoBuffer[_infoBufferIndex].IndexCount = indexCount;
            _infoBuffer[_infoBufferIndex].VertexCount = vertexCount;
            _infoBufferIndex++;
        }

        private void AddInfo (PrimitiveType primitiveType, int vertexCount, int indexCount, Texture2D texture)
        {
            _infoBuffer[_infoBufferIndex].Primitive = primitiveType;
            _infoBuffer[_infoBufferIndex].Texture = texture ?? _defaultTexture;
            _infoBuffer[_infoBufferIndex].IndexCount = indexCount;
            _infoBuffer[_infoBufferIndex].VertexCount = vertexCount;
            _infoBufferIndex++;
        }

        private void AddClosedPath (Vector2[] points, int offset, int count, Pen pen)
        {
            RequestBufferSpace(count * 2, count * 6);

            AddInfo(PrimitiveType.TriangleList, count * 2, count * 6, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            for (int i = 0; i < count - 2; i++) {
                AddMiteredJoint(points[offset + i], points[offset + i + 1], points[offset + i + 2], pen);
            }

            AddMiteredJoint(points[offset + count - 2], points[offset + count - 1], points[offset + 0], pen);
            AddMiteredJoint(points[offset + count - 1], points[offset + 0], points[offset + 1], pen);

            for (int i = 0; i < count - 1; i++) {
                AddSegment(baseVertexIndex + i * 2, baseVertexIndex + (i + 1) * 2);
            }

            AddSegment(baseVertexIndex + (count - 1) * 2, baseVertexIndex + 0);
        }

        private void AddPath (Vector2[] points, int offset, int count, Pen pen)
        {
            RequestBufferSpace(count * 2, (count - 1) * 6);

            AddInfo(PrimitiveType.TriangleList, count * 2, (count - 1) * 6, pen.Brush);

            int baseVertexIndex = _vertexBufferIndex;

            AddStartPoint(points[offset + 0], points[offset + 1], pen);

            for (int i = 0; i < count - 2; i++)
                AddMiteredJoint(points[offset + i], points[offset + i + 1], points[offset + i + 2], pen);

            AddEndPoint(points[offset + count - 2], points[offset + count - 1], pen);

            for (int i = 0; i < count - 1; i++)
                AddSegment(baseVertexIndex + i * 2, baseVertexIndex + (i + 1) * 2);
        }

        private void AddVertex (Vector2 position, Pen pen)
        {
            VertexPositionColorTexture vertex = new VertexPositionColorTexture();
            vertex.Position = new Vector3(position, 0);
            vertex.Color = pen.Color;

            if (pen.Brush != null && pen.Brush.Texture != null) {
                Texture2D tex = pen.Brush.Texture;
                vertex.TextureCoordinate = new Vector2(position.X / tex.Width, position.Y / tex.Height);
                vertex.Color *= pen.Brush.Alpha;
            }
            else {
                vertex.TextureCoordinate = new Vector2(position.X, position.Y);
            }

            _vertexBuffer[_vertexBufferIndex++] = vertex;
        }

        private void AddVertex (Vector2 position, Brush brush)
        {
            VertexPositionColorTexture vertex = new VertexPositionColorTexture();
            vertex.Position = new Vector3(position, 0);
            vertex.Color = Color.White;

            if (brush != null && brush.Texture != null) {
                Texture2D tex = brush.Texture;
                vertex.TextureCoordinate = new Vector2(position.X / tex.Width, position.Y / tex.Height);
                vertex.Color *= brush.Alpha;
            }

            _vertexBuffer[_vertexBufferIndex++] = vertex;
        }

        private void AddSegment (int startVertexIndex, int endVertexIndex)
        {
            _indexBuffer[_indexBufferIndex++] = (short)(startVertexIndex + 0);
            _indexBuffer[_indexBufferIndex++] = (short)(startVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(endVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(startVertexIndex + 0);
            _indexBuffer[_indexBufferIndex++] = (short)(endVertexIndex + 1);
            _indexBuffer[_indexBufferIndex++] = (short)(endVertexIndex + 0);
        }

        private void AddPrimitiveLineSegment (int startVertexIndex, int endVertexIndex)
        {
            _indexBuffer[_indexBufferIndex++] = (short)startVertexIndex;
            _indexBuffer[_indexBufferIndex++] = (short)endVertexIndex;
        }

        private void AddTriangle (int a, int b, int c)
        {
            _indexBuffer[_indexBufferIndex++] = (short)a;
            _indexBuffer[_indexBufferIndex++] = (short)b;
            _indexBuffer[_indexBufferIndex++] = (short)c;
        }

        private void FlushBuffer ()
        {
            SetRenderState();

            int vertexOffset = 0;
            int indexOffset = 0;
            int vertexCount = 0;
            int indexCount = 0;
            Texture2D texture = null;
            PrimitiveType primitive = PrimitiveType.TriangleList;

            for (int i = 0; i < _infoBufferIndex; i++) {
                if (texture != _infoBuffer[i].Texture || primitive != _infoBuffer[i].Primitive) {
                    if (indexCount > 0) {
                        for (int j = 0; j < indexCount; j++) {
                            _indexBuffer[indexOffset + j] -= (short)vertexOffset;
                        }

                        RenderBatch(primitive, indexOffset, indexCount, vertexOffset, vertexCount, texture);
                    }

                    vertexOffset += vertexCount;
                    indexOffset += indexCount;
                    vertexCount = 0;
                    indexCount = 0;
                    texture = _infoBuffer[i].Texture;
                    primitive = _infoBuffer[i].Primitive;
                }

                vertexCount += _infoBuffer[i].VertexCount;
                indexCount += _infoBuffer[i].IndexCount;
            }

            if (indexCount > 0) {
                for (int j = 0; j < indexCount; j++) {
                    _indexBuffer[indexOffset + j] -= (short)vertexOffset;
                }

                RenderBatch(primitive, indexOffset, indexCount, vertexOffset, vertexCount, texture);
            }

            ClearInfoBuffer();

            _infoBufferIndex = 0;
            _indexBufferIndex = 0;
            _vertexBufferIndex = 0;
        }

        private void RenderBatch (PrimitiveType primitiveType, int indexOffset, int indexCount, int vertexOffset, int vertexCount, Texture2D texture)
        {
            _device.Textures[0] = texture;
            switch (primitiveType) {
                case PrimitiveType.LineList:
                    _device.DrawUserIndexedPrimitives(primitiveType, _vertexBuffer, vertexOffset, vertexCount, _indexBuffer, indexOffset, indexCount / 2);
                    break;
                case PrimitiveType.TriangleList:
                    _device.DrawUserIndexedPrimitives(primitiveType, _vertexBuffer, vertexOffset, vertexCount, _indexBuffer, indexOffset, indexCount / 3);
                    break;
            }
        }

        private void ClearInfoBuffer ()
        {
            for (int i = 0; i < _infoBufferIndex; i++)
                _infoBuffer[i].Texture = null;
        }

        private void RequestBufferSpace (int newVertexCount, int newIndexCount)
        {
            if (_indexBufferIndex + newIndexCount > short.MaxValue) {
                FlushBuffer();
            }

            if (_infoBufferIndex + 1 > _infoBuffer.Length) {
                Array.Resize(ref _infoBuffer, _infoBuffer.Length * 2);
            }

            if (_indexBufferIndex + newIndexCount >= _indexBuffer.Length) {
                Array.Resize(ref _indexBuffer, _indexBuffer.Length * 2);
            }

            if (_vertexBufferIndex + newVertexCount >= _vertexBuffer.Length) {
                Array.Resize(ref _vertexBuffer, _vertexBuffer.Length * 2);
            }
        }

        private Dictionary<int, List<Vector2>> _circleCache = new Dictionary<int,List<Vector2>>();

        private List<Vector2> CalculateCircleSubdivisions (int divisions)
        {
            if (_circleCache.ContainsKey(divisions))
                return _circleCache[divisions];

            if (divisions < 0)
                throw new ArgumentOutOfRangeException("divisions");

            double slice = Math.PI * 2 / divisions;

            List<Vector2> unitCircle = new List<Vector2>();

            for (int i = 0; i < divisions; i++) {
                double angle = slice * i;
                unitCircle.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
            }

            _circleCache.Add(divisions, unitCircle);
            return unitCircle;
        }
    }

    
}
