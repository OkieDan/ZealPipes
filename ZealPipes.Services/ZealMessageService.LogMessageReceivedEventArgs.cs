using System;
using ZealPipes.Common.Models;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class LogMessageReceivedEventArgs : EventArgs
        {
            public LogMessage Message { get; private set; }
            public int ProcessId { get; private set; }

            public LogMessageReceivedEventArgs(int processId, LogMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
    }
}


