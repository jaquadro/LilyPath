using System;
using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPathDemo.TestSheets
{
    [TestName("Water Lily")]
    public class WaterLilySheet : TestSheet
    {
        private Vector2 _origin = new Vector2(200, 200);
        private float _startAngle = (float)(Math.PI / 16) * 25; // 11:20
        private float _arcLength = (float)(Math.PI / 16) * 30;

        private GraphicsPath _lilyOuterFlower;
        private GraphicsPath _lilyInnerFlower;

        public override void Setup (GraphicsDevice device)
        {
            Pen penOuterFlower = new Pen(Color.White * 0.75f, 15) { Alignment = PenAlignment.Outset };
            _lilyOuterFlower = CreateFlowerGP(penOuterFlower, _origin, 8, 120, 100, (float)(Math.PI / 8));

            Pen penInnerFlower = new Pen(Color.MediumPurple * 0.5f, 10) { Alignment = PenAlignment.Outset };
            _lilyInnerFlower = CreateFlowerGP(penInnerFlower, _origin, 16, 105, 60, 0);
        }

        public override void Draw (DrawBatch drawBatch)
        {
            drawBatch.FillCircle(new SolidColorBrush(Color.SkyBlue), _origin, 175);
            drawBatch.FillArc(new SolidColorBrush(Color.LimeGreen), _origin, 150, _startAngle, _arcLength, ArcType.Sector);
            drawBatch.DrawClosedArc(new Pen(Color.Green, 15), _origin, 150, _startAngle, _arcLength, ArcType.Sector);
            drawBatch.DrawPath(_lilyOuterFlower);
            drawBatch.DrawPath(_lilyInnerFlower);
        }

        private GraphicsPath CreateFlowerGP (Pen pen, Vector2 center, int petalCount, float petalLength, float petalWidth, float rotation)
        {
            List<Vector2> points = StarPoints(center, petalCount / 2, petalLength, petalLength, rotation, false);

            PathBuilder builder = new PathBuilder();
            builder.AddPoint(center);

            foreach (Vector2 point in points) {
                builder.AddArcByPoint(point, petalWidth / 2);
                builder.AddArcByPoint(center, petalWidth / 2);
            }

            return builder.Stroke(pen, PathType.Closed);
        }
    }
}
