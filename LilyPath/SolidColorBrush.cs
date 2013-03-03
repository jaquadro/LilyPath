using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    /// <summary>
    /// A <see cref="Brush"/> that represents a solid color.
    /// </summary>
    public class SolidColorBrush : Brush
    {
        /// <summary>
        /// The color of the brush.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Creates a new <see cref="SolidColorBrush"/> from the given <see cref="GraphicsDevice"/> and <see cref="Color"/>.
        /// </summary>
        /// <param name="device">A valid <see cref="GraphicsDevice"/>.</param>
        /// <param name="color">A color.</param>
        /// <remarks>The <see cref="Brush.Alpha"/> property of the brush is initialized 
        /// to the alpha value of the color.</remarks>
        public SolidColorBrush (GraphicsDevice device, Color color)
            : base()
        {
            Alpha = color.A / 255f;
            Color = color;

            color = new Color(color.R, color.G, color.B, 255);

            Texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new Color[] { color });
        }

        /// <inherit />
        protected override void DisposeManaged ()
        {
            Texture.Dispose();
        }
    }
}
