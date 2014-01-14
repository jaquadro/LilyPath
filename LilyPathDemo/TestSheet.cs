using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LilyPath;
using LilyPath.Pens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo
{
    public class TestNameAttribute : Attribute
    {
        public TestNameAttribute (string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class TestSheet
    {
        private RasterizerState _rasterState;
        private bool _needsSetup = true;

        private RasterizerState GetCommonRasterizerState ()
        {
            if (_rasterState == null)
                _rasterState = new RasterizerState() {
                    FillMode = DemoState.FillMode,
                    MultiSampleAntiAlias = DemoState.MultisampleAA,
                };

            return _rasterState;
        }

        public Color ClearColor { get; set; }

        public virtual void Setup (GraphicsDevice device)
        { }

        public virtual void TearDown ()
        { }

        public virtual void Apply (GameTime gameTime, DrawBatch drawBatch)
        {
            if (_needsSetup) {
                ClearColor = Color.WhiteSmoke;
                Setup(drawBatch.GraphicsDevice);
                _needsSetup = false;
            }

            Update(gameTime);

            Begin(drawBatch);
            Draw(drawBatch);
            End(drawBatch);
        }

        public virtual void Update (GameTime gameTime)
        { }

        public virtual void Begin (DrawBatch drawBatch)
        {
            drawBatch.Begin(DrawSortMode.Deferred, null, null, null, GetCommonRasterizerState(), null, Matrix.Identity);
        }

        public virtual void Draw (DrawBatch drawBatch)
        { }

        public virtual void End (DrawBatch drawBatch)
        {
            drawBatch.End();
        }

        protected static List<Vector2> ShiftPath (List<Vector2> path, float x, float y)
        {
            for (int i = 0; i < path.Count; i++)
                path[i] = new Vector2(path[i].X + x, path[i].Y + y);
            return path;
        }

        protected static List<Vector2> StarPoints (Vector2 center, int pointCount, float outerRadius, float innerRadius, float rotation, bool close)
        {
            List<Vector2> points = new List<Vector2>();

            int limit = (close) ? pointCount * 2 + 1 : pointCount * 2;

            float rot = (float)((Math.PI * 2) / (pointCount * 2));
            for (int i = 0; i < limit; i++) {
                float si = (float)Math.Sin(-i * rot + Math.PI + rotation);
                float ci = (float)Math.Cos(-i * rot + Math.PI + rotation);

                if (i % 2 == 0)
                    points.Add(center + new Vector2(si, ci) * outerRadius);
                else
                    points.Add(center + new Vector2(si, ci) * innerRadius);
            }

            return points;
        }

        protected static Texture2D BuildXorTexture (GraphicsDevice device, int bits)
        {
            if (bits < 1 || bits > 8)
                throw new ArgumentException("Xor texture must have between 1 and 8 bits", "bits");

            Texture2D tex = new Texture2D(device, 1 << bits, 1 << bits);
            Color[] data = new Color[tex.Width * tex.Height];

            for (int y = 0; y < tex.Height; y++) {
                for (int x = 0; x < tex.Width; x++) {
                    float lum = ((x << (8 - bits)) ^ (y << (8 - bits))) / 255f;
                    data[y * tex.Width + x] = new Color(lum, lum, lum);
                }
            }

            tex.SetData(data);

            return tex;
        }
    }
}
