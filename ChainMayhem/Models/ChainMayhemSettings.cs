namespace ChainMayhem.Models;

public class ChainMayhemSettings
{
    public string? SelectedCharacterName { get; set; }
    public int SpeechRate { get; set; } = 8;
    public bool TrackCharmBreaksForCharmClassesOnly { get; set; }
    public HashSet<string> IgnoredPlayers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
