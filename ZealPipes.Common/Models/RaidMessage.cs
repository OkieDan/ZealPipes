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
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int Group { get; set; }

            [JsonPropertyName("level")]
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public int Level { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("rank")]
            public string Rank { get; set; }

            [JsonPropertyName("loc")]
            public Vec3 Position { get; set; }

            [JsonPropertyName("heading")]
            public float Heading { get; set; }

            [JsonPropertyName("spawn_id")]
            public int? SpawnId { get; set; }

            [JsonPropertyName("hp_current")]
            public int? HpCurrent { get; set; }

            [JsonPropertyName("hp_max")]
            public int? HpMax { get; set; }

            [JsonPropertyName("zone_id")]
            public int? ZoneId { get; set; }
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
