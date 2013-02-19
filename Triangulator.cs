using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LilyPath
{
    public class Triangulator
    {
        private int[] _triPrev = new int[128];
        private int[] _triNext = new int[128];

        private int[] _indexComputeBuffer = new int[128];
        private int _indexCount = 0;

        public int[] ComputedIndexes
        {
            get { return _indexComputeBuffer; }
        }

        public int ComputedIndexCount
        {
            get { return _indexCount; }
        }

        public void Triangulate (IList<Vector2> points, int offset, int count)
        {
            Initialize(count);

            int index = 0;
            int computeIndex = 0;
            while (count >= 3) {
                bool isEar = true;

                Vector2 a = points[offset + _triPrev[index]];
                Vector2 b = points[offset + index];
                Vector2 c = points[offset + _triNext[index]];
                if (TriangleIsCCW(a, b, c)) {
                    int k = _triNext[_triNext[index]];
                    do {
                        if (PointInTriangleInclusive(points[offset + k], a, b, c)) {
                            isEar = false;
                            break;
                        }
                        k = _triNext[k];
                    } while (k != _triPrev[index]);
                }
                else {
                    isEar = false;
                }

                if (isEar) {
                    if (_indexComputeBuffer.Length < computeIndex + 3)
                        Array.Resize(ref _indexComputeBuffer, _indexComputeBuffer.Length * 2);

                    _indexComputeBuffer[computeIndex++] = offset + _triPrev[index];
                    _indexComputeBuffer[computeIndex++] = offset + index;
                    _indexComputeBuffer[computeIndex++] = offset + _triNext[index];

                    _triNext[_triPrev[index]] = _triNext[index];
                    _triPrev[_triNext[index]] = _triPrev[index];
                    count--;
                    index = _triPrev[index];
                }
                else {
                    index = _triNext[index];
                }
            }

            _indexCount = computeIndex;
        }

        private void Initialize (int count)
        {
            _indexCount = 0;

            if (_triNext.Length < count)
                Array.Resize(ref _triNext, Math.Max(_triNext.Length * 2, count));
            if (_triPrev.Length < count)
                Array.Resize(ref _triPrev, Math.Min(_triPrev.Length * 2, count));

            for (int i = 0; i < count; i++) {
                _triPrev[i] = i - 1;
                _triNext[i] = i + 1;
            }

            _triPrev[0] = count - 1;
            _triNext[count - 1] = 0;
        }

        private float Cross2D (Vector2 u, Vector2 v)
        {
            return (u.Y * v.X) - (u.X * v.Y);
        }

        private bool PointInTriangleInclusive (Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            if (Cross2D(point - a, b - a) <= 0f)
                return false;
            if (Cross2D(point - b, c - b) <= 0f)
                return false;
            if (Cross2D(point - c, a - c) <= 0f)
                return false;
            return true;
        }

        private bool TriangleIsCCW (Vector2 a, Vector2 b, Vector2 c)
        {
            return Cross2D(b - a, c - b) < 0;
        }
    }
}
