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
}
