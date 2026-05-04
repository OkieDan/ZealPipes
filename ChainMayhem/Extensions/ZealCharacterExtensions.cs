using ZealPipes.Common.Models;
using ChainMayhem.Models;
using ZealPipes.Common;

namespace ChainMayhem.Extensions;

public static class ZealCharacterExtensions
{
    public static Group ToGroup(this ZealCharacter character)
    {
        var group = new Group
        {
            Members = new List<GroupMember>()
        };

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
                    PetHp = memberPetHp
                });
            }
        }
        return group;
    }
}
