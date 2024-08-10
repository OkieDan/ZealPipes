using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static ZealPipes.Common.Models.GaugeMessage;
using static ZealPipes.Common.Models.LabelMessage;
using static ZealPipes.Common.Models.PlayerMessage;

namespace ZealPipes.Common.Models
{

    public class ZealCharacter
    {
        public string Name { get; set; }
        public int ProcessId { get; set; }
        public ZealCharacterDetail Detail { get; set; } = new ZealCharacterDetail();
        internal ZealCharacterDetail _partialCharacterDetail { get; set; } = new ZealCharacterDetail();
        
        public event EventHandler<ZealCharacterUpdatedEventArgs> OnCharacterUpdated;

        public class ZealCharacterUpdatedEventArgs : EventArgs
        {
            public ZealCharacter Character { get; private set; }
            public int ProcessId { get; private set; }

            public ZealCharacterUpdatedEventArgs(int processId, ZealCharacter character)
            {
                ProcessId = processId;
                Character = character;
            }
        }

        public ZealCharacter(string name, int processId)
        {
            Name = name;
            ProcessId = processId;
        }

        private void AddGaugeFragment(GaugeData data)
        {
            _partialCharacterDetail.GaugeData.Add(data);
        }

        private void AddLabelFragment(LabelData data)
        {
            _partialCharacterDetail.LabelData.Add(data);
        }

        public void UpdateCharacterData(GaugeData data)
        {
            AddGaugeFragment(data);
        }
        public void UpdateCharacterData(LabelData data)
        {
            AddLabelFragment(data);
        }
        public void UpdateCharacterData(RaidMessage.RaidData[] data)
        {
            _partialCharacterDetail.RaidData = data;
        }

        public void UpdateCharacterData(PlayerData data)
        {
            Detail.GaugeData = JsonSerializer.Deserialize<List<GaugeData>>(JsonSerializer.Serialize(_partialCharacterDetail.GaugeData));
            Detail.LabelData = JsonSerializer.Deserialize<List<LabelData>>(JsonSerializer.Serialize(_partialCharacterDetail.LabelData));
            Detail.RaidData = JsonSerializer.Deserialize<RaidMessage.RaidData[]>(JsonSerializer.Serialize(_partialCharacterDetail.RaidData));
            _partialCharacterDetail.GaugeData.Clear();
            _partialCharacterDetail.LabelData.Clear();
            this.Detail.PlayerData = data;
            OnCharacterUpdated?.Invoke(this, new ZealCharacterUpdatedEventArgs(ProcessId, this));
        }

        public class ZealCharacterDetail
        {
            public List<GaugeData> GaugeData { get; internal set; } = new List<GaugeData>();
            public List<LabelData> LabelData { get; internal set; } = new List<LabelData>();
            public PlayerData PlayerData { get; internal set; } = new PlayerData();
            public RaidMessage.RaidData[] RaidData { get; internal set; }
        }
    }
}
