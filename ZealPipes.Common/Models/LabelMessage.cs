using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZealPipes.Common.Models
{
    public class LabelMessage
    {
        public class LabelData
        {
            //public LabelData(LabelType type, string value)
            //{
            //    JsonSerializer.Deserialize<LabelData>(value);
            //    Type = type;
            //    Value = value;
            //}
            [JsonPropertyName("type")]
            public LabelType Type { get; set; }
            [JsonPropertyName("value")]
            public string Value { get; set; }
        }
        public LabelMessage(string character, string pipeMessageData)
        {
            Type = PipeMessageType.Label;
            Character = character;
            Data = JsonSerializer.Deserialize<LabelData>(pipeMessageData);            
        }
        public PipeMessageType Type { get; set; }

        public string Character { get; set; }

        public LabelData Data { get; set; }
    }
}
