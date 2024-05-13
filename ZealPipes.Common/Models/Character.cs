using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static ZealPipes.Common.Models.GaugeMessage;
using static ZealPipes.Common.Models.LabelMessage;

namespace ZealPipes.Common.Models
{

    public class Character
    {
        public string Name { get; set; }
        public int ProcessId { get; set; }
        public CharacterDetail Detail { get; set; } = new CharacterDetail();
        internal CharacterDetail _partialCharacterDetail { get; set; } = new CharacterDetail();
        public event EventHandler<CharacterUpdatedEventArgs> OnCharacterUpdated;

        public Character(string name, int processId)
        {
            Name = name;
            ProcessId = processId;
        }
        public class CharacterUpdatedEventArgs : EventArgs
        {
            public Character Character { get; private set; }
            public int ProcessId { get; private set; }

            public CharacterUpdatedEventArgs(int processId, Character character)
            {
                ProcessId = processId;
                Character = character;
            }
        }

        private void AddGaugeFragment(GaugeData data)
        {
            if (data.Type == Enum.GetValues(typeof(GaugeType)).Cast<GaugeType>().First() && _partialCharacterDetail.GaugeData.Count > 1 && _partialCharacterDetail.LabelData.Count > 1)
            {
                Detail.GaugeData = JsonSerializer.Deserialize<List<GaugeData>>(JsonSerializer.Serialize(_partialCharacterDetail.GaugeData));
                Detail.LabelData = JsonSerializer.Deserialize<List<LabelData>>(JsonSerializer.Serialize(_partialCharacterDetail.LabelData));
                _partialCharacterDetail.GaugeData.Clear();
                _partialCharacterDetail.LabelData.Clear();
                OnCharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(ProcessId, this));
            }
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

        public class CharacterDetail
        {
            public List<GaugeData> GaugeData { get; internal set; } = new List<GaugeData>();
            public List<LabelData> LabelData { get; internal set; } = new List<LabelData>();

        }
    }
}
