using System.Text.Json.Serialization;

namespace ZealPipes.Services.Models
{
    internal class PipeMessage
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("data_len")]
        public uint DataLen { get; set; }

        [JsonPropertyName("character")]
        public string Character { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }
    }


}
