using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZealPipes.ClientWinforms.Models;
using ZealPipes.ClientWinforms.UI.Interfaces;

namespace ZealPipes.ClientWinforms.UI.CustomTheme
{
    public partial class PlayerControl : UserControl, IPlayerControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }

        public void UpdatePlayerInfo(Character character)
        {
            lblPlayerName.Text = character.Name;
            lblPlayerLevel.Text = $"Level: {character.Level}";
            // Update other controls with player info
        }

        public Control GetControl()
        {
            return this;
        }
    }
}
