using System.Text.Json;
using ChainMayhem.Models;

namespace ChainMayhem.Services;

public class SettingsManager
{
    private readonly string _settingsFilePath;
    private ChainMayhemSettings _settings;
    private readonly object _saveLock = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public SettingsManager(string? settingsDirectory = null)
    {
        // Use LocalApplicationData for user-specific writable location
        var directory = settingsDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ChainMayhem");

        // Ensure directory exists
        try
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not create settings directory: {ex.Message}");
            Console.WriteLine("Settings will not be persisted.");
        }

        _settingsFilePath = Path.Combine(directory, "user-settings.json");
        _settings = LoadSettings();
    }

    public ChainMayhemSettings Settings => _settings;

    private ChainMayhemSettings LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                var json = File.ReadAllText(_settingsFilePath);
                var settings = JsonSerializer.Deserialize<ChainMayhemSettings>(json, JsonOptions);
                if (settings != null)
                {
                    var normalizedPlayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var player in settings.IgnoredPlayers)
                    {
                        // Normalize to Title Case using invariant culture
                        var normalized = NormalizePlayerName(player);
                        normalizedPlayers.Add(normalized);
                    }
                    settings.IgnoredPlayers = normalizedPlayers;

                    return settings;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not load user settings: {ex.Message}");
            Console.WriteLine("Using default settings.");
        }

        return new ChainMayhemSettings();
    }

    public void SaveSettings()
    {
        lock (_saveLock)
        {
            try
            {
                var json = JsonSerializer.Serialize(_settings, JsonOptions);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not save settings: {ex.Message}");
            }
        }
    }

    public void UpdateSelectedCharacter(string? characterName)
    {
        _settings.SelectedCharacterName = characterName;
        SaveSettings();
    }

    public void UpdateSpeechRate(int rate)
    {
        _settings.SpeechRate = rate;
        SaveSettings();
    }

    /// <summary>
    /// Adds a player to the ignored list. Returns true if added, false if already exists.
    /// </summary>
    public bool AddIgnoredPlayer(string playerName)
    {
        var normalized = NormalizePlayerName(playerName);
        var added = _settings.IgnoredPlayers.Add(normalized);
        if (added)
        {
            SaveSettings();
        }
        return added;
    }

    public void RemoveIgnoredPlayer(string playerName)
    {
        var normalized = NormalizePlayerName(playerName);
        _settings.IgnoredPlayers.Remove(normalized);
        SaveSettings();
    }

    public void ResetToDefaults()
    {
        _settings = new ChainMayhemSettings();
        SaveSettings();
    }

    /// <summary>
    /// Normalizes player names to Title Case using invariant culture.
    /// Example: "BOBSMITH" or "bobsmith" -> "Bobsmith"
    /// </summary>
    private static string NormalizePlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        return char.ToUpperInvariant(name[0]) + name.Substring(1).ToLowerInvariant();
    }
}
