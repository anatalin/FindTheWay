using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FindTheWay.Common.Models;
using Newtonsoft.Json;

namespace FindTheWay.Common.Services.Implementation
{
    public class MapService : IMapService
    {
        private const string DockerBaseUrl = "http://findtheway_neo4j_1:7474";
        //private const string LocalhostBaseUrl = "http://http://localhost:7474";

        private readonly IHttpClientFactory _httpClientFactory;

        public MapService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<CoordinatesModel>> GetPathAsync(List<long> nodeIds)
        {
            long nodeStartId = nodeIds[0],
                nodeFinishId = nodeIds[1];

            var url = $"{DockerBaseUrl}/examples/astar/getPath?from={nodeStartId}&to={nodeFinishId}";
            Console.WriteLine($"URL is {url}");
            var msg = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient();
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"neo4j:Qwerty123")));

                var response = await httpClient.SendAsync(msg).ConfigureAwait(false);
                var responseStr = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var resp = JsonConvert.DeserializeObject<ClosestNodeResponse[]>(responseStr);

                return resp.Select(r => new CoordinatesModel
                {
                    NodeId = long.Parse(r.Id),
                    Latitude = r.Lat,
                    Longitude = r.Lon
                });
            }
        }

        public async Task<CoordinatesModel> ApproximatePointAsync(CoordinatesModel coords)
        {
            var url = $"{DockerBaseUrl}/examples/astar/closestNode?lat={coords.Latitude.ToString(CultureInfo.InvariantCulture)}&long={coords.Longitude.ToString(CultureInfo.InvariantCulture)}";
            Console.WriteLine($"URL is {url}");
            var msg = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = this._httpClientFactory.CreateClient();
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"neo4j:Qwerty123")));

                var response = await httpClient.SendAsync(msg).ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // todo: if not successful - throw a meaningful error.
                var resp = JsonConvert.DeserializeObject<ClosestNodeResponse>(responseContent);

                Debug.WriteLine($"Node.NodeId = {resp.Id}");
                return new CoordinatesModel
                {
                    NodeId = long.Parse(resp.Id),
                    Latitude = resp.Lat,
                    Longitude = resp.Lon
                };
            }
        }
    }
}
