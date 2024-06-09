using ZealPipes.ClientWinforms.Models;

namespace ZealPipes.ClientWinforms.Interfaces;

public interface IMainView
{
    void UpdateCharacterData(Character character);
    void DrawGauge(string gaugeData);
    //void UpdatePlayerData(Player player);
    void UpdateLabelData(string labelData);
    void SetBackgroundImage(Image image);
    void ShowMessage(string message);
    void UpdateCharacterDropdown(List<string> characterNames);
}

