using System;
using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Mystify Your Mind")]
    public class MystifySheet : TestSheet
    {
        private class Figure
        {
            private static Random rand = new Random();

            public Vector2[] Points;
            public Vector2[] Velocities;
            public List<Vector2[]> History;

            public Pen ColorPen;
            public float Skip = 0;
            public float SkipLimit = .03f;

            public Figure (int pointCount, int lineCount)
            {
                Points = new Vector2[pointCount];
                Velocities = new Vector2[pointCount];

                History = new List<Vector2[]>();
                for (int i = 0; i < lineCount; i++) {
                    History.Add(new Vector2[pointCount]);
                }
            }

            public void Initialize (Rectangle bounds, float mag)
            {
                ColorPen = new Pen(new Color(rand.Next(255), rand.Next(255), rand.Next(255)));

                for (int i = 0; i < Points.Length; i++) {
                    Points[i] = new Vector2(bounds.Left + (float)rand.NextDouble() * bounds.Width, bounds.Top + (float)rand.NextDouble() * bounds.Height);
                    Velocities[i] = new Vector2((float)(rand.NextDouble() - .5) * mag, (float)(rand.NextDouble() - .5) * mag);

                    for (int j = 0; j < History.Count; j++)
                        History[j][i] = Points[i];
                }
            }

            public void Update (Rectangle bounds, float time)
            {
                for (int i = 0; i < Points.Length; i++) {
                    Points[i] += Velocities[i] * time;
                    if (Points[i].X < bounds.Left)
                        Velocities[i].X = Math.Abs(Velocities[i].X);
                    else if (Points[i].X > bounds.Right)
                        Velocities[i].X = -Math.Abs(Velocities[i].X);
                    if (Points[i].Y < bounds.Top)
                        Velocities[i].Y = Math.Abs(Velocities[i].Y);
                    else if (Points[i].Y > bounds.Bottom)
                        Velocities[i].Y = -Math.Abs(Velocities[i].Y);
                }

                Skip += time;
                if (Skip >= SkipLimit) {
                    Skip = 0;
                    for (int i = History.Count - 1; i > 0; i--)
                        History[i - 1].CopyTo(History[i], 0);
                    Points.CopyTo(History[0], 0);
                }
            }
        }

        private List<Figure> _figures = new List<Figure>();
        private Rectangle _bounds;

        public override void Setup (GraphicsDevice device)
        {
            ClearColor = Color.Black;

            _bounds = device.Viewport.Bounds;

            _figures.Add(new Figure(4, 5));
            _figures.Add(new Figure(4, 7));

            for (int i = 0; i < _figures.Count; i++)
                _figures[i].Initialize(_bounds, 400);
        }

        public override void Update (GameTime gameTime)
        {
            foreach (Figure fig in _figures) {
                fig.Update(_bounds, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public override void Draw (DrawBatch drawBatch)
        {
            _bounds = drawBatch.GraphicsDevice.Viewport.Bounds;

            foreach (Figure fig in _figures) {
                foreach (var points in fig.History) {
                    drawBatch.DrawPrimitivePath(fig.ColorPen, points, PathType.Closed);
                }
            }
        }
    }
}
