using System;
using ZealPipes.Common.Models;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class LabelMessageReceivedEventArgs : EventArgs
        {
            public LabelMessage Message { get; private set; }
            public int ProcessId { get; private set; }

            public LabelMessageReceivedEventArgs(int processId, LabelMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
    }
}


