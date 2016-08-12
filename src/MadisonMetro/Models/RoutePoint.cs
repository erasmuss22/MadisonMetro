using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{
    /// <summary>
    /// A single point on the path of a route
    /// </summary>
    public class RoutePoint
    {
        /// <summary>
        /// The latitude for this point on the route
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// The longitude for this point on the route
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Routes are divided into multiple paths with each path
        /// getting rendered in sequential order
        /// </summary>
        public int PathOrder { get; set; }

        /// <summary>
        /// The order in which this point appears on this path. When rendering,
        /// use the point order to render points within a path
        /// </summary>
        public int PointOrder { get; set; }
    }
}
