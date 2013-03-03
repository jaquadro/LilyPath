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
        /// <summary>
        /// Initializes a new instance of a <see cref="Brush"/> class.
        /// </summary>
        protected Brush ()
        { }

        /// <summary>
        /// The alpha value of the brush.
        /// </summary>
        public virtual float Alpha { get; protected set; }

        /// <summary>
        /// The texture resource of the brush.
        /// </summary>
        public virtual Texture2D Texture { get; protected set; }

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
