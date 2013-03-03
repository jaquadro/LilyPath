using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LilyPath
{
    /// <summary>
    /// The type of arc in closed drawing or fill operations.
    /// </summary>
    public enum ArcType
    {
        /// <summary>
        /// Causes the endpoints of the arc to be connected directly.
        /// </summary>
        Segment,

        /// <summary>
        /// Causes the endpoints of the arc to be connected to the arc center, as in a pie wedge.
        /// </summary>
        Sector
    }

    /// <summary>
    /// The style of termination used at the endpoints of stroked paths.
    /// </summary>
    public enum LineCap
    {
        /// <summary>
        /// The stroked path is cut off at the immediate edge of the path's endpoint.
        /// </summary>
        Flat,

        /// <summary>
        /// The stroked path runs half the pen's width past the edge of the path's endpoint.
        /// </summary>
        Square,
    }

    /// <summary>
    /// Whether a path is open or closed in draw operations.
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// The endpoints of the path should not be connected.
        /// </summary>
        Open,

        /// <summary>
        /// The endpoints of the path should be connected.
        /// </summary>
        Closed,
    }

    /// <summary>
    /// The alignment of a path stroked by a <see cref="Pen"/> relative to the ideal path.
    /// </summary>
    public enum PenAlignment
    {
        /// <summary>
        /// The stroked path should be centered directly over the ideal path.
        /// </summary>
        Center,

        /// <summary>
        /// The stroked path should run along the inside edge of the ideal path.
        /// </summary>
        Inset,

        /// <summary>
        /// The stroked path should run along the outside edge of the ideal path.
        /// </summary>
        Outset
    }
}
