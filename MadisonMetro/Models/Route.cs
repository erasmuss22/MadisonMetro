using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{
    /// <summary>
    /// A defined route in the Madison Metro system
    /// </summary>
    public class Route
    {
        /// <summary>
        /// The unique identifier for this route
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The latitude point that roughly defines the center of the route
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// The longitude point that roughly defines the center of the route
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// The friendly display name of the route
        /// </summary>
        public string Name { get; set; }
    }
}
