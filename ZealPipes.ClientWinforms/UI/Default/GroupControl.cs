using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZealPipes.ClientWinforms.Extensions;
using ZealPipes.ClientWinforms.Models;

namespace ZealPipes.ClientWinforms.UI.Default
{
    public partial class GroupControl : UserControl
    {
        public GroupControl()
        {
            InitializeComponent();
            this.EnableDragging();
        }

        internal void UpdateGroupInfo(Character character)
        {
            groupMemberControl1.GroupMemberName = character.GroupMember1Name;
            groupMemberControl1.GaugeValue = character.GroupMember1HPPerc;

            groupMemberControl2.GroupMemberName = character.GroupMember2Name;
            groupMemberControl2.GaugeValue = character.GroupMember2HPPerc;

            groupMemberControl3.GroupMemberName = character.GroupMember3Name;
            groupMemberControl3.GaugeValue = character.GroupMember3HPPerc;

            groupMemberControl4.GroupMemberName = character.GroupMember4Name;
            groupMemberControl4.GaugeValue = character.GroupMember4HPPerc;

            groupMemberControl5.GroupMemberName = character.GroupMember5Name;
            groupMemberControl5.GaugeValue = character.GroupMember5HPPerc;
        }
    }
}
