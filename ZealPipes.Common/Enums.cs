﻿namespace ZealPipes.Common
{
    public enum PipeMessageType
    {
        LogText,
        Label,
        Gauge,
        Player,
        PipeCmd, // custom in zeal
        Raid
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
        SpellSlot0 = 60,
        SpellSlot1 = 61,
        SpellSlot2 = 62,
        SpellSlot3 = 63,
        SpellSlot4 = 64,
        SpellSlot5 = 65,
        SpellSlot6 = 66,
        SpellSlot7 = 67,
        PlayerPetName = 68,
        PlayerPetHPPerc = 69,
        PlayerCurrentHPMaxHP = 70,
        CurrentAAPoints = 71,
        CurrentAAPerc = 72,
        LastName = 73,
        Title = 74,
        // Mana/MaxMana (duplicate of Mana/MaxMana at 80)
        // ...
        ManaMaxMana = 80,
        ExpPH = 81,
        TargetPetOwner = 82,
        // Duplicate Mana (124) and MaxMana (125) entries removed
        Mana = 124,
        MaxMana = 125,
        CastingName = 134
    }
    public enum LogType : int
    {
        unk1 = 1,
        GreenCon = 2,
        unk3 = 3,
        DarkBlueCon = 4,
        unk5 = 5,
        unk6 = 6,
        unk7 = 7,
        unk8 = 8,
        unk9 = 9,
        unk10 = 10,
        unk11 = 11,
        unk12 = 12,
        RedCon = 13,
        unk14 = 14,
        YellowCon = 15,
        unk16 = 16,
        unk17 = 17,
        unk18 = 18,
        unk19 = 19,
        unk20 = 20,
        OthersSay = 256,
        Tell = 257,
        Group = 258,
        Guild = 259,
        Ooc = 260,
        Auction = 261,
        Shout = 262,
        Emote = 263,
        Spells = 264,
        YouHitOther = 265,
        OtherHitsYou = 266,
        YouMissOther = 267,
        OtherMissesYou = 268,
        SomeBroadcasts = 269,
        Skills = 270,
        SpecialAbilities = 271,
        Unused17 = 272,
        DefaultText = 273,
        Unused19 = 274,
        MerchatOfferPrice = 275,
        MerchantBuySell = 276,
        YourDeathMessage = 277,
        OtherDeathMessage = 278,
        OtherDamageOther = 279,
        OtherMissOther = 280,
        WhoCommand = 281,
        YellForHelp = 282,
        HitForNonMelee = 283,
        SpellWornOff = 284,
        MoneySplits = 285,
        LootMessage = 286,
        DiceRoll = 287,
        OthersSpells = 288,
        SpellFailures = 289,
        ChatChannel = 290,
        ChatChannel1 = 291,
        ChatChannel2 = 292,
        ChatChannel3 = 293,
        ChatChannel4 = 294,
        ChatChannel5 = 295,
        ChatChannel6 = 296,
        ChatChannel7 = 297,
        ChatChannel8 = 298,
        ChatChannel9 = 299,
        ChatChannel10 = 300,
        MeleeCrits = 301,
        SpellCrits = 302,
        TooFarAwayMelee = 303,
        NpcRampage = 304,
        NpcFlurry = 305,
        NpcEnrage = 306,
        SayEcho = 307,
        TellEcho = 308,
        GroupEcho = 309,
        GuildEcho = 310,
        OocEcho = 311,
        AuctionEcho = 312,
        ShoutEcho = 313,
        EmoteEcho = 314,
        ChatChannel1Echo = 315,
        ChatChannel2Echo = 316,
        ChatChannel3Echo = 317,
        ChatChannel4Echo = 318,
        ChatChannel5Echo = 319,
        ChatChannel6Echo = 320,
        ChatChannel7Echo = 321,
        ChatChannel8Echo = 322,
        ChatChannel9Echo = 323,
        ChatChannel10Echo = 324,
        ZealBlueCon = 325,
        ItemTags = 326,
        RaidSay = 327
    }
    public enum RaidRank
    {
        RaidMember = 0,
        GroupLeader = 1,
        RaidLeader = 2
    }
    public enum ClassTypes
    {
        Warrior = 1,
        Cleric = 2,
        Paladin = 3,
        Ranger = 4,
        Shadowknight = 5,
        Druid = 6,
        Monk = 7,
        Bard = 8,
        Rogue = 9,
        Shaman = 10,
        Necromancer = 11,
        Wizard = 12,
        Magician = 13,
        Enchanter = 14,
        Beastlord = 15,
        Banker = 16,
        Merchant = 32
    };
}