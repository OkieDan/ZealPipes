using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZealPipes.Common.Models
{
    public class Vec3
    {
        [JsonPropertyName("x")]
        public float X { get; set; }
        [JsonPropertyName("y")]
        public float Y { get; set; }
        [JsonPropertyName("z")]
        public float Z { get; set; }
    }
    public class PlayerMessage
    {
        public class PlayerData
        {
            [JsonPropertyName("zone")]
            public int ZoneId { get; set; }
            [JsonPropertyName("location")]
            public Vec3 Position { get; set; }
            [JsonPropertyName("heading")]
            public float heading { get; set; }
            [JsonPropertyName("autoattack")]
            public bool AutoAttack { get; set; }
        }

        [JsonPropertyName("zone")]
        public string Character { get; }
        public PlayerData Data { get; }
        public PipeMessageType Type { get; } = PipeMessageType.Player;
        public PlayerMessage(string character, string data)
        {
            Character = character;
            Data = JsonSerializer.Deserialize<PlayerData>(data);
        }

    }
}
