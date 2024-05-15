using System.Text.Json;
using System.Text.Json.Serialization;
using static ZealPipes.Common.Models.GaugeMessage;

namespace ZealPipes.Common.Models
{


    public class LogMessage
    {
        public class LogData
        {
            [JsonPropertyName("type")]
            public LogType Type { get; set; }

            [JsonPropertyName("text")]
            public string Text { get; set; }    // chat message content            
        }

        public LogMessage(string character, string pipeMessageData)
        {
            Type = PipeMessageType.LogText;
            Character = character;
            Data = JsonSerializer.Deserialize<LogData>(pipeMessageData);
        }
        public PipeMessageType Type { get; set; }

        public string Character { get; set; }

        public LogData Data { get; set; }
    }
}
