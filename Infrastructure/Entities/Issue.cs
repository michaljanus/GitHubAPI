using System.Text.Json.Serialization;

namespace Infrastructure.Entities
{
    public class Issue
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;
    }
}
