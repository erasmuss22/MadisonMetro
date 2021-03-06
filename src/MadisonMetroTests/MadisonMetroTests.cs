﻿using System;
using System.Linq;
using MadisonMetroSDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadisonMetroTests
{
    [TestClass]
    public class MadisonMetroTests
    {
        [TestMethod]
        public void GetAllRoutes_ReturnsRoutes()
        {
            IEnumerable<Route> routes = MadisonMetro.GetAllRoutes();
            Assert.IsTrue(routes.Count() > 0);
            Assert.IsTrue(routes.All(r => r.Name != null && r.Id != null));
        }

        [TestMethod]
        public void GetAllRoutesIdentifiers_ReturnsIdentifiers()
        {
            IEnumerable<string> routes = MadisonMetro.GetRouteIds();
            Assert.IsTrue(routes.Count() > 0);
            Assert.IsTrue(routes.All(r => routes.Count(c => r == c) == 1));
        }

        [TestMethod]
        public void GetRouteByIdentifier_ReturnsRoute()
        {
            Route route = MadisonMetro.GetRouteById("06");
            Assert.IsNotNull(route);
            Assert.AreEqual("06", route.Id);
        }

        [TestMethod]
        public void GetRouteByIdentifier_ReturnsNull()
        {
            Route route = MadisonMetro.GetRouteById("blah");
            Assert.IsNull(route);
        }

        [TestMethod]
        public async Task GetCurrentLocation_Valid()
        {
            RouteCurrentData data = await MadisonMetro.GetRouteCurrentData("06");
            Assert.IsTrue(data.VehicleData.Count > 0);
            Assert.IsTrue(data.VehicleData.All(v => v.Number != null && v.FinalStop != null && v.NextStop != null));
        }
    }
}
