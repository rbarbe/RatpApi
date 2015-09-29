using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ratp.RatpApi.Models;
using Xunit;

namespace Ratp.RatpApi.Test
{
    public class Functional
    {
        [Fact]
        public async Task Test()
        {
            var api = new Api();

            var startElysee = new GeoLocation
            {
                Longitude = 2.316749,
                Latitude = 48.870663
            };
            var endTourEiffel = new GeoLocation
            {
                Longitude = 2.294555,
                Latitude = 48.858465
            };
            var arrivalTime = DateTimeOffset.Now.AddDays(3);

            var networks = new List<Network> {Network.Bus};

            var s =
                await
                    api.GetItinerary(startElysee, endTourEiffel, null, arrivalTime, networks, JourneyPreference.MinWait,
                        false, false, false);

            Assert.False(string.IsNullOrWhiteSpace(s));
        }
    }
}