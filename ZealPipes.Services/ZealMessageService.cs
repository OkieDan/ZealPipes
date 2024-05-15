using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using ZealPipes.Common;
using ZealPipes.Common.Models;
using ZealPipes.Services.Helpers;
using static ZealPipes.Common.Models.Character;
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
        public event EventHandler<PlayerMessageReceivedEventArgs> OnPlayerMessageReceived;
        public event EventHandler<CharacterUpdatedEventArgs> OnCharacterUpdated;
        public List<Character> CharacterList = new List<Character>();

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
            PipeMessageType pipeMessageType = (PipeMessageType)e.Message.Type;
            Character character;
            switch (pipeMessageType)
            {
                case PipeMessageType.LogText: // log
                    OnLogMessageReceived?.Invoke(this, new LogMessageReceivedEventArgs(
                            e.ProcessId,
                            new LogMessage(e.Message.Character,e.Message.Data)                            
                        ));
                    break;
                case PipeMessageType.Label: // label
                    var labelMessage = new LabelMessage(e.Message.Character, e.Message.Data);
                    character = CharacterList.Where(x => x.ProcessId == e.ProcessId && x.Name == labelMessage.Character).FirstOrDefault();
                    if (character == null)
                    {
                        character = new Character(labelMessage.Character, e.ProcessId);
                        character.OnCharacterUpdated += Character_OnCharacterUpdated;
                        CharacterList.Add(character);
                    }
                    character.UpdateCharacterData(labelMessage.Data);
                    OnLabelMessageReceived?.Invoke(this, new LabelMessageReceivedEventArgs(
                            e.ProcessId,
                            labelMessage
                        ));
                    break;
                case PipeMessageType.Gauge: // gauge
                    var gaugeMessage = new GaugeMessage(e.Message.Character, e.Message.Data);
                    character = CharacterList.Where(x => x.ProcessId == e.ProcessId && x.Name == gaugeMessage.Character).FirstOrDefault();
                    if (character == null)
                    {
                        character = new Character(gaugeMessage.Character, e.ProcessId);
                        character.OnCharacterUpdated += Character_OnCharacterUpdated;
                        CharacterList.Add(character);
                    }
                    character.UpdateCharacterData(gaugeMessage.Data);
                    OnGaugeMessageReceived?.Invoke(this, new GaugeMessageReceivedEventArgs(
                            e.ProcessId,
                            new GaugeMessage(e.Message.Character, e.Message.Data)                            
                        ));
                    break;
                case PipeMessageType.Player:
                    var playerMessage = new PlayerMessage(e.Message.Character, e.Message.Data);
                    character = CharacterList.Where(x => x.ProcessId == e.ProcessId && x.Name == playerMessage.Character).FirstOrDefault();
                    character.UpdateCharacterData(playerMessage.Data);

                    OnPlayerMessageReceived?.Invoke(this, new PlayerMessageReceivedEventArgs(
                            e.ProcessId,
                            new PlayerMessage(e.Message.Character, e.Message.Data)
                        ));
                    break;

            }
            // Further process the message here...
        }

        private void Character_OnCharacterUpdated(object sender, Character.CharacterUpdatedEventArgs e)
        {
            OnCharacterUpdated?.Invoke(this, e);
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