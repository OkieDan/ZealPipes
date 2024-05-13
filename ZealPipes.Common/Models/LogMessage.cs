using System;
using System.Collections.Generic;
using System.Text;

namespace ZealPipes.Common.Models
{
    public class LogMessage
    {
        public LogMessage(string character, string logText)
        {
            Type = PipeMessageType.LogText;
            Character = character;
            Value = logText;
        }
        public PipeMessageType Type { get; set; }

        public string Character { get; set; }

        public string Value { get; set; }
    }


}
