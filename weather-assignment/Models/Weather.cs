
using Microsoft.Azure.Cosmos.Table;
using System.Text.Json.Serialization;
using WeatherAssignment.Azure;

namespace WeatherAssignment.Models
{

    public class WeatherResult
    {
        [JsonPropertyName("current")]
        public Weather CurrentUnits { get; set; }
    }

    public class Weather : CosmosDocument
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature { get; set; }
        [JsonPropertyName("cloud_cover")]
        public double CloudCover { get; set; }
        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Location
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string ApiUrl { get; set; }
    }
}
