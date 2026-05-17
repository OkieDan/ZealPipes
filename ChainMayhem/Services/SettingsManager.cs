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
        var directory = settingsDirectory ?? AppDomain.CurrentDomain.BaseDirectory;
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

    public void AddIgnoredPlayer(string playerName)
    {
        _settings.IgnoredPlayers.Add(playerName);
        SaveSettings();
    }

    public void RemoveIgnoredPlayer(string playerName)
    {
        _settings.IgnoredPlayers.Remove(playerName);
        SaveSettings();
    }

    public void ResetToDefaults()
    {
        _settings = new ChainMayhemSettings();
        SaveSettings();
    }
}
