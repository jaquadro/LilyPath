using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LilyPath
{
    public class PatternBrush : Brush
    {
        public PatternBrush (GraphicsDevice device, Texture2D pattern)
            : this(device, pattern, 1f)
        {
        }

        public PatternBrush (GraphicsDevice device, Texture2D pattern, float opacity)
            : base()
        {
            Alpha = opacity;

            byte[] data = new byte[pattern.Width * pattern.Height * 4];
            pattern.GetData(data);

            Texture = new Texture2D(device, pattern.Width, pattern.Height, false, SurfaceFormat.Color);
            Texture.SetData(data);
        }

        protected override void DisposeManaged ()
        {
            Texture.Dispose();
        }
    }

    /*public class StippleBrush : PatternBrush
    {
        public StippleBrush (GraphicsDevice device, bool[,] pattern, Color color)
            : this(device, pattern, color, 1f)
        {
        }

        public StippleBrush (GraphicsDevice device, bool[,] pattern, Color color, float opacity)
            : base(device, BuildStipplePattern(device, pattern, color), opacity)
        {
        }

        private static Texture2D BuildStipplePattern (GraphicsDevice device, bool[,] pattern, Color color)
        {
            int h = pattern.GetUpperBound(0);
            int w = pattern.GetUpperBound(1);

            byte[] data = new byte[w * h * 4];
            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    Color c = color;
                    if (!pattern[y, x])
                        c = new Color(0, 0, 0, 0);

                    int index = (y * w + x) * 4;
                    data[index + 0] = c.A;
                    data[index + 1] = c.R;
                    data[index + 2] = c.G;
                    data[index + 3] = c.B;
                }
            }

            Texture2D tex = new Texture2D(device, w, h, false, SurfaceFormat.Color);
            tex.SetData(data);

            return tex;
        }
    }*/
}
