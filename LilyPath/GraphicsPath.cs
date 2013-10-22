using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using LilyPath.Utility;

namespace LilyPath
{
    /// <summary>
    /// Represents computed path geometry.
    /// </summary>
    public interface IGraphicsPath
    {
        /// <summary>
        /// The number of vertices in the computed geometry.
        /// </summary>
        int VertexCount { get; }

        /// <summary>
        /// The number of vertex indexes in the computed geometry.
        /// </summary>
        int IndexCount { get; }

        /// <summary>
        /// The raw vertex data of the computed geometry.
        /// </summary>
        Vector2[] VertexPositionData { get; }

        /// <summary>
        /// The raw texture data of the computed geometry.
        /// </summary>
        Vector2[] VertexTextureData { get; }

        /// <summary>
        /// The raw color data of the computed geometry.
        /// </summary>
        Color[] VertexColorData { get; }

        /// <summary>
        /// The raw index data of the computed geometry.
        /// </summary>
        short[] IndexData { get; }

        /// <summary>
        /// The <see cref="Pen"/> used to compute the geometry.
        /// </summary>
        Pen Pen { get; }
    }

    /// <summary>
    /// Represents a stroked path.
    /// </summary>
    public class GraphicsPath : IGraphicsPath
    {
        private static GraphicsPath[] _emptyOutlinePaths = new GraphicsPath[0];

        private Pen _pen;
        private StrokeType _strokeType;

        private int _pointCount;
        private int _vertexBufferIndex;
        private int _indexBufferIndex;

        private Vector2[] _positionData;
        private Vector2[] _textureData;
        private Color[] _colorData;
        private short[] _indexData;

        private bool[] _jointCCW;

        private GraphicsPath[] _outlinePaths;

        /// <summary>
        /// Create an empty path with a given <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen"></param>
        public GraphicsPath (Pen pen)
        {
            _pen = pen;
        }

        /// <summary>
        /// Compute a stroked open path given a set of points and a <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        public GraphicsPath (Pen pen, IList<Vector2> points)
            : this(pen, points, PathType.Open, 0, points.Count)
        { }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        public GraphicsPath (Pen pen, IList<Vector2> points, PathType pathType)
            : this(pen, points, pathType, 0, points.Count)
        { }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        /// <param name="offset">The offset into the list of points that starts the path.</param>
        /// <param name="count">The number of points in the path.</param>
        public GraphicsPath (Pen pen, IList<Vector2> points, PathType pathType, int offset, int count)
            : this(pen)
        {
            _pointCount = count;
            _strokeType = StrokeType.Fill;

            switch (pathType) {
                case PathType.Open:
                    CompileOpenPath(points, offset, count, null);
                    break;

                case PathType.Closed:
                    CompileClosedPath(points, offset, count, null);
                    break;
            }
        }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a path and outline <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="outlinePen">The pen to stroke the outline of the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        public GraphicsPath (Pen pen, Pen outlinePen, IList<Vector2> points)
            : this(pen, outlinePen, points, PathType.Open, 0, points.Count, StrokeType.Both)
        { }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a path and outline <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="outlinePen">The pen to stroke the outline of the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        /// <param name="strokeType">Whether to stroke just the path, the outline, or both.</param>
        public GraphicsPath (Pen pen, Pen outlinePen, IList<Vector2> points, StrokeType strokeType)
            : this(pen, outlinePen, points, PathType.Open, 0, points.Count, strokeType)
        { }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a path and outline <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="outlinePen">The pen to stroke the outline of the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        public GraphicsPath (Pen pen, Pen outlinePen, IList<Vector2> points, PathType pathType)
            : this(pen, outlinePen, points, pathType, 0, points.Count, StrokeType.Both)
        { }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a path and outline <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="outlinePen">The pen to stroke the outline of the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        /// <param name="strokeType">Whether to stroke just the path, the outline, or both.</param>
        public GraphicsPath (Pen pen, Pen outlinePen, IList<Vector2> points, PathType pathType, StrokeType strokeType)
            : this(pen, outlinePen, points, pathType, 0, points.Count, strokeType)
        { }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a path and outline <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="outlinePen">The pen to stroke the outline of the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        /// <param name="offset">The offset into the list of points that starts the path.</param>
        /// <param name="count">The number of points in the path.</param>
        public GraphicsPath (Pen pen, Pen outlinePen, IList<Vector2> points, PathType pathType, int offset, int count)
            : this(pen, outlinePen, points, pathType, offset, count, StrokeType.Both)
        { }

