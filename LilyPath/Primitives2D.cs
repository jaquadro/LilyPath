using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LilyPath
{
    /*public abstract class Brush
    {
        protected Brush ()
        {
        }

        public virtual float Alpha { get; protected set; }
        public virtual Texture2D Texture { get; protected set; }
    }

    public class SolidColorBrush : Brush
    {
        public Color Color { get; private set; }

        public SolidColorBrush (SpriteBatch spriteBatch, Color color)
            : base()
        {
            Alpha = color.A / 255f;
            Color = color;

            color = new Color(color.R, color.G, color.B, 255);

            Texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new Color[] { color });
        }
    }

    public class LineStippleBrush : Brush
    {
        public LineStippleBrush (SpriteBatch spriteBatch, Color color, string pattern, int thickness)
            : base()
        {
            Color[] data = new Color[pattern.Length * thickness];
            for (int i = 0; i < pattern.Length; i++) {
                if (pattern[i] == ' ') {
                    data[i] = Color.Transparent;
                }
                else {
                    data[i] = color;
                }
            }

            for (int i = 1; i < thickness; i++) {
                for (int j = 0; j < pattern.Length; j++) {
                    data[i * j] = data[j];
                }
            }

            Texture = new Texture2D(spriteBatch.GraphicsDevice, pattern.Length, thickness, false, SurfaceFormat.Color);
            Texture.SetData(data);
        }

        public override float Alpha
        {
            get { return 1f; }
        }
    }*/

    /// <summary>
    /// </summary>
    [Obsolete]
    public static class Primitives2D
    {


        #region Private Members

        private static readonly Dictionary<String, List<Vector2>> m_circleCache = new Dictionary<string, List<Vector2>>();
        private static readonly Dictionary<String, List<Vector2>> m_arcCache = new Dictionary<string, List<Vector2>>();
        private static Texture2D m_pixel;

        #endregion


        #region Vector Calculation Methods
        // TODO: Do... *something*... with these

        /* NOTE (gtexmo)
         * These are Vector Calculation Methods are temporary... I was going to
         * use them in creating an arc via subdivision but determined that it
         * would really be more work than necessary (vs. just doing it via
         * angles). They probably don't belong in this library but I'll leave
         * here until I can figure out where to put them. They're insanely
         * useful calculations so they gotta go somewhere :)
         */
        private static Vector2 PerpendicularVector (Vector2 v)
        {
            Vector3 perp = Vector3.Cross(new Vector3(v.X, v.Y, 0), new Vector3(0, 0, 1));
            perp.Normalize();

            return new Vector2(perp.X, perp.Y);
        }

        private static bool MidVector (Vector2 line1a, Vector2 line1b, Vector2 line2a, Vector2 line2b, out Vector2 pCenter, out Vector2 dCenter)
        {
            bool result = false;

            dCenter = new Vector2(0.0f, 0.0f);
            pCenter = new Vector2(0.0f, 0.0f);

            if (LineIntersect(line1a, line1b, line2a, line2b, out pCenter)) {
                // We have an intersection point, this is a common point for the two lines
                // Now get a point that is exactly length 1 away from the intersection point
                Vector2 dL1 = line1b - line1a;
                dL1.Normalize();

                Vector2 dL2 = line2b - line2a;
                dL2.Normalize();

                Vector2 a1 = pCenter + dL1;
                Vector2 a2 = pCenter + dL2;

                // Get the half-way point between these points
                Vector2 A = (a1 + a2) / 2;

                // The direction is the vector from the intersection point to the half-way
                // point
                dCenter = A - pCenter;
                dCenter.Normalize();

                result = true;
            }
            else {
                // Find the vector perpendicular to line 1
                Vector2 perp = PerpendicularVector(line1b - line1a);

                // Find where the perpendicular line intersects line 2 at points a and b of line 1
                Vector2 line1a_to_2, line1b_to_2;
                LineIntersect(line2a, line2b, line1a, line1a + perp, out line1a_to_2);
                LineIntersect(line2a, line2b, line1b, line1b + perp, out line1b_to_2);

                // Get the center points between the points we calculated above
                Vector2 a1 = (line1a + line1a_to_2) / 2;
                Vector2 a2 = (line1b + line1b_to_2) / 2;

                // Set up the point and direction
                pCenter = a1;
                dCenter = a2 - a1;
                dCenter.Normalize();
                result = true;
            }

            return result;
        }

        private static bool LineIntersect (Vector2 line1a, Vector2 line1b, Vector2 line2a, Vector2 line2b, out Vector2 intersectPoint)
        {
            bool result = false;
            intersectPoint = new Vector2(0.0f, 0.0f);

            // Build our line equation
            Vector2 p01 = line1a;
            Vector2 p02 = line2a;
            Vector2 d1 = line1b - line1a;
            Vector2 d2 = line2b - line2a;
            d1.Normalize();
            d2.Normalize();

            // Test the denominator
            float d = d2.X * d1.Y - d1.X * d2.Y;

            // Lines are parallel if the denominator is zero*
            // *NOTE: "Zero" isn't really zero since there is some error with floating point numbers...
            //        ... With that in mind, we'll call d zero if it's within a certain tolerance
            //if (d != 0)
            if (Math.Abs(d) > 0.00005) {
                // calculate s for line 2
                float s = (d1.X * p02.Y) - (d1.X * p01.Y) - (d1.Y * p02.X) + (d1.Y * p01.X);
                s /= d;

                intersectPoint = p02 + (s * d2);
                result = true;
            }

            return result;
        }
        #endregion


        #region Private Methods

        private static void CreateThePixel (SpriteBatch spriteBatch)
        {
            m_pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            m_pixel.SetData(new Color[] { Color.White });
        }


        /// <summary>
        /// Draws a list of connecting points
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="position">Where to position the points</param>
        /// <param name="points">The points to connect with lines</param>
        /// <param name="color">The color to use</param>
        private static void DrawPoints (SpriteBatch spriteBatch, Vector2 position, List<Vector2> points, Brush brush)
        {
            DrawPoints(spriteBatch, position, points, brush, 1.0f);
        }


        /// <summary>
        /// Draws a list of connecting points
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="position">Where to position the points</param>
        /// <param name="points">The points to connect with lines</param>
        /// <param name="color">The color to use</param>
        /// <param name="color">The thickness to use</param>
        /// <param name="thickness">The thickness of the lines</param>
        private static void DrawPoints (SpriteBatch spriteBatch, Vector2 position, List<Vector2> points, Brush brush, float thickness)
        {
            if (points.Count < 2)
                return;

            for (int i = 1; i < points.Count; i++) {
                DrawLine(spriteBatch, points[i - 1] + position, points[i] + position, brush, thickness);
            }
        }


        /// <summary>
        /// Creates a list of vectors that represents a circle
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <returns>A list of vectors that, if connected, will create a circle</returns>
        private static List<Vector2> CreateCircle (double radius, int sides)
        {
            // Look for a cached version of this circle
            String circleKey = radius + "x" + sides;
            if (m_circleCache.ContainsKey(circleKey)) {
                return m_circleCache[circleKey];
            }

            List<Vector2> vectors = new List<Vector2>();

            const double max = 2.0 * Math.PI;
            double step = max / sides;

            for (double theta = 0.0; theta < max; theta += step) {
                vectors.Add(new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta))));
            }

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2((float)(radius * Math.Cos(0)), (float)(radius * Math.Sin(0))));

            // Cache this circle so that it can be quickly drawn next time
            m_circleCache.Add(circleKey, vectors);

            return vectors;
        }


        /// <summary>
        /// Creates a list of vectors that represents an arc
        /// </summary>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate in the circle that this will cut out from</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, 90 being south</param>
        /// <param name="degrees">The number of degrees to draw, clockwise from the starting angle</param>
        /// <returns>A list of vectors that, if connected, will create an arc</returns>
        private static List<Vector2> CreateArc (float radius, int sides, float startingAngle, float degrees)
        {
            List<Vector2> points = new List<Vector2>();
            points.AddRange(CreateCircle(radius, sides));
            points.RemoveAt(points.Count - 1);		// remove the last point because it's a duplicate of the first

            // The circle starts at (radius, 0)
            double curAngle = 0.0;
            double anglePerSide = 360.0 / sides;

            // "Rotate" to the starting point
            while ((curAngle + (anglePerSide / 2.0)) < startingAngle) {
                curAngle += anglePerSide;

                // move the first point to the end
                points.Add(points[0]);
                points.RemoveAt(0);
            }

            // Add the first point, just in case we make a full circle
            points.Add(points[0]);

            // Now remove the points at the end of the circle to create the arc
            int sidesInArc = (int)((degrees / anglePerSide) + 0.5);
            points.RemoveRange(sidesInArc + 1, points.Count - sidesInArc - 1);

            return points;
        }


        /// <summary>
        /// Creates a list of vectors that represents an arc by generating points between two angles
        /// </summary>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides the arc will have</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, 90 being south</param>
        /// <param name="endingAngle">The starting angle of arc, 0 being to the east, 90 being south</param>
        /// <returns>A list of vectors that, if connected, will create an arc</returns>
        private static List<Vector2> CreateArc2 (float radius, int sides, float startingAngle, float endingAngle)
        {
            // Look for a cached version of this arc
            String arcKey = radius + "x" + sides + "x" + startingAngle + "x" + endingAngle;
            if (m_arcCache.ContainsKey(arcKey)) {
                return m_arcCache[arcKey];
            }

            List<Vector2> points = new List<Vector2>();

            // Convert the angles to radians
            double startRadians = Math.PI * startingAngle / 180.0;
            double endRadians = Math.PI * endingAngle / 180.0;

            // If the start is larger than the end, move the end another lap around the circle
            if (startRadians >= endRadians) endRadians += 2 * Math.PI;

            // How many radians per side?
            double step = (endRadians - startRadians) / sides;

            // Walk around our angle and generate points
            double theta;
            for (theta = startRadians; theta < endRadians; theta += step) {
                points.Add(new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta))));
            }
            points.Add(new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta))));

            // Cache the arc
            m_arcCache.Add(arcKey, points);

            return points;
        }


        public static List<Vector2> CreateArc3 (float radius, int sides, float startingAngle, float endingAngle)
        {
            // TODO: Something's wrong, strange results... see below
            // In some cases (ie, try a circle with 25 sides) the intersection point doesn't calculate
            // correctly. I don't feel like looking into it quite yet but it needs attention. I'm pretty
            // sure I screwed some math up somewhere.

            // Look for a cached version of this arc
            String arcKey = radius + "x" + sides + "x" + startingAngle + "x" + endingAngle;
            if (m_arcCache.ContainsKey(arcKey)) {
                return m_arcCache[arcKey];
            }

            List<Vector2> points = new List<Vector2>();

            double radiansPerSide = (2 * Math.PI) / sides;

            // Convert the angles to radians
            double startRadians = Math.PI * startingAngle / 180.0;
            double endRadians = Math.PI * endingAngle / 180.0;

            // If the start is larger than the end, move the end another lap around the circle
            if (startRadians >= endRadians) endRadians += 2 * Math.PI;

            // Get the start and end segments
            int start = (int)Math.Floor(startRadians / radiansPerSide);
            int end = (int)Math.Floor(endRadians / radiansPerSide);

            // Generate complete segments for the portion of the circle we're after
            double theta = 0.0;
            for (theta = start * radiansPerSide; theta <= end * radiansPerSide; theta += radiansPerSide) {
                points.Add(new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta))));
            }
            points.Add(new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta))));

            // Trim the start and end segments so they are at the appropriate angles
            if (points.Count > 1) {
                Vector2 startPoint = new Vector2((float)(radius * Math.Cos(startRadians)), (float)(radius * Math.Sin(startRadians)));
                Vector2 endPoint = new Vector2((float)(radius * Math.Cos(endRadians)), (float)(radius * Math.Sin(endRadians)));

                Vector2 startIntersect, endIntersect;
                Vector2 center = new Vector2(0, 0);
                bool changeStart = false, changeEnd = false;

                if (LineIntersect(center, startPoint, points[0], points[1], out startIntersect)) {
                    changeStart = true;
                }

                if (LineIntersect(center, endPoint, points[points.Count - 2], points[points.Count - 1], out endIntersect)) {
                    changeEnd = true;
                }

                if (changeStart) points[0] = startIntersect;
                if (changeEnd) points[points.Count - 1] = endIntersect;
            }

            // Cache the arc
            //m_arcCache.Add(arcKey, points);

            return points;
        }
        #endregion


        #region FillRectangle

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void FillRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush)
        {
            //if (m_pixel == null) { CreateThePixel(spriteBatch); }

            // Simply use the function already there
            spriteBatch.Draw(brush.Texture, rect, Color.White * brush.Alpha);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="angle">The angle to draw the rectangle at</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void FillRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush, float angle)
        {
            //if (m_pixel == null) { CreateThePixel(spriteBatch); }

            spriteBatch.Draw(brush.Texture, rect, null, Color.White * brush.Alpha, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void FillRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush)
        {
            FillRectangle(spriteBatch, location, size, brush, 0.0f);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="angle">The angle to draw the rectangle at</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void FillRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush, float angle)
        {
            //if (m_pixel == null) { CreateThePixel(spriteBatch); }

            // stretch the pixel between the two vectors
            spriteBatch.Draw(brush.Texture,
                             location,
                             null,
                             Color.White * brush.Alpha,
                             angle,
                             Vector2.Zero,
                             size,
                             SpriteEffects.None,
                             0);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x1">The X coord of the left side</param>
        /// <param name="y1">The Y coord of the upper side</param>
        /// <param name="x2">The X coord of the right side</param>
        /// <param name="y2">The Y coord of the bottom side</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void FillRectangle (SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Brush brush)
        {
            //if (m_pixel == null) { CreateThePixel(spriteBatch); }

            // Simply use the function already there
            FillRectangle(spriteBatch, new Vector2(x1, y1), new Vector2(x2, y2), brush, 1.0f);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x1">The X coord of the left side</param>
        /// <param name="y1">The Y coord of the upper side</param>
        /// <param name="x2">The X coord of the right side</param>
        /// <param name="y2">The Y coord of the bottom side</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void FillRectangle (SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Brush brush, float thickness)
        {
            //if (m_pixel == null) { CreateThePixel(spriteBatch); }

            // Simply use the function already there
            FillRectangle(spriteBatch, new Vector2(x1, y1), new Vector2(x2, y2), brush, thickness);
        }

        #endregion


        #region DrawRectangle

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush)
        {
            DrawRectangle(spriteBatch, rect, brush, 1.0f, 0.0f, new Vector2(rect.X, rect.Y));
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the lines</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush, float thickness)
        {
            DrawRectangle(spriteBatch, rect, brush, thickness, 0.0f, new Vector2(rect.X, rect.Y));
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the lines</param>
        /// <param name="angle">The angle to draw the rectangle at, this will rotate around the top-left of the rectangle by default</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush, float thickness, float angle)
        {
            DrawRectangle(spriteBatch, rect, brush, thickness, angle, new Vector2(rect.X, rect.Y));
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the lines</param>
        /// <param name="angle">The angle to draw the rectangle at</param>
        /// <param name="rotateAround">The location to rotate the rectangle around</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Rectangle rect, Brush brush, float thickness, float angle, Vector2 rotateAround)
        {

            // TODO: Handle rotations
            // TODO: Figure out the pattern for the offsets required and then handle it in the line instead of here

            DrawHLine(spriteBatch, new Vector2(rect.Left, rect.Top), rect.Width, brush);
            DrawHLine(spriteBatch, new Vector2(rect.Left, rect.Bottom), rect.Width, brush);
            DrawVLine(spriteBatch, new Vector2(rect.Left, rect.Top), rect.Height, brush);
            DrawVLine(spriteBatch, new Vector2(rect.Right, rect.Top), rect.Height, brush);

            /*DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.Right, rect.Y), brush, thickness); // top
            DrawLine(spriteBatch, new Vector2(rect.X + 1f, rect.Y), new Vector2(rect.X + 1f, rect.Bottom + 1f), brush, thickness); // left
            DrawLine(spriteBatch, new Vector2(rect.X, rect.Bottom), new Vector2(rect.Right, rect.Bottom), brush, thickness); // bottom
            DrawLine(spriteBatch, new Vector2(rect.Right + 1f, rect.Y), new Vector2(rect.Right + 1f, rect.Bottom + 1f), brush, thickness); // right
             */
            /*
            DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), Color.White, thickness); // top
            DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y + rect.Height + 1f), Color.Red, thickness); // left
            DrawLine(spriteBatch, new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), Color.Blue, thickness); // bottom
            DrawLine(spriteBatch, new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), Color.Green, thickness); // right
            //*/

            /* Gary's method:
            p2.X += 1;
            p2.Y += 1;
            float offset = thickness;
            Line(new Vector2(p1.X, p1.Y), new Vector2(p2.X, p1.Y), thickness, color);
            Line(new Vector2(p1.X + offset, p1.Y), new Vector2(p1.X + offset, p2.Y), thickness, color);
            Line(new Vector2(p1.X, p2.Y - offset), new Vector2(p2.X, p2.Y - offset), thickness, color);
            Line(new Vector2(p2.X, p1.Y), new Vector2(p2.X, p2.Y), thickness, color);
            //*/
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush)
        {
            DrawRectangle(spriteBatch, new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), brush, 1.0f, 0.0f, location);
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush, float thickness)
        {
            DrawRectangle(spriteBatch, new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), brush, thickness, 0.0f, location);
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the line</param>
        /// <param name="angle">The angle to draw the rectangle at, this will rotate around the top-left of the rectangle by default</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush, float thickness, float angle)
        {
            DrawRectangle(spriteBatch, new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), brush, thickness, angle, location);
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the line</param>
        /// <param name="angle">The angle to draw the rectangle at</param>
        /// <param name="rotateAround">Rotate around this point</param>
        public static void DrawRectangle (SpriteBatch spriteBatch, Vector2 location, Vector2 size, Brush brush, float thickness, float angle, Vector2 rotateAround)
        {
            DrawRectangle(spriteBatch, new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), brush, thickness, angle, rotateAround);
        }

        #endregion


        #region DrawLine

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x1">The X coord of the first point</param>
        /// <param name="y1">The Y coord of the first point</param>
        /// <param name="x2">The X coord of the second point</param>
        /// <param name="y2">The Y coord of the second point</param>
        /// <param name="color">The color to use</param>
        public static void DrawLine (SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Brush brush)
        {
            DrawLine(spriteBatch, new Vector2(x1, y1), new Vector2(x2, y2), brush, 1.0f);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x1">The X coord of the first point</param>
        /// <param name="y1">The Y coord of the first point</param>
        /// <param name="x2">The X coord of the second point</param>
        /// <param name="y2">The Y coord of the second point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawLine (SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Brush brush, float thickness)
        {
            DrawLine(spriteBatch, new Vector2(x1, y1), new Vector2(x2, y2), brush, thickness);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <param name="color">The color to use</param>
        public static void DrawLine (SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Brush brush)
        {
            DrawLine(spriteBatch, point1, point2, brush, 1.0f);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawLine (SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Brush brush, float thickness)
        {
            // calculate the distance between the two vectors
            float distance = Vector2.Distance(point1, point2);

            // calculate the angle between the two vectors
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            DrawLine(spriteBatch, point1, distance, angle, brush, thickness);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point</param>
        /// <param name="color">The color to use</param>
        public static void DrawLine (SpriteBatch spriteBatch, Vector2 point, float length, float angle, Brush brush)
        {
            DrawLine(spriteBatch, point, length, angle, brush, 1.0f);
        }

        public static void DrawHLine (SpriteBatch spriteBatch, Vector2 point, float length, Brush brush)
        {
            spriteBatch.Draw(brush.Texture, new Rectangle((int)point.X, (int)point.Y, (int)length, 1), Color.White * brush.Alpha);
        }

        public static void DrawVLine (SpriteBatch spriteBatch, Vector2 point, float length, Brush brush)
        {
            spriteBatch.Draw(brush.Texture, new Rectangle((int)point.X, (int)point.Y, 1, (int)length), Color.White * brush.Alpha);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawLine (SpriteBatch spriteBatch, Vector2 point, float length, float angle, Brush brush, float thickness)
        {
            if (m_pixel == null) { CreateThePixel(spriteBatch); }

            float width = length * (float)Math.Cos(angle);
            float height = length * (float)Math.Sin(angle);
            Vector2 point2 = new Vector2(point.X + width, point.Y + height);

            int steps = (int)(length / brush.Texture.Width);
            float stepX = brush.Texture.Width * (float)Math.Cos(angle);
            float stepY = brush.Texture.Width * (float)Math.Sin(angle);

            Vector2 stepPoint = point;
            for (int i = 0; i < steps; i++) {
                spriteBatch.Draw(brush.Texture,
                             stepPoint,
                             null,
                             Color.White * brush.Alpha,
                             angle,
                             Vector2.Zero,
                             Vector2.One,
                             SpriteEffects.None,
                             0);
                stepPoint.X += stepX;
                stepPoint.Y += stepY;
            }

            int rem = (int)(length - steps * brush.Texture.Width);

            if (rem > 0) {
                spriteBatch.Draw(brush.Texture,
                             stepPoint,
                             new Rectangle(0, 0, rem, brush.Texture.Height),
                             Color.White * brush.Alpha,
                             angle,
                             Vector2.Zero,
                             Vector2.One,
                             SpriteEffects.None,
                             0);
            }
        }

        #endregion


        #region PutPixel
        public static void PutPixel (SpriteBatch spriteBatch, float x, float y, Color color)
        {
            PutPixel(spriteBatch, new Vector2(x, y), color);
        }

        public static void PutPixel (SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            if (m_pixel == null) { CreateThePixel(spriteBatch); }

            spriteBatch.Draw(m_pixel, position, color);
        }
        #endregion


        #region DrawCircle
        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        public static void DrawCircle (SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Brush brush)
        {
            DrawPoints(spriteBatch, center, CreateCircle(radius, sides), brush, 1.0f);
        }


        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        public static void DrawCircle (SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Brush brush, float thickness)
        {
            DrawPoints(spriteBatch, center, CreateCircle(radius, sides), brush, thickness);
        }

        public static void DrawCircle (SpriteBatch spriteBatch, float x, float y, float radius, int sides, Brush brush)
        {
            DrawPoints(spriteBatch, new Vector2(x, y), CreateCircle(radius, sides), brush, 1.0f);
        }

        public static void DrawCircle (SpriteBatch spriteBatch, float x, float y, float radius, int sides, Brush brush, float thickness)
        {
            DrawPoints(spriteBatch, new Vector2(x, y), CreateCircle(radius, sides), brush, thickness);
        }

        #endregion


        #region DrawArc

        /// <summary>
        /// Draw a arc
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the arc</param>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, 90 being south</param>
        /// <param name="degrees">The number of degrees to draw, clockwise from the starting angle</param>
        /// <param name="color">The color of the arc</param>
        public static void DrawArc (SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float degrees, Brush brush)
        {
            DrawArc(spriteBatch, center, radius, sides, startingAngle, degrees, brush, 1.0f);
        }


        /// <summary>
        /// Draw a arc
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the arc</param>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, 90 being south</param>
        /// <param name="degrees">The number of degrees to draw, clockwise from the starting angle</param>
        /// <param name="color">The color of the arc</param>
        /// <param name="thickness">The thickness of the arc</param>
        public static void DrawArc (SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float degrees, Brush brush, float thickness)
        {
            List<Vector2> arc = CreateArc(radius, sides, startingAngle, degrees);
            //List<Vector2> arc = CreateArc2(radius, sides, startingAngle, degrees);
            DrawPoints(spriteBatch, center, arc, brush, thickness);
        }

        #endregion


    }
}