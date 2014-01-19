using Microsoft.Xna.Framework;
using System;

namespace LilyPath.Utility
{
    internal struct JoinSample
    {
        public Vector2 PointA;
        public Vector2 PointB;
        public Vector2 PointC;

        public float LengthA;
        public float LengthB;
        public float LengthC;

        public JoinSample (Vector2 pointA, Vector2 pointB, Vector2 pointC)
        {
            PointA = pointA;
            PointB = pointB;
            PointC = pointC;

            LengthA = 0;
            LengthB = 0;
            LengthC = 0;
        }

        public JoinSample (Vector2 pointA, Vector2 pointB, Vector2 pointC, float lengthA, float lengthB, float lengthC)
        {
            PointA = pointA;
            PointB = pointB;
            PointC = pointC;

            LengthA = lengthA;
            LengthB = lengthB;
            LengthC = lengthC;
        }

        public void Advance (Vector2 nextPoint)
        {
            PointA = PointB;
            PointB = PointC;
            PointC = nextPoint;
        }

        public void Advance (Vector2 nextPoint, float nextLength)
        {
            PointA = PointB;
            PointB = PointC;
            PointC = nextPoint;

            LengthA = LengthB;
            LengthB = LengthC;
            LengthC = nextLength;
        }
    }

    internal class PenWorkspace
    {
        private float _pathLength;
        private float _pathLengthScale;

        public Buffer<Vector2> XYBuffer;
        public Buffer<Vector2> XYInsetBuffer;
        public Buffer<Vector2> XYOutsetBuffer;

        public Buffer<Vector2> UVBuffer;
        public Buffer<Vector2> UVInsetBuffer;
        public Buffer<Vector2> UVOutsetBuffer;

        public Buffer<short> IndexBuffer;
        public Buffer<short> OutlineIndexBuffer;

        public Vector2[] BoundingQuad;

        public float PathLength
        {
            get { return _pathLength; }
            set
            {
                _pathLength = value;
                _pathLengthScale = (value == 0) ? 1 : 1 / _pathLength;
            }
        }

        public float PathLengthScale
        {
            get { return _pathLengthScale; }
        }

        public PenWorkspace ()
        {
            XYBuffer = new Buffer<Vector2>();
            XYInsetBuffer = new Buffer<Vector2>();
            XYOutsetBuffer = new Buffer<Vector2>();

            UVBuffer = new Buffer<Vector2>();
            UVInsetBuffer = new Buffer<Vector2>();
            UVOutsetBuffer = new Buffer<Vector2>();

            IndexBuffer = new Buffer<short>();
            OutlineIndexBuffer = new Buffer<short>();

            BoundingQuad = new Vector2[4];
        }

        public PenWorkspace (Pen pen)
        {
            XYBuffer = new Buffer<Vector2>(Math.Max(pen.StartPointVertexBound(), pen.EndPointVertexBound()));
            XYInsetBuffer = new Buffer<Vector2>(pen.LineJoinVertexBound());
            XYOutsetBuffer = new Buffer<Vector2>(XYInsetBuffer.Capacity);

            UVBuffer = new Buffer<Vector2>(XYBuffer.Capacity);
            UVInsetBuffer = new Buffer<Vector2>(XYInsetBuffer.Capacity);
            UVOutsetBuffer = new Buffer<Vector2>(XYOutsetBuffer.Capacity);

            IndexBuffer = new Buffer<short>(Math.Max(pen.StartCapInfo.IndexCount, pen.EndCapInfo.IndexCount));
            OutlineIndexBuffer = new Buffer<short>(XYBuffer.Capacity);

            BoundingQuad = new Vector2[4];
        }

        public void ResetWorkspace (Pen pen)
        {
            XYBuffer.EnsureCapacity(Math.Max(pen.StartPointVertexBound(), pen.EndPointVertexBound()));
            XYInsetBuffer.EnsureCapacity(pen.LineJoinVertexBound());
            XYOutsetBuffer.EnsureCapacity(XYInsetBuffer.Capacity);

            UVBuffer.EnsureCapacity(XYBuffer.Capacity);
            UVInsetBuffer.EnsureCapacity(XYInsetBuffer.Capacity);
            UVOutsetBuffer.EnsureCapacity(XYOutsetBuffer.Capacity);

            IndexBuffer.EnsureCapacity(Math.Max(pen.StartCapInfo.IndexCount, pen.EndCapInfo.IndexCount));
            OutlineIndexBuffer.EnsureCapacity(XYBuffer.Capacity);

            PathLength = 0;
        }
    }
}
