using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{

    public class VehicleLocation
    {
        /// <summary>
        /// The id of the route this vehicle is currently on
        /// </summary>
        public string RouteId { get; set; }

        /// <summary>
        /// The number of the bus as assigned by Madison Metro
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The current latitude of the bus
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// The current longitude of the bus
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// The current direction of the bus (1-8) with 1 being N, 2 being NE, and 8 NW
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// The name of the next stop this bus is headed towards
        /// </summary>
        public string NextStop { get; set; }

        /// <summary>
        /// The last stop the bus will make while headed in the current direction
        /// </summary>
        public string FinalStop { get; set; }
    }
}
