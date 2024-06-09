using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZealPipes.ClientWinforms.UI.SharedComponents
{
    public partial class GaugeControl : UserControl
    {
        private Color _gaugeColor;
        private string _caption = "F1";
        private int _primaryValue;
        private int _secondaryValue;

        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                label1.Text = value;
            }
        }

        public int PrimaryValue
        {
            get => _primaryValue;
            set
            {
                _primaryValue = value;
                zealProgressBar1.Value = value;
                label2.Text = $"{value}%";

            }
        }

        public int SecondaryValue
        {
            get => _secondaryValue;
            set
            {
                _secondaryValue = value;
                zealProgressBar1.SecondaryValue = value;
                label2.Text = $"{value}%";

            }
        }

        public Color GaugeColor
        {
            get => _gaugeColor;
            set
            {
                if (value != this._gaugeColor)
                {
                    this._gaugeColor = value;
                    Invalidate(); // Trigger repaint
                }
            }
        }

        public GaugeControl()
        {
            InitializeComponent();

        }
        private void GaugeControl_Load(object sender, EventArgs e)
        {
            // Optional: Set a default color for GaugeColor when the control loads
            //GaugeColor = Color.Gray; // Or any default color you prefer
        }

        // Override the OnPaint method to draw the gauge and update progress bar color
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (GaugeColor != null)
            {
                // Draw the gauge using the GaugeColor
                // You can use various drawing methods based on your desired gauge design
                e.Graphics.FillRectangle(new SolidBrush(GaugeColor), ClientRectangle);

                // Set the ForeColor of progressBar1 to the GaugeColor
                if (zealProgressBar1 != null)
                {
                    zealProgressBar1.ForeColor = GaugeColor;
                    //progressBar1.Value = Value;
                    //label2.Text = $"{Value}%";
                }
            }
        }


    }
}
