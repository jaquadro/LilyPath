using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    /// <summary>
    /// Objects used to fill the interiors of shapes and paths.
    /// </summary>
    public abstract class Brush : IDisposable
    {
        #region Default Brushes

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Black { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Blue { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Cyan { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Green { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Magenta { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Red { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush White { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Yellow { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush LightGray { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush Gray { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush DarkGray { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush LightBlue { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush LightCyan { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush LightGreen { get; private set; }

        /// <summary>A system-defined <see cref="Brush"/> object.</summary>
        public static Brush LightYellow { get; private set; }

        static Brush ()
        {
            Black = new SolidColorBrush(Color.Black);
            Blue = new SolidColorBrush(Color.Blue);
            Cyan = new SolidColorBrush(Color.Cyan);
            Green = new SolidColorBrush(Color.Green);
            Magenta = new SolidColorBrush(Color.Magenta);
            Red = new SolidColorBrush(Color.Red);
            White = new SolidColorBrush(Color.White);
            Yellow = new SolidColorBrush(Color.Yellow);

            LightBlue = new SolidColorBrush(Color.LightBlue);
            LightCyan = new SolidColorBrush(Color.LightCyan);
            LightGreen = new SolidColorBrush(Color.LightGreen);
            LightYellow = new SolidColorBrush(Color.LightYellow);

            Gray = new SolidColorBrush(Color.Gray);
            LightGray = new SolidColorBrush(Color.LightGray);
            DarkGray = new SolidColorBrush(Color.DarkGray);
        }

        #endregion

        private float _alpha;

        /// <summary>
        /// Initializes a new instance of a <see cref="Brush"/> class.
        /// </summary>
        protected Brush ()
        {
            Color = Color.White;
            Transform = Matrix.Identity;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="Brush"/> class with a given alpha value.
        /// </summary>
        /// <param name="alpha">Alpha value of the brush.</param>
        protected Brush (float alpha)
            : this()
        {
            _alpha = alpha;
        }

        /// <summary>
        /// The alpha value of the brush.
        /// </summary>
        public virtual float Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        /// <summary>
        /// The color of the brush.
        /// </summary>
        protected internal Color Color { get; protected set; }

        /// <summary>
        /// The texture resource of the brush.
        /// </summary>
        protected internal Texture2D Texture { get; protected set; }

        /// <summary>
        /// Gets or sets the transformation to apply to brush.
        /// </summary>
        protected internal Matrix Transform { get; protected set; }

        #region IDisposable

        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="Brush"/> object.
        /// </summary>
        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose (bool disposing)
        {
            if (!_disposed) {
                if (disposing)
                    DisposeManaged();
                DisposeUnmanaged();
                _disposed = true;
            }
        }

        /// <summary>
        /// Attempts to dispose unmanaged resources.
        /// </summary>
        ~Brush ()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the managed resources used by the <see cref="Brush"/>.
        /// </summary>
        protected virtual void DisposeManaged () { }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Brush"/>.
        /// </summary>
        protected virtual void DisposeUnmanaged () { }

        #endregion
    }
}
