using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    /// <summary>
    /// Brushes for a small set of standard colors.  Must be initialized before use.
    /// </summary>
    public static class Brushes
    {
        private static bool _init;

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

        /// <summary>
        /// Creates the default set of brushes with the given <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <param name="device">A valid <see cref="GraphicsDevice"/>.</param>
        public static void Initialize (GraphicsDevice device)
        {
            if (!_init) {
                _init = true;

                Black = new SolidColorBrush(Color.Black);
                Blue = new SolidColorBrush(Color.Blue);
                Cyan = new SolidColorBrush(Color.Cyan);
                Green = new SolidColorBrush(Color.Green);
                Magenta = new SolidColorBrush(Color.Magenta);
                Red = new SolidColorBrush(Color.Red);
                White = new SolidColorBrush(Color.White);
                Yellow = new SolidColorBrush(Color.Yellow);

                Gray = new SolidColorBrush(Color.Gray);
                LightGray = new SolidColorBrush(Color.LightGray);
                DarkGray = new SolidColorBrush(Color.DarkGray);
            }
        }
    }
}
