using Newtonsoft.Json;

namespace FindTheWay.Common.Models
{
	public class ClosestNodeResponse
	{
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Lat { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public double Lon { get; set; }

        //public long NodeId { get; set; }

        //public Metadata Metadata { get; set; }

        //public NodeResponse Data { get; set; }
    }
}
