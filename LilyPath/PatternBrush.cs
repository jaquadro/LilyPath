using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LilyPath
{
    /// <summary>
    /// A <see cref="Brush"/> that represents a texture.
    /// </summary>
    public class PatternBrush : Brush
    {
        /// <summary>
        /// Creates a new <see cref="PatternBrush"/> with the given <see cref="GraphicsDevice"/> and texture.
        /// </summary>
        /// <param name="pattern">A texture.</param>
        public PatternBrush (Texture2D pattern)
            : this(pattern, 1f)
        { }

        /// <summary>
        /// Creates a new <see cref="PatternBrush"/> with the given <see cref="GraphicsDevice"/>, texture, and opacity.
        /// </summary>
        /// <param name="pattern">A texture.</param>
        /// <param name="opacity">The opacity to render the texture with.</param>
        /// <remarks>The <see cref="Brush.Alpha"/> property of the brush is intialized to the opacity value.
        /// When the brush is rendered, any opacity already present in the texture is blended with
        /// the opacity value.</remarks>
        public PatternBrush (Texture2D pattern, float opacity)
            : base()
        {
            Alpha = opacity;
            Texture = pattern;
        }

        /// <summary>
        /// Gets or sets the texture resource of the brush.
        /// </summary>
        public new Texture2D Texture
        {
            get { return base.Texture; }
            private set { base.Texture = value; }
        }

        /// <summary>
        /// Gets or sets the transformation to apply to brush.
        /// </summary>
        public new Matrix Transform
        {
            get { return base.Transform; }
            set { base.Transform = value; }
        }

        /// <summary>
        /// Gets or sets whether this brush "owns" the texture used to construct it, and should therefor dispose the texture
        /// along with itself.
        /// </summary>
        public bool OwnsTexture { get; set; }

        /// <inherit />
        protected override void DisposeManaged ()
        {
            if (OwnsTexture)
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
