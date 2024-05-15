using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static ZealPipes.Common.Models.GaugeMessage;

namespace ZealPipes.Common.Models
{
    public class PlayerMessage
    {
        public class PlayerData
        {
            [JsonPropertyName("zone")]
            public int ZoneId { get; set; }
        }
    
    [JsonPropertyName("zone")]
        public string Character { get; }
        public PlayerData Data { get; }
        public PlayerMessage(string character, string data)
        {
            Character = character;
            Data = JsonSerializer.Deserialize<PlayerData>(data);
        }

    }
}
