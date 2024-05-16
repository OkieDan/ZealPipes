using System;
using ZealPipes.Common.Models;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class PipeCmdMessageReceivedEventArgs : EventArgs
        {
            public PipeCmdMessage Message { get; private set; }
            public int ProcessId { get; private set; }

            public PipeCmdMessageReceivedEventArgs(int processId, PipeCmdMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
    }
}


