using System;
using ZealPipes.Common.Models;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        public class ConnectionTerminatedEventArgs : EventArgs
        {
            public int ProcessId { get; private set; }

            public ConnectionTerminatedEventArgs(int processId)
            {
                ProcessId = processId;
            }
        }
    }
}