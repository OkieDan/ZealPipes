using ZealPipes.Common.Models;
using ChainMayhem.Models;
using ZealPipes.Common;

namespace ChainMayhem.Extensions;

public static class ZealCharacterExtensions
{
    public static Group ToGroup(this ZealCharacter character, IEnumerable<GroupMessage.GroupData>? groupMembers = null)
    {
        var members = groupMembers?.ToArray() ?? Array.Empty<GroupMessage.GroupData>();
        var group = new Group
        {
            Members = new List<GroupMember>()
        };

        // Add the player's own pet
        var playerPetHpString = character.Detail.LabelData.FirstOrDefault(ld => ld.Type == LabelType.PlayerPetHPPerc)?.Value;
        if (!string.IsNullOrEmpty(playerPetHpString))
        {
            var playerPetHp = int.TryParse(playerPetHpString, out var parsedPlayerPetHp) ? parsedPlayerPetHp : 0;
            group.Members.Add(new GroupMember
            {
                Name = character.Name,
                PetHp = playerPetHp,
                Class = GetPlayerClass(character)
            });
        }

        // Add group members' pets
        for (int i = 0; i < 5; i++)
        {
            var nameLabelType = (LabelType)((int)LabelType.GroupMember1Name + i);
            var name = character.Detail.LabelData.FirstOrDefault(ld => ld.Type == nameLabelType)?.Value;
            var ptHpString = character.Detail.LabelData.FirstOrDefault(ld => ld.Type == (LabelType)((int)LabelType.GroupPet1HPPerc + i))?.Value;

            if (!string.IsNullOrEmpty(name))
            {
                var memberPetHp = int.TryParse(ptHpString, out var parsedPetHp) ? parsedPetHp : 0;
                group.Members.Add(new GroupMember
                {
                    Name = name,
                    PetHp = memberPetHp,
                    Class = members.FirstOrDefault(member =>
                        string.Equals(member.Name, name, StringComparison.OrdinalIgnoreCase))?.Class
                });
            }
        }
        return group;
    }

    private static int? GetPlayerClass(ZealCharacter character)
    {
        var value = character.Detail.LabelData.FirstOrDefault(ld => ld.Type == LabelType.Class)?.Value?.Trim();
        if (Enum.TryParse<ClassTypes>(value, true, out var playerClass))
            return (int)playerClass;

        return value?.ToUpperInvariant() switch
        {
            "ENC" => (int)ClassTypes.Enchanter,
            "DRU" => (int)ClassTypes.Druid,
            "NEC" => (int)ClassTypes.Necromancer,
            _ => null
        };
    }
}
