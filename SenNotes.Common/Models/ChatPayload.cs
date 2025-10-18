using System.Text.Json.Serialization;

namespace SenNotes.Common.Models
{
    public class ChatPayload
    {
        public string? Model { get; set; }
        public List<Message>? Messages { get; set; }

        [JsonPropertyName("response_format")]
        public string Format = "{\"type\": \"json_object\"}";
    }

    public class Message
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
        
    }
}