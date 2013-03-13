using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

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
        private Pen _pen;

        private int _pointCount;
        private int _vertexBufferIndex;
        private int _indexBufferIndex;

        private Vector2[] _positionData;
        private Vector2[] _textureData;
        private Color[] _colorData;
        private short[] _indexData;

        private bool[] _jointCCW;

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

            switch (pathType) {
                case PathType.Open:
                    CompileOpenPath(points, offset, count);
                    break;

                case PathType.Closed:
                    CompileClosedPath(points, offset, count);
                    break;
            }
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

        /*private void CheckBufferFreeSpace (int additionalPoints)
        {
            int maxAdditionalVertexCount = Pen.Ma
            if (_geometryBuffer.Length < _geometryIndex + vertexCount)
                Array.Resize(ref _geometryBuffer, (_geometryIndex + vertexCount) * 2);
        }*/

        private void CompileOpenPath (IList<Vector2> points, int offset, int count)
        {
            InitializeBuffers(count);

            int vPrevCount = 0;
            int vNextCount = AddStartPoint(0, points[offset + 0], points[offset + 1]); ;

            for (int i = 0; i < count - 2; i++) {
                vPrevCount = vNextCount;
                vNextCount = AddJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2]);
                AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[i], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[i + 1]);
            }

            vPrevCount = vNextCount;
            vNextCount = AddEndPoint(count - 1, points[offset + count - 2], points[offset + count - 1]);
            AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[count - 2], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[count - 1]);
        }

        private void CompileClosedPath (IList<Vector2> points, int offset, int count)
        {
            InitializeBuffers(count + 1);

            if (IsClose(points[offset], points[offset + count - 1]))
                count--;

            int vBaseIndex = _vertexBufferIndex;
            int vBaseCount = AddJoint(0, points[offset + count - 1], points[offset + 0], points[offset + 1]);

            int vPrevCount = 0;
            int vNextCount = vBaseCount;

            for (int i = 0; i < count - 2; i++) {
                vPrevCount = vNextCount;
                vNextCount = AddJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2]);
                AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[i], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[i + 1]);
            }

            vPrevCount = vNextCount;
            vNextCount = AddJoint(count - 1, points[offset + count - 2], points[offset + count - 1], points[offset + 0]);
            AddSegment(_vertexBufferIndex - vNextCount - vPrevCount, vPrevCount, _jointCCW[count - 2], _vertexBufferIndex - vNextCount, vNextCount, _jointCCW[count - 1]);

            AddSegment(_vertexBufferIndex - vNextCount, vNextCount, _jointCCW[count - 1], vBaseIndex, vBaseCount, _jointCCW[0]);
        }

        private bool IsClose (Vector2 a, Vector2 b)
        {
            return Math.Abs(a.X - b.X) < 0.001 && Math.Abs(a.Y - b.Y) < 0.001;
        }

        private int AddStartPoint (int pointIndex, Vector2 a, Vector2 b)
        {
            int vIndex = _vertexBufferIndex;

            _vertexBufferIndex += _pen.ComputeStartPoint(_positionData, vIndex, a, b);
            if (_colorData != null) {
                for (int i = vIndex; i < _vertexBufferIndex; i++)
                    _colorData[i] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                for (int i = vIndex; i < _vertexBufferIndex; i++) {
                    Vector2 pos = _positionData[i];
                    _textureData[i] = new Vector2(pos.X / texWidth, pos.Y / texHeight);
                }
            }

            _jointCCW[pointIndex] = true;

            return _vertexBufferIndex - vIndex;
        }

        private int AddEndPoint (int pointIndex, Vector2 a, Vector2 b)
        {
            int vIndex = _vertexBufferIndex;

            _vertexBufferIndex += _pen.ComputeEndPoint(_positionData, vIndex, a, b);
            if (_colorData != null) {
                for (int i = vIndex; i < _vertexBufferIndex; i++)
                    _colorData[i] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                for (int i = vIndex; i < _vertexBufferIndex; i++) {
                    Vector2 pos = _positionData[i];
                    _textureData[i] = new Vector2(pos.X / texWidth, pos.Y / texHeight);
                }
            }

            _jointCCW[pointIndex] = true;

            return _vertexBufferIndex - vIndex;
        }

        private int AddJoint (int pointIndex, Vector2 a, Vector2 b, Vector2 c)
        {
            switch (Pen.LineJoin) {
                case LineJoin.Miter:
                    return AddMiteredJoint(pointIndex, a, b, c);
                case LineJoin.Bevel:
                    return AddBeveledJoint(pointIndex, a, b, c);
                default:
                    return 0;
            }
        }

        private int AddMiteredJoint (int pointIndex, Vector2 a, Vector2 b, Vector2 c)
        {
            int vIndex = _vertexBufferIndex;

            int vertexGen = _pen.ComputeMiter(_positionData, vIndex, a, b, c);

            _vertexBufferIndex += Math.Abs(vertexGen);
            if (_colorData != null) {
                for (int i = vIndex; i < _vertexBufferIndex; i++)
                    _colorData[i] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                for (int i = vIndex; i < _vertexBufferIndex; i++) {
                    Vector2 pos = _positionData[i];
                    _textureData[i] = new Vector2(pos.X / texWidth, pos.Y / texHeight);
                }
            }

            if (vertexGen > 0) {
                _jointCCW[pointIndex] = true;
                for (int i = 0; i < vertexGen - 2; i++) {
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 1);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 2);
                }
            }
            else if (vertexGen < 0) {
                _jointCCW[pointIndex] = false;
                vertexGen *= -1;
                for (int i = 0; i < vertexGen - 2; i++) {
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 2);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 1);
                }
            }
            else {
                _jointCCW[pointIndex] = true;
            }

            return _vertexBufferIndex - vIndex;
        }

        private int AddBeveledJoint (int pointIndex, Vector2 a, Vector2 b, Vector2 c)
        {
            int vIndex = _vertexBufferIndex;

            int vertexGen = _pen.ComputeBevel(_positionData, vIndex, a, b, c);

            _vertexBufferIndex += Math.Abs(vertexGen);
            if (_colorData != null) {
                for (int i = vIndex; i < _vertexBufferIndex; i++)
                    _colorData[i] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                for (int i = vIndex; i < _vertexBufferIndex; i++) {
                    Vector2 pos = _positionData[i];
                    _textureData[i] = new Vector2(pos.X / texWidth, pos.Y / texHeight);
                }
            }

            if (vertexGen > 0) {
                _jointCCW[pointIndex] = true;
                for (int i = 0; i < vertexGen - 2; i++) {
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 1);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 2);
                }
            }
            else if (vertexGen < 0) {
                _jointCCW[pointIndex] = false;
                vertexGen *= -1;
                for (int i = 0; i < vertexGen - 2; i++) {
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 2);
                    _indexData[_indexBufferIndex++] = (short)(_vertexBufferIndex - vertexGen + i + 1);
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
