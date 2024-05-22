using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;
using ZealPipes.Common;
using ZealPipes.Common.Models;

namespace ZealPipes.ClientWinforms.Models
{
    public class Character
    {
        public string Name { get; internal set; }
        public int Level { get; internal set; }
        public int HP { get; internal set; }
        public int Mana { get; internal set; }
        public decimal Stamina { get; internal set; }
        public decimal Experience { get; internal set; }
        public decimal AltExp { get; internal set; }
        public decimal Target { get; internal set; }
        public decimal Casting { get; internal set; }
        public decimal Breath { get; internal set; }
        public decimal Memorize { get; internal set; }
        public decimal Scribe { get; internal set; }
        public decimal Group1HP { get; internal set; }
        public decimal Group2HP { get; internal set; }
        public decimal Group3HP { get; internal set; }
        public decimal Group4HP { get; internal set; }
        public decimal Group5HP { get; internal set; }
        public decimal PetHP { get; internal set; }
        public decimal Group1PetHP { get; internal set; }
        public decimal Group2PetHP { get; internal set; }
        public decimal Group3PetHP { get; internal set; }
        public decimal Group4PetHP { get; internal set; }
        public decimal Group5PetHP { get; internal set; }
        public decimal ExpPerHR { get; internal set; }

        public string Class { get; internal set; }
        public string Deity { get; internal set; }
        public int Strength { get; internal set; }
        public int StaminaLabel { get; internal set; }
        public int Dexterity { get; internal set; }
        public int Agility { get; internal set; }
        public int Wisdom { get; internal set; }
        public int Intelligence { get; internal set; }
        public int Charisma { get; internal set; }
        public int SaveVsPoison { get; internal set; }
        public int SaveVsDisease { get; internal set; }
        public int SaveVsFire { get; internal set; }
        public int SaveVsCold { get; internal set; }
        public int SaveVsMagic { get; internal set; }
        public int CurrentHP { get; internal set; }
        public int MaxHP { get; internal set; }
        public int HPPerc { get; internal set; }
        public int ManaPerc { get; internal set; }
        public int STAPerc { get; internal set; }
        public int CurrentMitigation { get; internal set; }
        public int CurrentOffense { get; internal set; }
        public int Weight { get; internal set; }
        public int MaxWeight { get; internal set; }
        public int ExpPerc { get; internal set; }
        public int AltExpPerc { get; internal set; }
        public string TargetName { get; internal set; }
        public int TargetHPPerc { get; internal set; }
        public string GroupMember1Name { get; internal set; }
        public string GroupMember2Name { get; internal set; }
        public string GroupMember3Name { get; internal set; }
        public string GroupMember4Name { get; internal set; }
        public string GroupMember5Name { get; internal set; }
        public int GroupMember1HPPerc { get; internal set; }
        public int GroupMember2HPPerc { get; internal set; }
        public int GroupMember3HPPerc { get; internal set; }
        public int GroupMember4HPPerc { get; internal set; }
        public int GroupMember5HPPerc { get; internal set; }
        public int GroupPet1HPPerc { get; internal set; }
        public int GroupPet2HPPerc { get; internal set; }
        public int GroupPet3HPPerc { get; internal set; }
        public int GroupPet4HPPerc { get; internal set; }
        public int GroupPet5HPPerc { get; internal set; }
        public string PlayerPetName { get; internal set; }
        public int PlayerPetHPPerc { get; internal set; }
        public string PlayerCurrentHPMaxHP { get; internal set; }
        public int CurrentAAPoints { get; internal set; }
        public string CurrentAAPerc { get; internal set; }
        public string LastName { get; internal set; }
        public string Title { get; internal set; }
        public string ExpPH { get; internal set; }
        public string TargetPetOwner { get; internal set; }
        public string CastingName { get; internal set; }

        private ZealCharacter _zealCharacter;

        private readonly Dictionary<GaugeType, Action<decimal>> _gaugeMappings;
        private readonly Dictionary<LabelType, Action<string>> _labelMappings;

