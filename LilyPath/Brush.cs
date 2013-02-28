using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    public abstract class Brush : IDisposable
    {
        protected Brush ()
        {
        }

        public virtual float Alpha { get; protected set; }
        public virtual Texture2D Texture { get; protected set; }

        #region IDisposable

        private bool _disposed;

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

        ~Brush ()
        {
            Dispose(false);
        }

        protected virtual void DisposeManaged () { }

        protected virtual void DisposeUnmanaged () { }

        #endregion
    }
}
