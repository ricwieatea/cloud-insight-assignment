using System.Text.Json.Serialization;

namespace WeatherAssignment.Azure
{
    public class CosmosDocument
    {
        public string partitionKey { get; set; }
        public string id { get; set; }
    }
}
