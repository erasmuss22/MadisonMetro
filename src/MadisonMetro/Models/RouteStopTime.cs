using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{
    public class RouteStopTime
    {
        public RouteStopTime()
        {
            this.Times = new List<string>();
        }

        public string RouteId { get; set; }

        public string StopId { get; set; }

        public List<string> Times { get; set; }
    }
}
