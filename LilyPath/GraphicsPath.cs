using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
        private int _vertexCount;
        private int _indexCount;

        private Vector2[] _positionData;
        private Vector2[] _textureData;
        private Color[] _colorData;
        private short[] _indexData;

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
            get { return _indexCount; }
        }

        /// <inherit />
        public int VertexCount
        {
            get { return _vertexCount; }
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

        private void InitializeBuffers (int vertexCount, int indexCount)
        {
            _vertexCount = vertexCount;
            _indexCount = indexCount;

            _indexData = new short[indexCount];
            _positionData = new Vector2[vertexCount];

            if (_pen.Color != null)
                _colorData = new Color[vertexCount];

            if (_pen.Brush != null && _pen.Brush.Texture != null)
                _textureData = new Vector2[vertexCount];
        }

        private void CompileOpenPath (IList<Vector2> points, int offset, int count)
        {
            InitializeBuffers(count * 2, (count - 1) * 6);

            AddStartPoint(0, points[offset + 0], points[offset + 1]);

            for (int i = 0; i < count - 2; i++) {
                AddMiteredJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2]);
                AddSegment(i, i + 1);
            }

            AddEndPoint(count - 1, points[offset + count - 2], points[offset + count - 1]);
            AddSegment(count - 2, count - 1);
        }

        private void CompileClosedPath (IList<Vector2> points, int offset, int count)
        {
            InitializeBuffers(count * 2, count * 6);

            AddMiteredJoint(0, points[offset + count - 1], points[offset + 0], points[offset + 1]);

            for (int i = 0; i < count - 2; i++) {
                AddMiteredJoint(i + 1, points[offset + i], points[offset + i + 1], points[offset + i + 2]);
                AddSegment(i, i + 1);
            }

            AddMiteredJoint(count - 1, points[offset + count - 2], points[offset + count - 1], points[offset + 0]);
            AddSegment(count - 2, count - 1);

            AddSegment(count - 1, 0);
        }

        private void AddStartPoint (int pointIndex, Vector2 a, Vector2 b)
        {
            short vIndex = (short)(pointIndex * 2);

            _pen.ComputeStartPoint(_positionData, vIndex, a, b);

            if (_colorData != null) {
                _colorData[vIndex + 0] = _pen.Color;
                _colorData[vIndex + 1] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                Vector2 pos0 = _positionData[vIndex + 0];
                Vector2 pos1 = _positionData[vIndex + 1];

                _textureData[vIndex + 0] = new Vector2(pos0.X / texWidth, pos0.Y / texHeight);
                _textureData[vIndex + 1] = new Vector2(pos1.X / texWidth, pos1.Y / texHeight);
            }
        }

        private void AddEndPoint (int pointIndex, Vector2 a, Vector2 b)
        {
            short vIndex = (short)(pointIndex * 2);

            _pen.ComputeEndPoint(_positionData, vIndex, a, b);

            if (_colorData != null) {
                _colorData[vIndex + 0] = _pen.Color;
                _colorData[vIndex + 1] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                Vector2 pos0 = _positionData[vIndex + 0];
                Vector2 pos1 = _positionData[vIndex + 1];

                _textureData[vIndex + 0] = new Vector2(pos0.X / texWidth, pos0.Y / texHeight);
                _textureData[vIndex + 1] = new Vector2(pos1.X / texWidth, pos1.Y / texHeight);
            }
        }

        private void AddMiteredJoint (int pointIndex, Vector2 a, Vector2 b, Vector2 c)
        {
            short vIndex = (short)(pointIndex * 2);

            _pen.ComputeMiter(_positionData, vIndex, a, b, c);

            if (_colorData != null) {
                _colorData[vIndex + 0] = _pen.Color;
                _colorData[vIndex + 1] = _pen.Color;
            }

            if (_textureData != null) {
                int texWidth = _pen.Brush.Texture.Width;
                int texHeight = _pen.Brush.Texture.Height;

                Vector2 pos0 = _positionData[vIndex + 0];
                Vector2 pos1 = _positionData[vIndex + 1];

                _textureData[vIndex + 0] = new Vector2(pos0.X / texWidth, pos0.Y / texHeight);
                _textureData[vIndex + 1] = new Vector2(pos1.X / texWidth, pos1.Y / texHeight);
            }
        }

        private void AddSegment (int startPoint, int endPoint)
        {
            short vIndexStart = (short)(startPoint * 2);
            short vIndexEnd = (short)(endPoint * 2);
            short iIndex = (short)(startPoint * 6);

            _indexData[iIndex + 0] = (short)(vIndexStart + 0);
            _indexData[iIndex + 1] = (short)(vIndexStart + 1);
            _indexData[iIndex + 2] = (short)(vIndexEnd + 1);
            _indexData[iIndex + 3] = (short)(vIndexStart + 0);
            _indexData[iIndex + 4] = (short)(vIndexEnd + 1);
            _indexData[iIndex + 5] = (short)(vIndexEnd + 0);
        }
    }
}
