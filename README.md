LilyPath
========

LilyPath is a 2D path and shape drawing library for MonoGame and XNA.  All drawing is rendered to polygons, not rasterized to a texture.  The main API is presented as a DrawBatch, which is directly analogous to a SpriteBatch, and allows drawing operations to be batched together.  Other supporting classes allow for better caching or building up of complex geometry.  Several aspects of the API are analogous to .NET's System.Drawing and System.Drawing.Drawing2D namespaces, although more limited.

![](https://raw.github.com/wiki/jaquadro/LilyPath/images/lilypath.png)

Capabilities include:
* Drawing primitive lines, paths, and closed shapes (drawn as 1px line lists).
* Drawing lines, paths, and shapes with customizable pens.
* Filling paths and shapes with customizable brushes.
* Color or pattern fill for brushes.
* Varying width, alignment, end cap styles for pens.
* Multisegment lines are mitered.
* Several ways to draw arcs, including by radius/angle or between points.

The LilyPath project is configured to depend on MonoGame by default, but it should be possible to retarget to XNA as well.
