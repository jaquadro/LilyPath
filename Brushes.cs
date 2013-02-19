using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    public static class Brushes
    {
        private static bool _init;

        public static Brush Black { get; private set; }
        public static Brush Blue { get; private set; }
        public static Brush Cyan { get; private set; }
        public static Brush Green { get; private set; }
        public static Brush Magenta { get; private set; }
        public static Brush Red { get; private set; }
        public static Brush White { get; private set; }
        public static Brush Yellow { get; private set; }

        public static Brush LightGray { get; private set; }
        public static Brush Gray { get; private set; }
        public static Brush DarkGray { get; private set; }

        public static void Initialize (GraphicsDevice device)
        {
            if (!_init) {
                _init = true;

                Black = new SolidColorBrush(device, Color.Black);
                Blue = new SolidColorBrush(device, Color.Blue);
                Cyan = new SolidColorBrush(device, Color.Cyan);
                Green = new SolidColorBrush(device, Color.Green);
                Magenta = new SolidColorBrush(device, Color.Magenta);
                Red = new SolidColorBrush(device, Color.Red);
                White = new SolidColorBrush(device, Color.White);
                Yellow = new SolidColorBrush(device, Color.Yellow);

                Gray = new SolidColorBrush(device, Color.Gray);
                LightGray = new SolidColorBrush(device, Color.LightGray);
                DarkGray = new SolidColorBrush(device, Color.DarkGray);
            }
        }
    }
}
