using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FindTheWay.Common.Models;
using Newtonsoft.Json;

namespace FindTheWay.Common.Services.Implementation
{
    public class MapService : IMapService
    {
        public IEnumerable<CoordinatesModel> GetPath(List<long> nodeIds)
        {
            long nodeStartId = nodeIds[0],
                nodeFinishId = nodeIds[1];

            var data = new
            {
                from = nodeStartId,
                to = nodeFinishId
            };

            var url = $"http://localhost:7474/examples/astar/getPath?from={nodeStartId}&to={nodeFinishId}";
            var msg = new HttpRequestMessage(HttpMethod.Get, url)
            {
                //Content = new StringContent(JsonConvert.SerializeObject(data), encoding: Encoding.UTF8, mediaType: "application/json")
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"neo4j:Qwerty123")));

                var response = httpClient.SendAsync(msg).ConfigureAwait(false).GetAwaiter().GetResult();
                var responseStr = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var resp = JsonConvert.DeserializeObject<ClosestNodeResponse[]>(responseStr);

                return resp.Select(r => new CoordinatesModel
                {
                    NodeId = Int64.Parse(r.Id),
                    Latitude = r.Lat,
                    Longitude = r.Lon
                });
            }
        }

        public CoordinatesModel ApproximatePoint(CoordinatesModel coords)
        {
            var data = new
            {
                lat = coords.Latitude,
                @long = coords.Longitude
            };

            var json = JsonConvert.SerializeObject(data);
            var url = $"http://localhost:7474/examples/astar/closestNode?lat={coords.Latitude.ToString(CultureInfo.InvariantCulture)}&long={coords.Longitude.ToString(CultureInfo.InvariantCulture)}";
            var msg = new HttpRequestMessage(HttpMethod.Get, url)
            {
                //Content = new StringContent(JsonConvert.SerializeObject(data), encoding: Encoding.UTF8, mediaType: "application/json")
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"neo4j:Qwerty123")));

                var response = httpClient.SendAsync(msg).ConfigureAwait(false).GetAwaiter().GetResult();
                var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                // if not successful - throw a meaningful error.
                var resp = JsonConvert.DeserializeObject<ClosestNodeResponse>(response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());

                Debug.WriteLine($"Node.NodeId = {resp.Id}");
                return new CoordinatesModel
                {
                    NodeId = Int64.Parse(resp.Id),
                    Latitude = resp.Lat,
                    Longitude = resp.Lon
                };
            }
        }
    }
}
