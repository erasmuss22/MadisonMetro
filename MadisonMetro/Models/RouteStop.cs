using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{
    public class RouteStop
    {
        public string StopId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int StopType { get; set; }

        public string Name { get; set; }

        public string Direction { get; set; }
    }
}
