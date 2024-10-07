using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZealPipes.Common.Models
{
    public class GroupMessage
    {
        public class GroupData
        {
            [JsonPropertyName("loc")]
            public Vec3 Position { get; set; }
            [JsonPropertyName("heading")]
            public float Heading { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
        public GroupMessage(string character, string pipeMessageData)
        {
            Type = PipeMessageType.Group;
            Character = character;
            Data = JsonSerializer.Deserialize<GroupData[]>(pipeMessageData);            
        }
        public PipeMessageType Type { get; set; }

        public string Character { get; set; }

        public GroupData[] Data { get; set; }
    }
}