using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZealPipes.Common.Models
{
    public class RaidMessage
    {
        public class RaidData
        {
            [JsonPropertyName("class")]
            public string Class { get; set; }

            [JsonPropertyName("group")]
            private string Group_Str { get; set; }
            public int Group 
            {
                get 
                {
                    if (int.TryParse(Group_Str, out int group))
                    {
                        return group;
                    }
                    else
                    {
                        return 0;
                    }
                } 
            }

            [JsonPropertyName("level")]
            private string Level_Str { get; set; }
            public int Level
            {
                get
                {
                    if (int.TryParse(Level_Str, out int level))
                    {
                        return level;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("rank")]
            public string Rank { get; set; }

            [JsonPropertyName("loc")]
            public Vec3 Position { get; set; }

            [JsonPropertyName("heading")]
            public float Heading { get; set; }
        }

        public RaidMessage(string character, string pipeMessageData)
        {
            Type = PipeMessageType.Raid;
            Character = character;
            Data = JsonSerializer.Deserialize<RaidData[]>(pipeMessageData).Where(x => !string.IsNullOrEmpty(x.Name)).ToArray();
        }

        public PipeMessageType Type { get; set; }

        public string Character { get; set; }

        public RaidData[] Data { get; set; }
    }
}
