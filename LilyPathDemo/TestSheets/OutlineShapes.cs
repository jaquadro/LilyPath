using System;
using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Outline Shapes")]
    public class OutlineShapesSheet : TestSheet
    {
        private List<Vector2> _wavy = new List<Vector2>();
        private GraphicsPath _wavyPath;

        private Pen _thickBlue;
        private Pen _thickRed;
        private Pen _thickMagenta;
        private Pen _thickBlack;
        private Pen _thickDarkGray;
        private Pen _thickGreen;

        public override void Setup (GraphicsDevice device)
        {
            for (int i = 0; i < 20; i++) {
                if (i % 2 == 0)
                    _wavy.Add(new Vector2(50 + i * 10, 100));
                else
                    _wavy.Add(new Vector2(50 + i * 10, 110));
            }

            _thickBlue = new Pen(Color.Blue, 15);
            _thickRed = new Pen(Color.Red, 15) {
                EndCap = LineCap.Square,
                StartCap = LineCap.Square,
            };
            _thickMagenta = new Pen(Color.Magenta, 15);
            _thickBlack = new Pen(Color.Black, 15);
            _thickDarkGray = new Pen(Color.DarkGray, 15);
            _thickGreen = new Pen(Color.Green, 15);

            _wavyPath = new GraphicsPath(_thickRed, _wavy);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.DrawLine(_thickBlue, new Vector2(50, 50), new Vector2(250, 50));
            drawBatch.DrawPath(_wavyPath);
            drawBatch.DrawRectangle(_thickMagenta, new Rectangle(50, 160, 200, 100));
            drawBatch.DrawCircle(_thickBlack, new Vector2(350, 100), 50);
            drawBatch.DrawCircle(_thickDarkGray, new Vector2(350, 225), 50, 16);
            drawBatch.DrawRectangle(_thickGreen, new Rectangle(50, 350, 200, 100), (float)Math.PI / 4f);
        }
    }
}
