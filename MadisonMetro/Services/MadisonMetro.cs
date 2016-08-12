using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MadisonMetroSDK
{
    public class MadisonMetro
    {
        // TODO add real route data
        private static Dictionary<string, Route> routesByRouteNumber = new Dictionary<string, Route>()
        {
            { "01", new Route() { Name = "Route 1", Id = "01" } },
            { "02", new Route() { Name = "Route 2", Id = "02" } },
            { "03", new Route() { Name = "Route 3", Id = "03" } },
            { "04", new Route() { Name = "Route 4", Id = "04" } },
            { "06", new Route() { Name = "Route 6", Id = "06" } },
            { "09", new Route() { Name = "Route 9", Id = "09" } }
        };

        /// <summary>
        /// Gets all currently active routes in the Madison Metro system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Route> GetAllRoutes()
        {
            return routesByRouteNumber.Values;
        }

        /// <summary>
        /// Gets all ids for currently active routes in the Madison Metro system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetRouteIds()
        {
            return routesByRouteNumber.Keys;
        }

        /// <summary>
        /// Gets a specific route using the id and returns null if the route doesn't exist
        /// </summary>
        /// <param name="id">The unique id of the rotue</param>
        /// <returns></returns>
        public static Route GetRouteById(string id)
        {
            if (routesByRouteNumber.ContainsKey(id))
            {
                return routesByRouteNumber[id];
            }

            return null;
        }

        /// <summary>
        /// Returns all route legs for the specified route to aid in
        /// rendering the path of that route
        /// </summary>
        /// <param name="route">A route with a valid route id</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the route is invalid or the id doesn't exist</exception>
        public static async Task<IEnumerable<RoutePoint>> GetRoutePath(Route route)
        {
            if (route == null)
            {
                throw new ArgumentNullException("A route with an id is required to get the path of the route");
            }

            return await GetRoutePath(route.Id);
        }

        /// <summary>
        /// Returns all route legs for the specified route id to aid in
        /// rendering the path of that route
        /// </summary>
        /// <param name="routeId">The unique identifier for a route</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the route id is null or the id doesn't exist</exception>
        public static async Task<IEnumerable<RoutePoint>> GetRoutePath(string routeId)
        {
            if (routeId == null || !routesByRouteNumber.ContainsKey(routeId))
            {
                throw new ArgumentNullException("A valid route id is required to retrieve path data");
            }

            List<RoutePoint> routeLegs = new List<RoutePoint>();

            string paramValue = await UpdateRoute(routeId);

            if (paramValue != null)
            {
                // Madison Metro separates related data by * and then uses other
                // delimeters on other values
                string[] parameters = paramValue.Split('*');
                string[] paths = parameters[1].Split('|');
                string[] penParms = parameters[2].Split(',');
                string[] latLongs = paths[0].Split(';');
                string[] coordinates = latLongs[0].Split(' ');

                // roughly the center of the route
                decimal latitude = decimal.Parse(coordinates[1]);
                decimal longitude = decimal.Parse(coordinates[0]);

                for (int path = 0; path < paths.Length; path++)
                {
                    string[] points = paths[path].Split(';');
                    for (int point = 0; point < points.Length; point++)
                    {
                        string[] latLngs = points[point].Split(' ');
                        if (latLngs.Length > 1)
                        {
                            RoutePoint routeLeg = new RoutePoint();
                            routeLeg.PathOrder = path;
                            routeLeg.PointOrder = point;
                            routeLeg.Latitude = decimal.Parse(latLngs[1]);
                            routeLeg.Longitude = decimal.Parse(latLngs[0]);

                            routeLegs.Add(routeLeg);
                        }
                    }
                }
            }

            return routeLegs;
        }

        private static async Task<string> UpdateRoute(string routeId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(@"http://webwatch.cityofmadison.com/webwatch/Scripts/Route{0}_trace.js", routeId));
                    request.Headers.Add("Accept", "application/x-javascript");
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        // data is returned as a javascript variable, so remove the javascript portions
                        string paramValue = await response.Content.ReadAsStringAsync();
                        paramValue = paramValue.Replace("var parms=\"", string.Empty).Replace("\"", string.Empty);
                        return paramValue;
                    }
                }
                catch (Exception)
                {
                    // don't do anything on service exception
                }

                return null;
            }
        }

        private static async Task<string[]> UpdateRouteAndLocationData(string routeId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    int secondsSinceEpoch = (int)t.TotalSeconds;
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(@"http://webwatch.cityofmadison.com/webwatch/UpdateWebMap.aspx?u={0}&timestamp={1}", routeId, secondsSinceEpoch));
                    request.Headers.Add("Accept", "text/html");
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string busDataResponse = await response.Content.ReadAsStringAsync();
                        string[] busData = busDataResponse.Split('*');

                        return busData;
                    }                    
                }
                catch (Exception)
                {
                    // don't do anything on service exception
                }

                return null;
            }
        }

        private List<VehicleLocation> ParseVehicles(string vehicleData, string routeId)
        {
            List<VehicleLocation> locations = new List<VehicleLocation>();

            if (string.IsNullOrEmpty(vehicleData))
            {
                return locations;
            }

            string[] vehicles = vehicleData.Split(';');
            for (int i = 0; i < vehicles.Length; i++)
            {
                string[] vehicleInfo = vehicles[i].Split('|');
                if (vehicleInfo.Length >= 4)
                {
                    decimal latitude = decimal.Parse(vehicleInfo[0]);
                    decimal longitude = decimal.Parse(vehicleInfo[1]);

                    int direction = int.Parse(vehicleInfo[2]);

                    VehicleLocation location = new VehicleLocation();
                    location.Number = vehicleInfo[3];
                    location.Data = vehicleInfo[3];
                    location.Latitude = latitude;
                    location.Longitude = longitude;
                    location.Direction = direction;
                    location.RouteId = routeId;

                    locations.Add(location);
                }
            }

            return locations;
        }

        private List<RouteStop> ParseStops(string stopData, int stopType)
        {
            List<RouteStop> busStops = new List<RouteStop>();
            if (string.IsNullOrEmpty(stopData))
            {
                return busStops;
            }

            //stopID|lat|lon|name|dir|time time time;
            string[] stops = stopData.Split(';');
            foreach (string stop in stops)
            {
                if (!string.IsNullOrEmpty(stop))
                {
                    string[] stopInfo = stop.Split('|');

                    decimal latitude = decimal.Parse(stopInfo[0]);
                    decimal longitude = decimal.Parse(stopInfo[1]);

                    if (latitude == 0.0m || longitude == 0.0m)
                    {
                        continue;
                    }

                    RouteStop newStop = new RouteStop();
                    newStop.Latitude = latitude;
                    newStop.Longitude = longitude;
                    newStop.Name = stopInfo[2];
                    newStop.StopType = stopType;
                    newStop.StopId = string.Format("{0}{1}", latitude, longitude);
                    newStop.Direction = stopInfo[3];

                    busStops.Add(newStop);
                }
            }

            return busStops;
        }

        private List<RouteStopTime> ParseStopTimes(string stopData, int stopType, string routeId)
        {
            List<RouteStopTime> routeStopTimes = new List<RouteStopTime>();
            if (string.IsNullOrEmpty(stopData))
            {
                return routeStopTimes;
            }

            //stopID|lat|lon|name|dir|time time time;
            string[] stops = stopData.Split(';');
            foreach (string stop in stops)
            {
                if (!string.IsNullOrEmpty(stop))
                {
                    string[] stopInfo = stop.Split('|');

                    decimal latitude = decimal.Parse(stopInfo[0]);
                    decimal longitude = decimal.Parse(stopInfo[1]);

                    if (latitude == 0.0m || longitude == 0.0m)
                    {
                        continue;
                    }

                    string times = stopInfo[4]; // next n buses
                    string[] arrayTimes = times.Split(new string[] { "<br>" }, StringSplitOptions.None);


                    if (string.IsNullOrEmpty(times))
                    {
                        times = @"No buses available";
                    }

                    string stopId = string.Format("{0}{1}", latitude, longitude);
                    RouteStopTime stopTime = new RouteStopTime();
                    stopTime.Data = times;
                    stopTime.RouteId = routeId;
                    stopTime.StopId = stopId;
                    routeStopTimes.Add(stopTime);
                }
            }

            return routeStopTimes;
        }
    }
}
