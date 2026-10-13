using ChainMayhem.Extensions;
using ChainMayhem.Models;
using ZealPipes.Common;

namespace ChainMayhem.Services;

public static class PetAlertService
{
    public static IEnumerable<string> GetAlerts(Group? previousGroup, Group currentGroup,
        bool charmClassesOnly, ISet<string> ignoredPlayers)
    {
        if (previousGroup == null) yield break;

        foreach (var currentMember in currentGroup.Members)
        {
            var previousMember = previousGroup.Members.FirstOrDefault(member =>
                string.Equals(member.Name, currentMember.Name, StringComparison.OrdinalIgnoreCase));
            if (previousMember == null || ignoredPlayers.Contains(currentMember.Name)) continue;

            if (currentMember.PetHp == 0 && previousMember.PetHp > 10)
            {
                if (!charmClassesOnly || currentMember.Class is
                    (int)ClassTypes.Enchanter or (int)ClassTypes.Druid or (int)ClassTypes.Necromancer)
                    yield return $"Charm Break {currentMember.Name}";
            }
            else if (currentMember.PetHp.Between(1, 19) && previousMember.PetHp > 19)
            {
                yield return "Heal Pet";
            }
        }
    }
}
