using System.Text.Json;
using System.Text.Json.Serialization;
using static ZealPipes.Common.Models.GaugeMessage;

namespace ZealPipes.Common.Models
{


    public class PipeCmdMessage
    {
        public class PipeCmdData
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }    // chat message content            
        }

        public PipeCmdMessage(string character, string pipeMessageData)
        {
            Type = PipeMessageType.PipeCmd;
            Character = character;
            Data.Text = pipeMessageData;
        }
        public PipeMessageType Type { get; set; }

        public string Character { get; set; }

        public PipeCmdData Data { get; set; } = new PipeCmdData();
    }
}
