using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZealPipes.ClientWinforms.Models;

namespace ZealPipes.ClientWinforms.UI.Interfaces
{
    public interface IPlayerControl
    {
        void UpdatePlayerInfo(Character character);
        Control GetControl();
    }
}
