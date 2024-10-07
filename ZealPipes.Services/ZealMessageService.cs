using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ZealPipeReader _zealPipeReader;
        private readonly ConcurrentDictionary<int, byte> _processList = new ConcurrentDictionary<int, byte>();
        private readonly ConcurrentBag<ZealCharacter> _characterList = new ConcurrentBag<ZealCharacter>();

        public event EventHandler<LogMessageReceivedEventArgs> OnLogMessageReceived;
        public event EventHandler<LabelMessageReceivedEventArgs> OnLabelMessageReceived;
        public event EventHandler<GaugeMessageReceivedEventArgs> OnGaugeMessageReceived;
        public event EventHandler<PlayerMessageReceivedEventArgs> OnPlayerMessageReceived;
        public event EventHandler<ZealCharacter.ZealCharacterUpdatedEventArgs> OnCharacterUpdated;
        public event EventHandler<PipeCmdMessageReceivedEventArgs> OnPipeCmdMessageReceived;
        public event EventHandler<ConnectionTerminatedEventArgs> OnConnectionTerminated;
        public event EventHandler<RaidMessageReceivedEventArgs> OnRaidMessageReceived;
        public event EventHandler<GroupMessageReceivedEventArgs> OnGroupMessageReceived;

        public ZealMessageService(ProcessMonitor processMonitor, ZealPipeReader zealPipeReader)
        {
            _processMonitor = processMonitor;
            _zealPipeReader = zealPipeReader;
            _processMonitor.OnNewProcessFound += ProcessMonitor_OnNewProcessFound;
            _zealPipeReader.OnPipeMessageReceived += ZealPipeReader_OnPipeMessageReceived;
            _zealPipeReader.OnConnectionTerminated += ZealPipeReader_OnConnectionTerminated;
            StartProcessing();
        }

        private void ZealPipeReader_OnConnectionTerminated(object sender, ConnectionTerminatedEventArgs e)
        {
            OnConnectionTerminated?.Invoke(this, e);
        }

        private void ProcessMonitor_OnNewProcessFound(object sender, ProcessMonitor.NewProcessFoundEvent e)
        {
            if (_processList.TryAdd(e.ProcessId, 0))
            {
                Task.Run(() => _zealPipeReader.StartReading(e.ProcessId));
            }
        }

        private ZealCharacter GetOrCreateCharacter(int processId, string characterName)
        {
            var character = _characterList.FirstOrDefault(x => x.ProcessId == processId && x.Name == characterName);
            if (character == null)
            {
                character = new ZealCharacter(characterName, processId);
                character.OnCharacterUpdated += Character_OnCharacterUpdated;
                _characterList.Add(character);
            }
            return character;
        }

        private void ZealPipeReader_OnPipeMessageReceived(object sender, PipeMessageReceivedEventArgs e)
        {
            var message = e.Message;
            var characterName = message.Character;
            var processId = e.ProcessId;
            var data = message.Data;

            var character = GetOrCreateCharacter(processId, characterName);
            var pipeMessageType = (PipeMessageType)message.Type;

            switch (pipeMessageType)
            {
                case PipeMessageType.LogText:
                    OnLogMessageReceived?.Invoke(this, new LogMessageReceivedEventArgs(processId, new LogMessage(characterName, data)));
                    break;

                case PipeMessageType.PipeCmd:
                    OnPipeCmdMessageReceived?.Invoke(this, new PipeCmdMessageReceivedEventArgs(processId, new PipeCmdMessage(characterName, data)));
                    break;

                case PipeMessageType.Label:
                    var labelMessage = new LabelMessage(characterName, data);
                    foreach (var label in labelMessage.Data)
                    {
                        character.UpdateCharacterData(label);
                    }
                    OnLabelMessageReceived?.Invoke(this, new LabelMessageReceivedEventArgs(processId, labelMessage));
                    break;

                case PipeMessageType.Gauge:
                    var gaugeMessage = new GaugeMessage(characterName, data);
                    foreach (var gauge in gaugeMessage.Data)
                    {
                        character.UpdateCharacterData(gauge);
                    }
                    OnGaugeMessageReceived?.Invoke(this, new GaugeMessageReceivedEventArgs(processId, gaugeMessage));
                    break;

                case PipeMessageType.Player:
                    var playerMessage = new PlayerMessage(characterName, data);
                    character.UpdateCharacterData(playerMessage.Data);
                    OnPlayerMessageReceived?.Invoke(this, new PlayerMessageReceivedEventArgs(processId, playerMessage));
                    break;

                case PipeMessageType.Raid:
                    var raidMessage = new RaidMessage(characterName, data);
                    //foreach (var raid in raidMessage.Data)
                    //{
                    //    character.UpdateCharacterData(raid);
                    //}
                    character.UpdateCharacterData(raidMessage.Data);
                    OnRaidMessageReceived?.Invoke(this, new RaidMessageReceivedEventArgs(processId, raidMessage));
                    break;
                case PipeMessageType.Group:
                    OnGroupMessageReceived?.Invoke(this, new GroupMessageReceivedEventArgs(
                            e.ProcessId,
                            new GroupMessage(e.Message.Character, e.Message.Data)
                        ));
                    break;

                default:
                    break;
            }
        }

        private void Character_OnCharacterUpdated(object sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
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
            foreach (var processId in _processList.Keys)
            {
                _zealPipeReader.StopReading(processId);
            }
        }
    }
}
