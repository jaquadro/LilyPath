LilyPath
========
<img src="https://raw.github.com/wiki/jaquadro/LilyPath/images/lilypath.png" align="right" title="Rendered with MSAA on XNA with LilyPath" />

LilyPath is a 2D drawing library for MonoGame and XNA.  LilyPath provides some of the functionality found in `System.Drawing`, such as drawing paths and shapes with configurable pens and brushes.

Instead of creating raster images, LilyPath renders everything directly to your scene or render target.  Complex paths and filled shapes are rendered as polygons, while primitives are rendered as GL or DX lines.

Drawing is handled through a `DrawBatch` object to reduce the number of draw calls needed.  This mirrors the role of `SpriteBatch` for rendering textured quads.  More complex geometry can be compiled ahead of time into `GraphicsPath` objects, which contain the polygon data after arcs, joins, and other calculations have been completed.

Features
--------
* Draw primitive lines, paths, and closed shapes.
* Draw complex lines, paths, and shapes with pens.
* Fill paths and shapes with brushes.
* Basic paths and shapes supported:
  * Arc, Circle, Ellipse, Line, Path, Point, Rectangle, Quad
* Pen features supported:
  * Alignment, Color, End Styles, Gradient, Join Styles (Mitering), Width
* Brush features supported:
  * Color, Texture, Transform

Example
-------
Here’s a short code sample for drawing the lily pad in the picture above (without the flower):

```csharp
drawBatch.Begin(DrawSortMode.Deferred);

Vector2 origin = new Vector2(200, 200);
float startAngle = (float)(Math.PI / 16) * 25; // 11:20
float arcLength = (float)(Math.PI / 16) * 30;

drawBatch.FillCircle(new SolidColorBrush(Color.SkyBlue), origin, 175);
drawBatch.FillArc(new SolidColorBrush(Color.LimeGreen), origin, 150, 
    startAngle, arcLength, ArcType.Sector);
drawBatch.DrawClosedArc(new Pen(Color.Green, 15), origin, 150, 
    startAngle, arcLength, ArcType.Sector);

drawBatch.End();
```

Source code for the full image and other examples can be found in the included test project, [LilyPathDemo](https://github.com/jaquadro/LilyPath/tree/master/LilyPathDemo).
