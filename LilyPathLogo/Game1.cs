using System;
using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LilyPathLogo
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawBatch drawBatch;

        public Game1 ()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize ()
        {
            base.Initialize();
        }

        protected override void LoadContent ()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawBatch = new DrawBatch(GraphicsDevice);

            InitializePaths();
        }

        protected override void Update (GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            Vector2 center = new Vector2(200, 200);

            drawBatch.Begin(DrawSortMode.Deferred, null, null, null, null, null, Matrix.Identity);

            drawBatch.FillCircle(new SolidColorBrush(Color.SkyBlue), center, 175);
            drawBatch.FillPath(new SolidColorBrush(Color.LimeGreen), _lilypadPath.Buffer, 0, _lilypadPath.Count);
            drawBatch.DrawPath(_lilypadStroke);
            drawBatch.DrawPath(_outerFlowerStroke);
            drawBatch.DrawPath(_innerFlowerStroke);

            drawBatch.End();

            base.Draw(gameTime);
        }

        private PathBuilder _lilypadPath;
        private GraphicsPath _lilypadStroke;
        private GraphicsPath _outerFlowerStroke;
        private GraphicsPath _innerFlowerStroke;

        private void InitializePaths ()
        {
            Vector2 center = new Vector2(200, 200);

            Pen lilypadPen = new Pen(Color.Green, 15) {
                Alignment = PenAlignment.Center
            };

            _lilypadPath = BuildLillyPad(center, 150, 0);
            _lilypadStroke = _lilypadPath.Stroke(lilypadPen, PathType.Closed);

            Pen outerFlowerPen = new Pen(Color.White * 0.75f, 15) {
                Alignment = PenAlignment.Outset
            };

            _outerFlowerStroke = BuildFlower(center, 8, 120, 100, (float)(Math.PI / 8)).Stroke(outerFlowerPen, PathType.Closed);

            Pen innerFlowerPen = new Pen(Color.MediumPurple * 0.5f, 10) {
                Alignment = PenAlignment.Outset
            };

            _innerFlowerStroke = BuildFlower(center, 16, 105, 60, 0).Stroke(innerFlowerPen, PathType.Closed);
        }

        private static PathBuilder BuildLillyPad (Vector2 center, int radius, float rotation)
        {
            float segment = (float)(Math.PI * 2 / 32);

            PathBuilder builder = new PathBuilder();

            builder.AddPoint(center);
            builder.AddLine(radius, segment * 25 + rotation);
            builder.AddArcByAngle(center, segment * 30, radius / 2);

            return builder;
        }

        private static PathBuilder BuildFlower (Vector2 center, int petalCount, float petalLength, float petalWidth, float rotation)
        {
            List<Vector2> points = StarPoints(center, petalCount / 2, petalLength, petalLength, rotation, false);

            PathBuilder builder = new PathBuilder();
            builder.AddPoint(center);

            foreach (Vector2 point in points) {
                builder.AddArcByPoint(point, petalWidth / 2);
                builder.AddArcByPoint(center, petalWidth / 2);
            }

            return builder;
        }

        private static List<Vector2> StarPoints (Vector2 center, int pointCount, float outerRadius, float innerRadius, float rotation, bool close)
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
    }
}
