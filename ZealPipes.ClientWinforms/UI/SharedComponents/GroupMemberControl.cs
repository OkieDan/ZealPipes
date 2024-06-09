using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZealPipes.ClientWinforms.UI.SharedComponents
{
    public partial class GroupMemberControl : UserControl
    {
        private string _groupMemberName = "GroupMemberName";
        private string _gaugeCaption = "F1";
        private int _gaugeValue;
        private Color _gaugeColor;

        public string GroupMemberName
        {
            get => _groupMemberName;
            set
            {
                _groupMemberName = value;
                PlayerNameLabel.Text = value;
            }
        }

        public string GaugeCaption
        {
            get => _gaugeCaption;
            set
            {
                _gaugeCaption = value;
                gaugeControl1.Caption = value;
            }
        }

        public int GaugeValue
        {
            get => _gaugeValue;
            set
            {
                _gaugeValue = value;
                gaugeControl1.PrimaryValue = value;
            }
        }

        public Color GaugeColor
        {
            get => _gaugeColor;
            set
            {
                if (value != this._gaugeColor)
                {
                    _gaugeColor = value;
                    gaugeControl1.GaugeColor = value;
                    Invalidate(); // Trigger repaint
                }
            }
        }

        public GroupMemberControl()
        {
            InitializeComponent();
        }
    }
}
