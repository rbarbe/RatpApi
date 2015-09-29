using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ratp.RatpApi.Models;

namespace Ratp.RatpApi
{
    public class Api
    {
        private readonly HttpClient _httpClient;

        public Api()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://apixha.ixxi.net")};
        }

        public async Task<string> GetItinerary(GeoLocation start, GeoLocation end, DateTimeOffset? leaveTime,
            DateTimeOffset? arrivalTime, List<Network> prefNetworks, JourneyPreference? preference,
            bool withTrafficEvents, bool withText, bool withDetails)
        {
            var queryStringParametters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("startPointLon", start.Longitude.ToString()),
                new KeyValuePair<string, string>("startPointLat", start.Latitude.ToString()),
                new KeyValuePair<string, string>("keyapp", "FvChCBnSetVgTKk324rO"),
                new KeyValuePair<string, string>("engine", "ratp"),
                new KeyValuePair<string, string>("endPointLon", end.Longitude.ToString()),
                new KeyValuePair<string, string>("endPointLat", end.Latitude.ToString()),
                new KeyValuePair<string, string>("cmd", "getItinerary")
            };

            if (withTrafficEvents)
            {
                queryStringParametters.Add(new KeyValuePair<string, string>("withTrafficEvents", "true"));
            }

            if (withText)
            {
                queryStringParametters.Add(new KeyValuePair<string, string>("withText", "true"));
            }

            if (withDetails)
            {
                queryStringParametters.Add(new KeyValuePair<string, string>("withDetails", "true"));
            }

            if (leaveTime == null && arrivalTime == null)
            {
                queryStringParametters.Add(new KeyValuePair<string, string>("leaveTime", ToApix(DateTimeOffset.Now)));
            }
            else if (leaveTime != null && arrivalTime != null)
            {
                throw new ArgumentException("Can't set both leaveTime and arrivalTime");
            }
            else if (leaveTime != null)
            {
                queryStringParametters.Add(new KeyValuePair<string, string>("leaveTime", ToApix(leaveTime.Value)));
            }
            else
            {
                queryStringParametters.Add(new KeyValuePair<string, string>("arrivalTime", ToApix(arrivalTime.Value)));
            }

            if (preference != null)
            {
                queryStringParametters.Add(new KeyValuePair<string, string>("prefJourney",
                    preference.ToString().PascalCaseToCamelCase()));
            }

            if (prefNetworks != null && prefNetworks.Any())
            {
                var prefNetworksStr = string.Join(",", prefNetworks.Select(s => s.ToString().ToLower()));

                queryStringParametters.Add(new KeyValuePair<string, string>("prefNetworks", prefNetworksStr));
            }

            HttpContent queryString = new FormUrlEncodedContent(queryStringParametters);
            var response =
                await _httpClient.GetAsync(new Uri("/APIX?" + await queryString.ReadAsStringAsync(), UriKind.Relative));

            return await response.Content.ReadAsStringAsync();
        }

        private static string ToApix(DateTimeOffset time)
        {
            var timeStr = time.ToString("yyyy-MM-ddTHH:mm:ss ");
            timeStr += time.ToString("zzzz").Replace("+", "").Replace(":", "");
            return timeStr;
        }
    }
}