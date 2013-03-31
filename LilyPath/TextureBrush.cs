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
    public class TextureBrush : Brush
    {
        /// <summary>
        /// Creates a new <see cref="TextureBrush"/> with the given <see cref="GraphicsDevice"/> and texture.
        /// </summary>
        /// <param name="texture">A texture.</param>
        public TextureBrush (Texture2D texture)
            : this(texture, 1f)
        { }

        /// <summary>
        /// Creates a new <see cref="TextureBrush"/> with the given <see cref="GraphicsDevice"/>, texture, and opacity.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="opacity">The opacity to render the texture with.</param>
        /// <remarks>The <see cref="Brush.Alpha"/> property of the brush is intialized to the opacity value.
        /// When the brush is rendered, any opacity already present in the texture is blended with
        /// the opacity value.</remarks>
        public TextureBrush (Texture2D texture, float opacity)
            : base()
        {
            Alpha = opacity;
            Texture = texture;
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
        /// Gets or sets the color to blend with the texture.
        /// </summary>
        public new Color Color
        {
            get { return base.Color; }
            set { base.Color = value; }
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

    public class CheckerBrush : TextureBrush
    {
        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width)
            : this(device, color1, color2, width, width, 1f)
        { }

        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width, float opacity)
            : this(device, color1, color2, width, width, opacity)
        { }

        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width1, int width2)
            : this(device, color1, color2, width1, width2, 1f)
        { }

        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width1, int width2, float opacity)
            : base(BuildCheckerTexture(device, color1, color2, width1, width2), opacity)
        {
            OwnsTexture = true;
        }

        private static Texture2D BuildCheckerTexture (GraphicsDevice device, Color color1, Color color2, int blockWidth, int blockHeight)
        {
            int width = blockWidth * 2;
            int height = blockHeight * 2;

            byte[] data = new byte[width * height * 4];
            for (int y = 0; y < height / 2; y++)
                for (int x = 0; x < width / 2; x++)
                    SetColor(data, width, x, y, color1);

            for (int y = 0; y < height / 2; y++)
                for (int x = width / 2; x < width; x++)
                    SetColor(data, width, x, y, color2);

            for (int y = width / 2; y < height; y++)
                for (int x = 0; x < width / 2; x++)
                    SetColor(data, width, x, y, color2);

            for (int y = width / 2; y < height; y++)
                for (int x = width / 2; x < width; x++)
                    SetColor(data, width, x, y, color1);

            Texture2D tex = new Texture2D(device, width, height, false, SurfaceFormat.Color);
            tex.SetData(data);

            return tex;
        }

        private static void SetColor (byte[] data, int width, int x, int y, Color color)
        {
            int index = (y * width + x) * 4;

            data[index + 0] = color.R;
            data[index + 1] = color.G;
            data[index + 2] = color.B;
            data[index + 3] = color.A;
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
