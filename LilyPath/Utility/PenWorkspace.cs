using Microsoft.Xna.Framework;
using System;

namespace LilyPath.Utility
{
    internal class PenWorkspace
    {
        public Buffer<Vector2> XYBuffer;
        public Buffer<Vector2> XYInsetBuffer;
        public Buffer<Vector2> XYOutsetBuffer;

        public Buffer<Vector2> UVBuffer;
        public Buffer<Vector2> UVInsetBuffer;
        public Buffer<Vector2> UVOutsetBuffer;

        public PenWorkspace ()
        {
            XYBuffer = new Buffer<Vector2>();
            XYInsetBuffer = new Buffer<Vector2>();
            XYOutsetBuffer = new Buffer<Vector2>();

            UVBuffer = new Buffer<Vector2>();
            UVInsetBuffer = new Buffer<Vector2>();
            UVOutsetBuffer = new Buffer<Vector2>();
        }

        public PenWorkspace (Pen pen)
        {
            XYBuffer = new Buffer<Vector2>(Math.Max(pen.StartPointVertexBound(), pen.EndPointVertexBound()));
            XYInsetBuffer = new Buffer<Vector2>(pen.LineJoinVertexBound());
            XYOutsetBuffer = new Buffer<Vector2>(XYInsetBuffer.Capacity);

            UVBuffer = new Buffer<Vector2>(XYBuffer.Capacity);
            UVInsetBuffer = new Buffer<Vector2>(XYInsetBuffer.Capacity);
            UVOutsetBuffer = new Buffer<Vector2>(XYOutsetBuffer.Capacity);
        }

        public void ResetWorkspace (Pen pen)
        {
            XYBuffer.EnsureCapacity(Math.Max(pen.StartPointVertexBound(), pen.EndPointVertexBound()));
            XYInsetBuffer.EnsureCapacity(pen.LineJoinVertexBound());
            XYOutsetBuffer.EnsureCapacity(XYInsetBuffer.Capacity);

            UVBuffer.EnsureCapacity(XYBuffer.Capacity);
            UVInsetBuffer.EnsureCapacity(XYInsetBuffer.Capacity);
            UVOutsetBuffer.EnsureCapacity(XYOutsetBuffer.Capacity);
        }
    }
}
