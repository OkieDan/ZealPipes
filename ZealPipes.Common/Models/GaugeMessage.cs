using System;
using System.Collections.Generic;
using System.Text;
using static ZealPipes.Common.Models.LabelMessage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZealPipes.Common.Models
{
    public class GaugeMessage
    {
        public class GaugeData
        {
            //public GaugeData(GaugeType type, string value)
            //{
            //    Type = type;
            //    Value = value;
            //}
            [JsonPropertyName("type")]

            public GaugeType Type { get; set; }
            [JsonPropertyName("value")]

            public string Value { get; set; }
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
