using System;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    public static class Pens
    {
        public static bool _init;

        public static Pen Black { get; private set; }
        public static Pen Blue { get; private set; }
        public static Pen Cyan { get; private set; }
        public static Pen Green { get; private set; }
        public static Pen Magenta { get; private set; }
        public static Pen Red { get; private set; }
        public static Pen White { get; private set; }
        public static Pen Yellow { get; private set; }

        public static Pen LightGray { get; private set; }
        public static Pen Gray { get; private set; }
        public static Pen DarkGray { get; private set; }

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
