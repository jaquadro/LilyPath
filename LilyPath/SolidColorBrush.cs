using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    public class SolidColorBrush : Brush
    {
        public Color Color { get; private set; }

        public SolidColorBrush (GraphicsDevice device, Color color)
            : base()
        {
            Alpha = color.A / 255f;
            Color = color;

            color = new Color(color.R, color.G, color.B, 255);

            Texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new Color[] { color });
        }

        protected override void DisposeManaged ()
        {
            Texture.Dispose();
        }
    }
}
