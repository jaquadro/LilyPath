using System;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Texture Fill")]
    public class TextureFillSheet : TestSheet
    {
        private Texture2D _xor6;

        private TextureBrush _brush1;
        private TextureBrush _brush2;
        private TextureBrush _brush3;
        private TextureBrush _brush4;
        private TextureBrush _brush5;
        private TextureBrush _brush6;

        public override void Setup (GraphicsDevice device)
        {
            _xor6 = BuildXorTexture(device, 6);

            _brush1 = new TextureBrush(_xor6);
            _brush2 = new TextureBrush(_xor6) {
                Transform = Matrix.CreateTranslation(-50f / _xor6.Width, -175f / _xor6.Height, 0)
            };
            _brush3 = new TextureBrush(_xor6) {
                Transform = Matrix.CreateScale(.25f, .5f, 1f)
            };
            _brush4 = new TextureBrush(_xor6) {
                Transform = Matrix.CreateRotationZ((float)Math.PI / 4)
            };
            _brush5 = new TextureBrush(_xor6, .5f);
            _brush6 = new TextureBrush(_xor6) {
                Color = Color.Purple
            };
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.FillRectangle(_brush1, new Rectangle(50, 50, 200, 100));
            drawBatch.FillRectangle(_brush2, new Rectangle(50, 175, 200, 100));
            drawBatch.FillRectangle(_brush3, new Rectangle(50, 300, 200, 100));
            drawBatch.FillRectangle(_brush4, new Rectangle(50, 425, 200, 100));
            drawBatch.FillCircle(_brush5, new Vector2(350, 100), 50);
            drawBatch.FillCircle(_brush6, new Vector2(350, 225), 50);
        }
    }
}