        /// <summary>
        /// Compute a stroked open or closed path given a set of points and a path and outline <see cref="Pen"/>.
        /// </summary>
        /// <param name="pen">The pen to stroke the path with.</param>
        /// <param name="outlinePen">The pen to stroke the outline of the path with.</param>
        /// <param name="points">The points making up the ideal path.</param>
        /// <param name="pathType">Whether the path is open or closed.</param>
        /// <param name="offset">The offset into the list of points that starts the path.</param>
        /// <param name="count">The number of points in the path.</param>
        /// <param name="strokeType">Whether to stroke just the path, the outline, or both.</param>
        public GraphicsPath (Pen pen, Pen outlinePen, IList<Vector2> points, PathType pathType, int offset, int count, StrokeType strokeType)
            : this(pen)
        {
            _pointCount = count;
            _strokeType = strokeType;

            switch (pathType) {
                case PathType.Open:
                    CompileOpenPath(points, offset, count, outlinePen);
                    break;

                case PathType.Closed:
                    CompileClosedPath(points, offset, count, outlinePen);
                    break;
            }
        }

        /// <summary>
        /// Gets the outline paths that have been generated for this path.
        /// </summary>
        public GraphicsPath[] OutlinePaths
        {
            get { return _outlinePaths ?? _emptyOutlinePaths; }
        }

        #region IGraphicsPath Interface

        /// <inherit />
        public int IndexCount
        {
            get { return _indexBufferIndex; }
        }

        /// <inherit />
        public int VertexCount
        {
            get { return _vertexBufferIndex; }
        }

        /// <inherit />
        public Vector2[] VertexPositionData
        {
            get { return _positionData; }
        }

        /// <inherit />
        public Vector2[] VertexTextureData
        {
            get { return _textureData; }
        }

        /// <inherit />
        public Color[] VertexColorData
        {
            get { return _colorData; }
        }

        /// <inherit />
        public short[] IndexData
        {
            get { return _indexData; }
        }

        /// <inherit />
        public Pen Pen
        {
            get { return _pen; }
        }

        #endregion

        private void InitializeBuffers (int pointCount)
        {
            _jointCCW = new bool[pointCount];

            int vertexCount = Pen.MaximumVertexCount(pointCount);
            int indexCount = Pen.MaximumIndexCount(pointCount);

            _indexData = new short[indexCount];
            _positionData = new Vector2[vertexCount];

            if (_pen.Brush != null) {
                if (_pen.Brush.Color != null)
                    _colorData = new Color[vertexCount];

                if (_pen.Brush.Texture != null)
                    _textureData = new Vector2[vertexCount];
            }
        }

