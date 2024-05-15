using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZealPipes.Common.Models
{
    public class GaugeMessage
    {
        public class GaugeData
        {
            [JsonPropertyName("type")]
            public GaugeType Type { get; set; }

            [JsonPropertyName("text")]
            public string Text { get; set; }
            
            [JsonPropertyName("value")]
            public int Value { get; set; }
        }
        public GaugeMessage(string character, string pipeMessageData)
        {
            Type = PipeMessageType.Gauge;
            Character = character;
            Data = JsonSerializer.Deserialize<GaugeData>(pipeMessageData);
        }
        public PipeMessageType Type { get; set; }

        public string Character { get; set; }

        public GaugeData Data { get; set; }
    }
}
