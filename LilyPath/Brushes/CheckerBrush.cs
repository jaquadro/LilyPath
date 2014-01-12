using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath.Brushes
{
    /// <summary>
    /// A <see cref="Brush"/> that represents a two-color checkered texture.
    /// </summary>
    public class CheckerBrush : TextureBrush
    {
        private Color _color1;
        private Color _color2;
        private int _width;
        private int _height;

        /// <summary>
        /// Creates a new <see cref="CheckerBrush"/> with the given <see cref="GraphicsDevice"/>, colors, and square cell size.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/> that should be used to create a texture.</param>
        /// <param name="color1">The first checker color.</param>
        /// <param name="color2">The second checker color.</param>
        /// <param name="width">The size of the width and height of a single colored square.</param>
        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width)
            : this(device, color1, color2, width, width, 1f)
        { }

        /// <summary>
        /// Creates a new <see cref="CheckerBrush"/> with the given <see cref="GraphicsDevice"/>, colors, square cell size, and opacity.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/> that should be used to create a texture.</param>
        /// <param name="color1">The first checker color.</param>
        /// <param name="color2">The second checker color.</param>
        /// <param name="width">The size of the width and height of a single colored square.</param>
        /// <param name="opacity">The opacity to render the texture with.</param>
        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width, float opacity)
            : this(device, color1, color2, width, width, opacity)
        { }

        /// <summary>
        /// Creates a new <see cref="CheckerBrush"/> with the given <see cref="GraphicsDevice"/>, colors, and cell dimensions.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/> that should be used to create a texture.</param>
        /// <param name="color1">The first checker color.</param>
        /// <param name="color2">The second checker color.</param>
        /// <param name="width">The width of a single colored cell.</param>
        /// <param name="height">The height of a single colored cell.</param>
        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width, int height)
            : this(device, color1, color2, width, height, 1f)
        { }

        /// <summary>
        /// Creates a new <see cref="CheckerBrush"/> with the given <see cref="GraphicsDevice"/>, colors, cell dimensions, and opacity.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/> that should be used to create a texture.</param>
        /// <param name="color1">The first checker color.</param>
        /// <param name="color2">The second checker color.</param>
        /// <param name="width">The width of a single colored cell.</param>
        /// <param name="height">The height of a single colored cell.</param>
        /// <param name="opacity">The opacity to render the texture with.</param>
        public CheckerBrush (GraphicsDevice device, Color color1, Color color2, int width, int height, float opacity)
            : base(BuildCheckerTexture(device, color1, color2, width, height), opacity)
        {
            _color1 = color1;
            _color2 = color2;
            _width = width;
            _height = height;

            OwnsTexture = true;
            device.DeviceReset += HandleGraphicsDeviceReset;
        }

        /// <InheritDoc />
        protected override void DisposeManaged ()
        {
            if (Texture != null && Texture.GraphicsDevice != null)
                Texture.GraphicsDevice.DeviceReset -= HandleGraphicsDeviceReset;

            base.DisposeManaged();
        }

        private void HandleGraphicsDeviceReset (object sender, EventArgs e)
        {
            GraphicsDevice device = sender as GraphicsDevice;
            if (device == null)
                return;

            Texture = BuildCheckerTexture(device, _color1, _color2, _width, _height);
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
}
