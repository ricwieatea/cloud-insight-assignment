using System.Text.Json.Serialization;

namespace RandomPayloadAssignment.Models
{
    public class RandomPayload
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("entries")]
        public Entry[] Entries { get; set; }
    }

    public class Entry
    {
        [JsonPropertyName("API")]
        public string Api { get; set; }
        [JsonPropertyName("Description")]
        public string Description { get; set; }
        [JsonPropertyName("Auth")]
        public string Auth { get; set; }
        [JsonPropertyName("HTTPS")]
        public bool Https { get; set; }
        [JsonPropertyName("Cors")]
        public string Cors { get; set; }
        [JsonPropertyName("Link")]
        public string Link { get; set; }
        [JsonPropertyName("Category")]
        public string Category { get; set; }
    }
}
