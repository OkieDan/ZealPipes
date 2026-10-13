using ChainMayhem.Models;
using ChainMayhem.Services;
using Xunit;

namespace ChainMayhem.Tests;

public class PetAlertTests
{
    private static readonly ISet<string> NoIgnoredPlayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    [Theory]
    [InlineData(6)]
    [InlineData(11)]
    [InlineData(14)]
    public void FilterAllowsCharmClasses(int playerClass)
    {
        Assert.Equal("Charm Break Player", Assert.Single(PetAlertService.GetAlerts(
            GroupWithPet(55, playerClass), GroupWithPet(0, playerClass), true, NoIgnoredPlayers)));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(15)]
    [InlineData(0)]
    [InlineData(999)]
    [InlineData(null)]
    public void FilterSuppressesOtherAndUnknownClasses(int? playerClass)
    {
        Assert.Empty(PetAlertService.GetAlerts(GroupWithPet(55, playerClass),
            GroupWithPet(0, playerClass), true, NoIgnoredPlayers));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(13)]
    [InlineData(null)]
    public void DisabledFilterPreservesCharmBreakAlerts(int? playerClass)
    {
        Assert.Equal("Charm Break Player", Assert.Single(PetAlertService.GetAlerts(
            GroupWithPet(55, playerClass), GroupWithPet(0, playerClass), false, NoIgnoredPlayers)));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(null)]
    public void FilterDoesNotSuppressLowHealthAlerts(int? playerClass)
    {
        Assert.Equal("Heal Pet", Assert.Single(PetAlertService.GetAlerts(
            GroupWithPet(55, playerClass), GroupWithPet(19, playerClass), true, NoIgnoredPlayers)));
    }

    [Fact]
    public void IgnoredPlayersStillSuppressBothAlerts()
    {
        var ignored = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "player" };
        Assert.Empty(PetAlertService.GetAlerts(GroupWithPet(55, 14), GroupWithPet(0, 14), true, ignored));
        Assert.Empty(PetAlertService.GetAlerts(GroupWithPet(55, 14), GroupWithPet(19, 14), true, ignored));
    }

    [Fact]
    public void MissingBaselineDoesNotProduceAlerts()
    {
        Assert.Empty(PetAlertService.GetAlerts(null, GroupWithPet(0, 14), true, NoIgnoredPlayers));
        Assert.Empty(PetAlertService.GetAlerts(new Group(), GroupWithPet(0, 14), true, NoIgnoredPlayers));
    }

    [Fact]
    public void DisappearingClassDoesNotReusePreviouslyAllowedClass()
    {
        Assert.Empty(PetAlertService.GetAlerts(GroupWithPet(55, 14), GroupWithPet(0, null), true, NoIgnoredPlayers));
    }

    private static Group GroupWithPet(int hp, int? playerClass) => new()
    {
        Members = new() { new() { Name = "Player", PetHp = hp, Class = playerClass } }
    };
}
