using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FindTheWay.Common.Models
{
    [DataContract]
	public class CoordinatesModel
    {
        [DataMember(Name = "NodeId")]
        [JsonProperty(PropertyName = "NodeId")]
        public long? NodeId { get; set; }

        [DataMember(Name = "Latitude")]
        [JsonProperty(PropertyName = "Latitude")]
        public double Latitude { get; set; }

        [DataMember(Name = "Longitude")]
        [JsonProperty(PropertyName = "Longitude")]
        public double Longitude { get; set; }
    }
}
