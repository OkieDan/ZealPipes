using System;
using ZealPipes.Common.Models;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class GroupMessageReceivedEventArgs : EventArgs
        {
            public GroupMessage Message { get; private set; }
            public int ProcessId { get; private set; }

            public GroupMessageReceivedEventArgs(int processId, GroupMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
    }
}


