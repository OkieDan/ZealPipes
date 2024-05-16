using System;
using ZealPipes.Common.Models;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class PlayerMessageReceivedEventArgs : EventArgs
        {
            public PlayerMessage Message { get; private set; }
            public int ProcessId { get; private set; }

            public PlayerMessageReceivedEventArgs(int processId, PlayerMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
    }
}


