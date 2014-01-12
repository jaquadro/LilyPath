using Microsoft.Xna.Framework;

namespace LilyPath.Pens
{
    /// <summary>
    /// A <see cref="Pen"/> that blends two colors across the length of the stroked path.
    /// </summary>
    public class PathGradientPen : Pen
    {
        private Color _color1;
        private Color _color2;

        /// <summary>
        /// Creates a new <see cref="GradientPen"/> with the given colors and width.
        /// </summary>
        /// <param name="startColor">The starting pen color.</param>
        /// <param name="endColor">The ending pen color.</param>
        /// <param name="width">The width of the paths drawn by the pen.</param>
        public PathGradientPen (Color startColor, Color endColor, float width)
            : base(Color.White, width)
        {
            _color1 = startColor;
            _color2 = endColor;
        }

        /// <summary>
        /// Creates a new <see cref="GradientPen"/> with the given colors and a width of 1.
        /// </summary>
        /// <param name="startColor">The starting pen color.</param>
        /// <param name="endColor">The ending pen color.</param>
        public PathGradientPen (Color startColor, Color endColor)
            : this(startColor, endColor, 1)
        { }

        /// <InheritDoc />
        public override bool NeedsPathLength
        {
            get { return true; }
        }

        /// <InheritDoc />
        protected internal override Color ColorAt (float widthPosition, float lengthPosition, float lengthScale)
        {
            return Color.Lerp(_color1, _color2, lengthPosition * lengthScale);
        }
    }
}
