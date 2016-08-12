using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{

    public class VehicleLocation
    {
        public string RouteId { get; set; }

        public string Number { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public int Direction { get; set; }

        public string Data { get; set; }
    }
}
