using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealPipes.ClientWinforms.UI.Interfaces;

namespace ZealPipes.ClientWinforms.UI.Factories;

public static class PlayerControlFactory
{
    public static IPlayerControl CreatePlayerControl(string theme)
    {
        switch (theme)
        {
            case "Default":
                return new ZealPipes.ClientWinforms.UI.Default.PlayerControl();
            case "CustomTheme":
                return new ZealPipes.ClientWinforms.UI.CustomTheme.PlayerControl();
            default:
                throw new ArgumentException("Invalid theme specified");
        }
    }
}
