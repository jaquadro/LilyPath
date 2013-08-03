using Microsoft.Xna.Framework;

namespace LilyPath.Pens
{
    /// <summary>
    /// A <see cref="Pen"/> that blends two colors across its stroke width.
    /// </summary>
    public class GradientPen : Pen
    {
        private Color _color1;
        private Color _color2;

        /// <summary>
        /// Creates a new <see cref="GradientPen"/> with the given colors and width.
        /// </summary>
        /// <param name="color1">The first pen color.</param>
        /// <param name="color2">The second pen color.</param>
        /// <param name="width">The width of the paths drawn by the pen.</param>
        public GradientPen (Color color1, Color color2, float width)
            : base(Color.White, width)
        {
            _color1 = color1;
            _color2 = color2;
        }

        /// <summary>
        /// Creates a new <see cref="GradientPen"/> with the given colors and a width of 1.
        /// </summary>
        /// <param name="color1">The first pen color.</param>
        /// <param name="color2">The second pen color.</param>
        public GradientPen (Color color1, Color color2)
            : this(color1, color2, 1)
        { }

        /// <InheritDoc />
        protected internal override Color ColorAt (float widthPosition, float lengthPosition)
        {
            return Color.Lerp(_color1, _color2, widthPosition);
        }
    }
}
