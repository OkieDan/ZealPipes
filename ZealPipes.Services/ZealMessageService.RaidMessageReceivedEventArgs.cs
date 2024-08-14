using System;
using ZealPipes.Common.Models;

namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class RaidMessageReceivedEventArgs : EventArgs
        {
            public RaidMessage Message { get; private set; }
            public int ProcessId { get; private set; }

            public RaidMessageReceivedEventArgs(int processId, RaidMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
    }
}
