using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{
    public class MadisonMetro
    {
        private static Dictionary<string, Route> routesByRouteNumber = new Dictionary<string, Route>()
        {
            { "01", new Route() { IsActive = true, Name = "Route 1", Id = "01" } },
            { "02", new Route() { IsActive = true, Name = "Route 2", Id = "02" } },
            { "03", new Route() { IsActive = true, Name = "Route 3", Id = "03" } },
            { "04", new Route() { IsActive = true, Name = "Route 4", Id = "04" } },
            { "06", new Route() { IsActive = true, Name = "Route 6", Id = "06" } },
            { "09", new Route() { IsActive = true, Name = "Route 9", Id = "09" } }
        };

        public static IEnumerable<Route> GetAllRoutes()
        {
            return routesByRouteNumber.Values;
        }

        public static IEnumerable<string> GetRouteIds()
        {
            return routesByRouteNumber.Keys;
        }

        public static Route GetRouteById(string id)
        {
            if (routesByRouteNumber.ContainsKey(id))
            {
                return routesByRouteNumber[id];
            }

            return null;
        }
    }
}
