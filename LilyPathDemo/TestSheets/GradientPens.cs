using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LilyPath;
using LilyPath.Pens;
using Microsoft.Xna.Framework;

namespace LilyPathDemo.TestSheets
{
    public static class GradientPens
    {
        private static bool _setup;
        private static Pen gradWidth;
        private static Pen gradLength;

        private static GraphicsPath widthStar;
        private static GraphicsPath lengthStar;

        private static void Setup ()
        {
            gradWidth = new GradientPen(Color.Lime, Color.Blue, 15);
            gradLength = new PathGradientPen(Color.Lime, Color.Blue, 15);

            PathBuilder pathBuilder = new PathBuilder() { CalculateLengths = true };
            pathBuilder.AddPath(TestSheetUtilities.StarPoints(new Vector2(325, 75), 5, 50, 25, 0, false));

            widthStar = pathBuilder.Stroke(gradWidth, PathType.Open);
            lengthStar = pathBuilder.Stroke(gradLength, Matrix.CreateTranslation(0, 125, 0), PathType.Open);

            _setup = true;
        }

        [TestSheet("Gradient Pens")]
        public static void DrawGradientPens (DrawBatch drawBatch)
        {
            if (!_setup)
                Setup();

            TestSheetUtilities.SetupDrawBatch(drawBatch);

            drawBatch.DrawLine(gradWidth, new Vector2(25, 25), new Vector2(125, 125));
            drawBatch.DrawCircle(gradWidth, new Vector2(200, 75), 50);
            drawBatch.DrawPath(widthStar);

            drawBatch.DrawLine(gradLength, new Vector2(25, 150), new Vector2(125, 250));
            drawBatch.DrawCircle(gradLength, new Vector2(200, 200), 50);
            drawBatch.DrawPath(lengthStar);

            drawBatch.End();
        }
    }
}
