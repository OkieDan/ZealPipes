using System.Collections.Generic;
using System.Threading;
using System;
using System.Diagnostics;
using ZealPipes.Common;

namespace ZealPipes.Services.Helpers
{
    public class ProcessMonitor
    {
        internal class NewProcessFoundEvent : EventArgs
        {
            public int ProcessId { get; private set; }

            public NewProcessFoundEvent(int processId)
            {
                ProcessId = processId;
            }
        }
        private readonly ZealSettings _zealSettings;
        internal event EventHandler<NewProcessFoundEvent> OnNewProcessFound;
        private bool _isMonitoring;
        private HashSet<int> _connectedProcesses = new HashSet<int>();

        public ProcessMonitor(ZealSettings zealSettings)
        {
            _zealSettings = zealSettings;
        }

        internal void StartMonitoring()
        {
            if (_isMonitoring)
            {
                Console.WriteLine("Monitoring is already running.");
                return;
            }
            _isMonitoring = true;

            Thread monitorThread = new Thread(() =>
            {
                while (_isMonitoring)
                {
                    Process[] processes = Process.GetProcessesByName(_zealSettings.EqProcessName);

                    foreach (Process process in processes)
                    {
                        if (!_connectedProcesses.Contains(process.Id))
                        {
                            _connectedProcesses.Add(process.Id);
                            OnNewProcessFound?.Invoke(this, new NewProcessFoundEvent(process.Id));
                        }
                    }

                    Thread.Sleep(500); // Check every 500 milliseconds
                }
            });
            monitorThread.Start();
        }

        internal void StopMonitoring()
        {
            _isMonitoring = false;
        }
    }
}
