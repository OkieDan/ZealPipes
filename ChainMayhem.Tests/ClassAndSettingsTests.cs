using ChainMayhem.Extensions;
using ChainMayhem.Services;
using Xunit;
using ZealPipes.Common;
using ZealPipes.Common.Models;

namespace ChainMayhem.Tests;

public class ClassAndSettingsTests
{
    [Theory]
    [InlineData("Enchanter", 14)]
    [InlineData("druid", 6)]
    [InlineData("Necromancer", 11)]
    [InlineData("ENC", 14)]
    [InlineData("DRU", 6)]
    [InlineData("NEC", 11)]
    [InlineData("Magician", 13)]
    [InlineData(null, null)]
    public void OwnPetUsesPlayerClassLabel(string? classLabel, int? expectedClass)
    {
        var character = CharacterWithPetLabels();
        if (classLabel != null)
            character.Detail.LabelData.Add(new LabelMessage.LabelData { Type = LabelType.Class, Value = classLabel });

        var member = Assert.Single(character.ToGroup().Members);
        Assert.Equal("Self", member.Name);
        Assert.Equal(expectedClass, member.Class);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GroupClassesMatchByNameAndRemainUnknownWithoutVerboseFields(bool verbose)
    {
        var character = CharacterWithPetLabels();
        character.Detail.LabelData.Add(new LabelMessage.LabelData { Type = LabelType.GroupMember1Name, Value = "Member" });
        character.Detail.LabelData.Add(new LabelMessage.LabelData { Type = LabelType.GroupPet1HPPerc, Value = "55" });
        character.Detail.LabelData.Add(new LabelMessage.LabelData { Type = LabelType.GroupMember2Name, Value = "Other" });
        character.Detail.LabelData.Add(new LabelMessage.LabelData { Type = LabelType.GroupPet2HPPerc, Value = "44" });
        var payload = verbose
            ? "[{\"name\":\"Other\",\"class\":13},{\"name\":\"member\",\"class\":14}]"
            : "[{\"name\":\"Other\"},{\"name\":\"member\"}]";

        var group = character.ToGroup(new GroupMessage("Self", payload).Data);
        Assert.Equal(verbose ? (int?)14 : null, group.Members.Single(m => m.Name == "Member").Class);
        Assert.Equal(verbose ? (int?)13 : null, group.Members.Single(m => m.Name == "Other").Class);
        Assert.Null(Assert.Single(character.ToGroup().Members, m => m.Name == "Member").Class);
    }

    [Fact]
    public void ClassFilterDefaultsOffAndPersistsAndResets()
    {
        var directory = Path.Combine(Path.GetTempPath(), "ChainMayhem-test-" + Guid.NewGuid());
        Directory.CreateDirectory(directory);
        try
        {
            var path = Path.Combine(directory, "user-settings.json");
            File.WriteAllText(path, "{\"speechRate\":5,\"selectedCharacterName\":\"Self\",\"ignoredPlayers\":[\"friend\"]}");
            var manager = new SettingsManager(directory);
            Assert.False(manager.Settings.TrackCharmBreaksForCharmClassesOnly);
            manager.UpdateCharmBreakClassFilter(true);

            var reloaded = new SettingsManager(directory);
            Assert.True(reloaded.Settings.TrackCharmBreaksForCharmClassesOnly);
            Assert.Equal(5, reloaded.Settings.SpeechRate);
            Assert.Equal("Self", reloaded.Settings.SelectedCharacterName);
            Assert.Contains("Friend", reloaded.Settings.IgnoredPlayers);
            reloaded.ResetToDefaults();
            Assert.False(new SettingsManager(directory).Settings.TrackCharmBreaksForCharmClassesOnly);
        }
        finally
        {
            File.Delete(Path.Combine(directory, "user-settings.json"));
            Directory.Delete(directory);
        }
    }

    private static ZealCharacter CharacterWithPetLabels()
    {
        var character = new ZealCharacter("Self", 123);
        character.Detail.LabelData.Add(new LabelMessage.LabelData { Type = LabelType.PlayerPetHPPerc, Value = "55" });
        return character;
    }
}
