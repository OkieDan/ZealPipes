namespace ChainMayhem.Models;

public class Group
{
    public List<GroupMember> Members { get; set; } = new();
}

public class GroupMember
{
    public string Name { get; set; } = string.Empty;
    public int PetHp { get; set; }
}
