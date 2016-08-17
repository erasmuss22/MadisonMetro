using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{
    public class RouteCurrentData
    {
        public RouteCurrentData()
        {
            this.StopData = new List<RouteStopTime>();
            this.VehicleData = new List<VehicleLocation>();
        }

        public List<RouteStopTime> StopData { get; set; }

        public List<VehicleLocation> VehicleData { get; set; }
    }
}
