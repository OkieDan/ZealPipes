using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using ZealPipes.Common;
using ZealPipes.Common.Models;
using ZealPipes.Services.Helpers;
using static ZealPipes.Services.Helpers.ZealPipeReader;
namespace ZealPipes.Services
{
    public partial class ZealMessageService
    {
        private readonly ProcessMonitor _processMonitor;
        private readonly ZealSettings _zealSettings;
        private readonly ZealPipeReader _zealPipeReader;
        private List<int> _processList = new List<int>();
        public event EventHandler<LogMessageReceivedEventArgs> OnLogMessageReceived;
        public event EventHandler<LabelMessageReceivedEventArgs> OnLabelMessageReceived;
        public event EventHandler<GaugeMessageReceivedEventArgs> OnGaugeMessageReceived;

        public ZealMessageService(IConfiguration configuration, ProcessMonitor processMonitor, ZealSettings zealSettings, ZealPipeReader zealPipeReader)
        {
            _processMonitor = processMonitor;
            _zealSettings = zealSettings;
            _zealPipeReader = zealPipeReader;
            _processMonitor.OnNewProcessFound += ProcessMonitor_OnNewProcessFound;
            _zealPipeReader.OnPipeMessageReceived += ZealPipeReader_OnPipeMessageReceived;
            StartProcessing();
        }

        private void ProcessMonitor_OnNewProcessFound(object sender, ProcessMonitor.NewProcessFoundEvent e)
        {
            _processList.Add(e.ProcessId);
            new Thread(() => { _zealPipeReader.StartReading(e.ProcessId); }).Start();
        }

        private void ZealPipeReader_OnPipeMessageReceived(object sender, PipeMessageReceivedEventArgs e)
        {
            //Console.WriteLine($"Message from Zeal Pipe {e.ProcessId}: {e.Message.Character}: {e.Message.Type}: {e.Message.DataLen}: {e.Message.Data}");
            switch ((PipeMessageType)e.Message.Type)
            {
                case PipeMessageType.LogText: // log
                    OnLogMessageReceived?.Invoke(this, new LogMessageReceivedEventArgs(
                            e.ProcessId,
                            new LogMessage(e.Message.Character,e.Message.Data)                            
                        ));
                    break;
                case PipeMessageType.Label: // label
                    OnLabelMessageReceived?.Invoke(this, new LabelMessageReceivedEventArgs(
                            e.ProcessId,
                            new LabelMessage(e.Message.Character,  e.Message.Data)                            
                        ));
                    break;
                case PipeMessageType.Gauge: // gauge
                    OnGaugeMessageReceived?.Invoke(this, new GaugeMessageReceivedEventArgs(
                            e.ProcessId,
                            new GaugeMessage(e.Message.Character, e.Message.Data)                            
                        ));
                    break;

            }
            // Further process the message here...
        }

        public void StartProcessing()
        {
            _processMonitor.StartMonitoring();
        }
        public void StopProcessing()
        {
            _processMonitor.StopMonitoring();
            _processList.ForEach(x =>
            {
                _zealPipeReader.StopReading(x);
            });
        }
    }
}