        public Character(ZealCharacter zealCharacter)
        {
            _zealCharacter = zealCharacter;
            Name = zealCharacter.Name;

            // Initialize mappings
            _gaugeMappings = new Dictionary<GaugeType, Action<decimal>>
        {
            { GaugeType.HP, value => HP = (int)value },
            { GaugeType.Mana, value => Mana = (int)value },
            { GaugeType.Stamina, value => Stamina = value },
            { GaugeType.Experience, value => Experience = value },
            { GaugeType.AltExp, value => AltExp = value },
            { GaugeType.Target, value => Target = value },
            { GaugeType.Casting, value => Casting = value },
            { GaugeType.Breath, value => Breath = value },
            { GaugeType.Memorize, value => Memorize = value },
            { GaugeType.Scribe, value => Scribe = value },
            { GaugeType.Group1HP, value => Group1HP = value },
            { GaugeType.Group2HP, value => Group2HP = value },
            { GaugeType.Group3HP, value => Group3HP = value },
            { GaugeType.Group4HP, value => Group4HP = value },
            { GaugeType.Group5HP, value => Group5HP = value },
            { GaugeType.PetHP, value => PetHP = value },
            { GaugeType.Group1PetHP, value => Group1PetHP = value },
            { GaugeType.Group2PetHP, value => Group2PetHP = value },
            { GaugeType.Group3PetHP, value => Group3PetHP = value },
            { GaugeType.Group4PetHP, value => Group4PetHP = value },
            { GaugeType.Group5PetHP, value => Group5PetHP = value },
            { GaugeType.ExpPerHR, value => ExpPerHR = value },
            // Add more mappings as needed...
        };

            _labelMappings = new Dictionary<LabelType, Action<string>>
        {
            { LabelType.Name, value => Name = value },
            { LabelType.Level, value => Level = int.Parse(value) },
            { LabelType.Class, value => Class = value },
            { LabelType.Deity, value => Deity = value },
            { LabelType.Strength, value => Strength = int.Parse(value) },
            { LabelType.Stamina, value => StaminaLabel = int.Parse(value) },
            { LabelType.Dexterity, value => Dexterity = int.Parse(value) },
            { LabelType.Agility, value => Agility = int.Parse(value) },
            { LabelType.Wisdom, value => Wisdom = int.Parse(value) },
            { LabelType.Intelligence, value => Intelligence = int.Parse(value) },
            { LabelType.Charisma, value => Charisma = int.Parse(value) },
            { LabelType.SaveVsPoison, value => SaveVsPoison = int.Parse(value) },
            { LabelType.SaveVsDisease, value => SaveVsDisease = int.Parse(value) },
            { LabelType.SaveVsFire, value => SaveVsFire = int.Parse(value) },
            { LabelType.SaveVsCold, value => SaveVsCold = int.Parse(value) },
            { LabelType.SaveVsMagic, value => SaveVsMagic = int.Parse(value) },
            { LabelType.CurrentHP, value => CurrentHP = int.Parse(value) },
            { LabelType.MaxHP, value => MaxHP = int.Parse(value) },
            { LabelType.HPPerc, value => HPPerc = int.Parse(value) },
            { LabelType.ManaPerc, value => ManaPerc = int.Parse(value) },
            { LabelType.STAPerc, value => STAPerc = int.Parse(value) },
            { LabelType.CurrentMitigation, value => CurrentMitigation = int.Parse(value) },
            { LabelType.CurrentOffense, value => CurrentOffense = int.Parse(value) },
            { LabelType.Weight, value => Weight = int.Parse(value) },
            { LabelType.MaxWeight, value => MaxWeight = int.Parse(value) },
            { LabelType.ExpPerc, value => ExpPerc = int.Parse(value) },
            { LabelType.AltExpPerc, value => AltExpPerc = int.Parse(value) },
            { LabelType.TargetName, value => TargetName = value },
            { LabelType.TargetHPPerc, value => TargetHPPerc = int.Parse(value) },
            { LabelType.GroupMember1Name, value => GroupMember1Name = value },
            { LabelType.GroupMember2Name, value => GroupMember2Name = value },
            { LabelType.GroupMember3Name, value => GroupMember3Name = value },
            { LabelType.GroupMember4Name, value => GroupMember4Name = value },
            { LabelType.GroupMember5Name, value => GroupMember5Name = value },
            { LabelType.GroupMember1HPPerc, value => GroupMember1HPPerc = int.Parse(value) },
            { LabelType.GroupMember2HPPerc, value => GroupMember2HPPerc = int.Parse(value) },
            { LabelType.GroupMember3HPPerc, value => GroupMember3HPPerc = int.Parse(value) },
            { LabelType.GroupMember4HPPerc, value => GroupMember4HPPerc = int.Parse(value) },
            { LabelType.GroupMember5HPPerc, value => GroupMember5HPPerc = int.Parse(value) },
            { LabelType.GroupPet1HPPerc, value => GroupPet1HPPerc = int.Parse(value) },
            { LabelType.GroupPet2HPPerc, value => GroupPet2HPPerc = int.Parse(value) },
            { LabelType.GroupPet3HPPerc, value => GroupPet3HPPerc = int.Parse(value) },
            { LabelType.GroupPet4HPPerc, value => GroupPet4HPPerc = int.Parse(value) },
            { LabelType.GroupPet5HPPerc, value => GroupPet5HPPerc = int.Parse(value) },
            { LabelType.PlayerPetName, value => PlayerPetName = value },
            { LabelType.PlayerPetHPPerc, value => PlayerPetHPPerc = int.Parse(value) },
            { LabelType.PlayerCurrentHPMaxHP, value => PlayerCurrentHPMaxHP = value },
            { LabelType.CurrentAAPoints, value => CurrentAAPoints = int.Parse(value) },
            { LabelType.CurrentAAPerc, value => CurrentAAPerc = value },
            { LabelType.LastName, value => LastName = value },
            { LabelType.Title, value => Title = value },
            { LabelType.ExpPH, value => ExpPH = value },
            { LabelType.TargetPetOwner, value => TargetPetOwner = value },
            { LabelType.CastingName, value => CastingName = value },
            // Add more mappings as needed...
        };

            InitializeProperties();
        }

        private void InitializeProperties()
        {
            foreach (var gauge in _zealCharacter.Detail.GaugeData)
            {
                if (_gaugeMappings.TryGetValue(gauge.Type, out var setter))
                {
                    setter(gauge.Value);
                }
            }

            foreach (var label in _zealCharacter.Detail.LabelData)
            {
                if (_labelMappings.TryGetValue(label.Type, out var setter))
                {
                    setter(label.Value);
                }
            }
        }
    }
}