        private void CompileOpenPath (IList<Vector2> points, int offset, int count, Pen outlinePen)
        {
            if (_strokeType != StrokeType.Outline)
                InitializeBuffers(count);

            Buffer<Vector2> insetBuffer = null;
            Buffer<Vector2> outsetBuffer = null;

            if (outlinePen != null && _strokeType != StrokeType.Fill) {
                insetBuffer = Pools<Buffer<Vector2>>.Obtain();
                outsetBuffer = Pools<Buffer<Vector2>>.Obtain();

                int vCount = _positionData != null ? _positionData.Length : _pen.MaximumVertexCount(count);

                insetBuffer.EnsureCapacity(vCount);
                outsetBuffer.EnsureCapacity(vCount);
            }

            PenWorkspace ws = Pools<PenWorkspace>.Obtain();
            ws.ResetWorkspace(_pen);

            int vPrevCount = 0;
            int vNextCount = AddStartPoint(0, points[offset + 0], points[offset + 1], ws, insetBuffer);

            if (insetBuffer != null)
                Array.Reverse(insetBuffer.Data, 0, insetBuffer.Index);

            for (int i = 0; i < count - 2; i++) {
                vPrevCount = vNextCount;
                vNextCount = AddJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2], ws, insetBuffer, outsetBuffer);
                if (_strokeType != StrokeType.Outline)
                    AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[i], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[i + 1]);
            }

            vPrevCount = vNextCount;
            vNextCount = AddEndPoint(count - 1, points[offset + count - 2], points[offset + count - 1], ws, insetBuffer);
            if (_strokeType != StrokeType.Outline)
                AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[count - 2], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[count - 1]);

            if (insetBuffer != null)
                Array.Reverse(insetBuffer.Data, 0, insetBuffer.Index);

            Pools<PenWorkspace>.Release(ws);

            if (outlinePen != null && _strokeType != StrokeType.Fill) {
                Buffer<Vector2> mergedBuffer = Pools<Buffer<Vector2>>.Obtain();
                mergedBuffer.EnsureCapacity(insetBuffer.Index + outsetBuffer.Index);

                Array.Copy(insetBuffer.Data, 0, mergedBuffer.Data, 0, insetBuffer.Index);
                Array.Copy(outsetBuffer.Data, 0, mergedBuffer.Data, insetBuffer.Index, outsetBuffer.Index);

                _outlinePaths = new GraphicsPath[] {
                    new GraphicsPath(outlinePen, mergedBuffer.Data, PathType.Closed, 0, insetBuffer.Index + outsetBuffer.Index),
                };

                Pools<Buffer<Vector2>>.Release(mergedBuffer);
                Pools<Buffer<Vector2>>.Release(insetBuffer);
                Pools<Buffer<Vector2>>.Release(outsetBuffer);
            }
        }

        private void CompileClosedPath (IList<Vector2> points, int offset, int count, Pen outlinePen)
        {
            if (_strokeType != StrokeType.Outline)
                InitializeBuffers(count + 1);

            if (IsClose(points[offset], points[offset + count - 1]))
                count--;

            Buffer<Vector2> insetBuffer = null;
            Buffer<Vector2> outsetBuffer = null;

            if (outlinePen != null && _strokeType != StrokeType.Fill) {
                insetBuffer = Pools<Buffer<Vector2>>.Obtain();
                outsetBuffer = Pools<Buffer<Vector2>>.Obtain();

                int vCount = _positionData != null ? _positionData.Length : _pen.MaximumVertexCount(count);

                insetBuffer.EnsureCapacity(vCount);
                outsetBuffer.EnsureCapacity(vCount);
            }

            PenWorkspace ws = Pools<PenWorkspace>.Obtain();
            ws.ResetWorkspace(_pen);

            int vBaseIndex = _vertexBufferIndex;
            int vBaseCount = AddJoint(0, points[offset + count - 1], points[offset + 0], points[offset + 1], ws, insetBuffer, outsetBuffer);

            int vPrevCount = 0;
            int vNextCount = vBaseCount;

            for (int i = 0; i < count - 2; i++) {
                vPrevCount = vNextCount;
                vNextCount = AddJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2], ws, insetBuffer, outsetBuffer);
                if (_strokeType != StrokeType.Outline)
                    AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[i], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[i + 1]);
            }

            vPrevCount = vNextCount;
            vNextCount = AddJoint(count - 1, points[offset + count - 2], points[offset + count - 1], points[offset + 0], ws, insetBuffer, outsetBuffer);

            if (_strokeType != StrokeType.Outline) {
                AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[count - 2], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[count - 1]);
                AddSegment(_vertexBufferIndex - vNextCount, vNextCount, _jointCCW[count - 1], vBaseIndex, vBaseCount, _jointCCW[0]);
            }

            Pools<PenWorkspace>.Release(ws);

            if (outlinePen != null && _strokeType != StrokeType.Fill) {
                Array.Reverse(insetBuffer.Data, 0, insetBuffer.Index);

                _outlinePaths = new GraphicsPath[] {
                    new GraphicsPath(outlinePen, insetBuffer.Data, PathType.Closed, 0, insetBuffer.Index),
                    new GraphicsPath(outlinePen, outsetBuffer.Data, PathType.Closed, 0, outsetBuffer.Index),
                };

                Pools<Buffer<Vector2>>.Release(insetBuffer);
                Pools<Buffer<Vector2>>.Release(outsetBuffer);
            }
        }

        private bool IsClose (Vector2 a, Vector2 b)
        {
            return Math.Abs(a.X - b.X) < 0.001 && Math.Abs(a.Y - b.Y) < 0.001;
        }

        private int AddStartPoint (int pointIndex, Vector2 a, Vector2 b, PenWorkspace ws, Buffer<Vector2> positionBuffer)
        {
            return AddStartOrEndPoint(pointIndex, _pen.ComputeStartPoint(a, b, ws), ws, positionBuffer);
        }

        private int AddEndPoint (int pointIndex, Vector2 a, Vector2 b, PenWorkspace ws, Buffer<Vector2> positionBuffer)
        {
            return AddStartOrEndPoint(pointIndex, _pen.ComputeEndPoint(a, b, ws), ws, positionBuffer);
        }

        private int AddStartOrEndPoint (int pointIndex, int xyCount, PenWorkspace ws, Buffer<Vector2> positionBuffer)
        {
            if (positionBuffer != null) {
                for (int i = 0; i < xyCount; i++)
                    positionBuffer.SetNext(ws.XYBuffer[i]);
            }

            if (_strokeType == StrokeType.Outline)
                return 0;

            int baseIndex = _vertexBufferIndex;

            _vertexBufferIndex += xyCount;

            for (int i = 0; i < xyCount; i++)
                _positionData[baseIndex + i] = ws.XYBuffer[i];

            if (_colorData != null) {
                for (int i = 0; i < xyCount; i++)
                    _colorData[baseIndex + i] = _pen.ColorAt(ws.UVBuffer[i]);
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                for (int i = baseIndex; i < _vertexBufferIndex; i++) {
                    Vector2 pos = _positionData[i];
                    _textureData[i] = new Vector2(pos.X / texWidth, pos.Y / texHeight);
                }
            }

            _jointCCW[pointIndex] = true;

            return xyCount;
        }

        private int AddJoint (int pointIndex, Vector2 a, Vector2 b, Vector2 c, PenWorkspace ws, Buffer<Vector2> insetBuffer, Buffer<Vector2> outsetBuffer)
        {
            InsetOutsetCount vioCount = new InsetOutsetCount();

            switch (_pen.LineJoin) {
                case LineJoin.Miter:
                    vioCount = _pen.ComputeMiter(a, b, c, ws);
                    break;
                case LineJoin.Bevel:
                    vioCount = _pen.ComputeBevel(a, b, c, ws);
                    break;
            }

            if (insetBuffer != null) {
                for (int i = 0; i < vioCount.InsetCount; i++)
                    insetBuffer.SetNext(ws.XYInsetBuffer[i]);
            }
            if (outsetBuffer != null) {
                for (int i = 0; i < vioCount.OutsetCount; i++)
                    outsetBuffer.SetNext(ws.XYOutsetBuffer[i]);
            }

            return (_strokeType != StrokeType.Outline) ? AddJoint(pointIndex, vioCount, ws) : 0;
        }

        private int AddJoint (int pointIndex, InsetOutsetCount vioCount, PenWorkspace ws)
        {
            int vIndex = _vertexBufferIndex;

            _vertexBufferIndex += vioCount.InsetCount + vioCount.OutsetCount;

            if (!vioCount.CCW) {
                _jointCCW[pointIndex] = false;
                _positionData[vIndex + 0] = ws.XYOutsetBuffer[0];
                for (int i = 0; i < vioCount.InsetCount; i++)
                    _positionData[vIndex + 1 + i] = ws.XYInsetBuffer[i];

                for (int i = 0; i < vioCount.InsetCount - 1; i++) {
                    _indexData[_indexBufferIndex++] = (short)(vIndex);
                    _indexData[_indexBufferIndex++] = (short)(vIndex + i + 2);
                    _indexData[_indexBufferIndex++] = (short)(vIndex + i + 1);
                }
            }
            else {
                _jointCCW[pointIndex] = true;
                _positionData[vIndex + 0] = ws.XYInsetBuffer[0];
                for (int i = 0; i < vioCount.OutsetCount; i++)
                    _positionData[vIndex + 1 + i] = ws.XYOutsetBuffer[i];

                for (int i = 0; i < vioCount.OutsetCount - 1; i++) {
                    _indexData[_indexBufferIndex++] = (short)(vIndex);
                    _indexData[_indexBufferIndex++] = (short)(vIndex + i + 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndex + i + 2);
                }
            }

            if (_colorData != null) {
                if (!vioCount.CCW) {
                    _colorData[vIndex] = _pen.ColorAt(ws.UVOutsetBuffer[0]);
                    for (int i = 0; i < vioCount.InsetCount; i++)
                        _colorData[vIndex + 1 + i] = _pen.ColorAt(ws.UVInsetBuffer[i]);
                }
                else {
                    _colorData[vIndex] = _pen.ColorAt(ws.UVInsetBuffer[0]);
                    for (int i = 0; i < vioCount.OutsetCount; i++)
                        _colorData[vIndex + 1 + i] = _pen.ColorAt(ws.UVOutsetBuffer[i]);
                }
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                for (int i = vIndex; i < _vertexBufferIndex; i++) {
                    Vector2 pos = _positionData[i];
                    _textureData[i] = new Vector2(pos.X / texWidth, pos.Y / texHeight);
                }
            }

            return _vertexBufferIndex - vIndex;
        }

        private void AddSegment (int vIndexStart, int vStartCount, bool vStartCCW, int vIndexEnd, int vEndCount, bool vEndCCW)
        {
            if (vStartCCW) {
                if (vEndCCW) {
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + vStartCount - 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + 0);
                }
                else {
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + vStartCount - 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + 0);
                }
            }
            else {
                if (vEndCCW) {
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + vStartCount - 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + vStartCount - 1);
                }
                else {
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + vStartCount - 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 0);
                    _indexData[_indexBufferIndex++] = (short)(vIndexEnd + 1);
                    _indexData[_indexBufferIndex++] = (short)(vIndexStart + vStartCount - 1);
                }
            }
        }
    }
}
