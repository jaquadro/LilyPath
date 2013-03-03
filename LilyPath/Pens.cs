using System;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    /// <summary>
    /// Pens for a small set of standard colors.  Must be initialized before use.
    /// </summary>
    public static class Pens
    {
        private static bool _init;

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Black { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Blue { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Cyan { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Green { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Magenta { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Red { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen White { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Yellow { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen LightGray { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen Gray { get; private set; }

        /// <summary>A system-defined <see cref="Pen"/> object.</summary>
        public static Pen DarkGray { get; private set; }

        /// <summary>
        /// Creates the default set of pens with the given <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <param name="device">A valid <see cref="GraphicsDevice"/>.</param>
        public static void Initialize (GraphicsDevice device)
        {
            if (!_init) {
                _init = true;

                Brushes.Initialize(device);

                Black = new Pen(Brushes.Black);
                Blue = new Pen(Brushes.Blue);
                Cyan = new Pen(Brushes.Cyan);
                Green = new Pen(Brushes.Green);
                Magenta = new Pen(Brushes.Magenta);
                Red = new Pen(Brushes.Red);
                White = new Pen(Brushes.White);
                Yellow = new Pen(Brushes.Yellow);

                LightGray = new Pen(Brushes.LightGray);
                Gray = new Pen(Brushes.Gray);
                DarkGray = new Pen(Brushes.DarkGray);
            }
        }
    }
}
