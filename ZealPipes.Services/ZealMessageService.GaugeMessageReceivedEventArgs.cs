using System;
using ZealPipes.Common.Models;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class GaugeMessageReceivedEventArgs : EventArgs
        {
            public GaugeMessage Message { get; private set; }
            public int ProcessId { get; private set; }

            public GaugeMessageReceivedEventArgs(int processId, GaugeMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
    }
}


