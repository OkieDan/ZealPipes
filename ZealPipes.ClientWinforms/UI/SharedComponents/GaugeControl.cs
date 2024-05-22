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
        private int _value;
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                label1.Text = value;
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                progressBar1.Value = value;
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
                if (progressBar1 != null)
                {
                    progressBar1.ForeColor = GaugeColor;
                    //progressBar1.Value = Value;
                    //label2.Text = $"{Value}%";
                }
            }
        }


    }
}
