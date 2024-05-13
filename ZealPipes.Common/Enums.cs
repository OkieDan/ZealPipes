namespace ZealPipes.Common
{
    public enum PipeMessageType
    {
        LogText,
        Label,
        Gauge
    }
    public enum GaugeType
    {
        HP = 1,
        Mana = 2,
        Stamina = 3,
        Experience = 4,
        AltExp = 5,
        Target = 6,
        Casting = 7,
        Breath = 8,
        Memorize = 9,
        Scribe = 10,
        Group1HP = 11,
        Group2HP = 12,
        Group3HP = 13,
        Group4HP = 14,
        Group5HP = 15,
        PetHP = 16,
        Group1PetHP = 17,
        Group2PetHP = 18,
        Group3PetHP = 19,
        Group4PetHP = 20,
        Group5PetHP = 21,
        // Removed duplicate entry for ExpPerHR
        ExpPerHR = 23
    }
    public enum LabelType
    {
        Name = 1,
        Level = 2,
        Class = 3,
        Deity = 4,
        Strength = 5,
        Stamina = 6,
        Dexterity = 7,
        Agility = 8,
        Wisdom = 9,
        Intelligence = 10,
        Charisma = 11,
        SaveVsPoison = 12,
        SaveVsDisease = 13,
        SaveVsFire = 14,
        SaveVsCold = 15,
        SaveVsMagic = 16,
        CurrentHP = 17,
        MaxHP = 18,
        HPPerc = 19,
        ManaPerc = 20,
        STAPerc = 21,
        CurrentMitigation = 22,
        CurrentOffense = 23,
        Weight = 24,
        MaxWeight = 25,
        ExpPerc = 26,
        AltExpPerc = 27,
        TargetName = 28,
        TargetHPPerc = 29,
        // Group Member Names
        GroupMember1Name = 30,
        GroupMember2Name = 31,
        GroupMember3Name = 32,
        GroupMember4Name = 33,
        GroupMember5Name = 34,
        // Group Member HP Percentages
        GroupMember1HPPerc = 35,
        GroupMember2HPPerc = 36,
        GroupMember3HPPerc = 37,
        GroupMember4HPPerc = 38,
        GroupMember5HPPerc = 39,
        // Group Pet HP Percentages
        GroupPet1HPPerc = 40,
        GroupPet2HPPerc = 41,
        GroupPet3HPPerc = 42,
        GroupPet4HPPerc = 43,
        GroupPet5HPPerc = 44,
        // Buff Labels
        Buff0 = 45,
        Buff1 = 46,
        Buff2 = 47,
        Buff3 = 48,
        Buff4 = 49,
        Buff5 = 50,
        Buff6 = 51,
        Buff7 = 52,
        Buff8 = 53,
        Buff9 = 54,
        Buff10 = 55,
        Buff11 = 56,
        Buff12 = 57,
        Buff13 = 58,
        Buff14 = 59,
        // Spell XML Names (skipped - not meaningful as labels)
        // ...
        PlayerPetName = 68,
        PlayerPetHPPerc = 69,
        PlayerCurrentHPMaxHP = 70,
        CurrentAAPoints = 71,
        CurrentAAPerc = 72,
        LastName = 73,
        Title = 74,
        // Mana/MaxMana (duplicate of Mana/MaxMana at 80)
        // ...
        ExpPH = 81,
        TargetPetOwner = 82,
        // Duplicate Mana (124) and MaxMana (125) entries removed
        CastingName = 134
    }

}