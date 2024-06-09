using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZealPipes.ClientWinforms.Extensions;
using ZealPipes.ClientWinforms.Models;
using ZealPipes.ClientWinforms.UI.Interfaces;

namespace ZealPipes.ClientWinforms.UI.Default
{
    public partial class PlayerControl : UserControl, IPlayerControl
    {
        public PlayerControl()
        {
            InitializeComponent();
            this.EnableDragging();
        }

        public void UpdatePlayerInfo(Character character)
        {
            //if (this.Handle != IntPtr.Zero)
            //{
            //    try
            //    {
            //        this.Invoke(() => {
            //            lblPlayerName.Text = player.Name;
            //            lblPlayerLevel.Text = $"Level: {player.Level}";            
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex);
            //    }
            //}
            lblPlayerName.Text = character.Name;
            lblPlayerLevel.Text = $"Level: {character.Level}";
            HpGauge.PrimaryValue = character.HPPerc;
            ManaGauge.PrimaryValue = character.ManaPerc;
            EnduranceGauge.PrimaryValue = character.STAPerc;
            XpGauge.PrimaryValue = character.ExpPerc;
            lblPlayerLevel.Text = character.Level.ToString();
            lblPlayerClass.Text = character.Class;
            lblPlayerDiety.Text = character.Deity;

        }

        public Control GetControl()
        {
            return this;
        }
    }
}
