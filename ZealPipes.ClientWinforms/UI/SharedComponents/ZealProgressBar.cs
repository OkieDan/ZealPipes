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
    public partial class ZealProgressBar : UserControl
    {
        private int _minValue = 0;
        //public int MinimumValue
        //{
        //    get => _minValue;
        //    set
        //    {
        //        _minValue = value;
        //    }
        //}

        private int _maxValue = 100;
        public int MaximumValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
            }
        }

        private int _value = 75;
        public int Value
        {
            get => _value;
            set
            {
                value = Math.Min(100, Math.Max(0, value));
                _value = value;
                RecalcWidth();
            }
        }

        //private int _minSecondaryValue = 0;
        //public int MinimumSecondaryValue
        //{
        //    get => _minValue;
        //    set
        //    {
        //        _minValue = value;
        //    }
        //}

        private int _maxSecondaryValue = 100;
        public int MaximumSecondaryValue
        {
            get => _maxSecondaryValue;
            set
            {
                _maxSecondaryValue = value;
            }
        }

        private int _secondaryValue = 100;
        public int SecondaryValue
        {
            get => _secondaryValue;
            set
            {
                value = Math.Min(100, Math.Max(0, value));
                _secondaryValue = value;
                RecalcSecondaryWidth();
            }
        }
        private void RecalcWidth()
        {            
            var width = 0;
            if (_value > 0)
            {
                width = this.Width * _value / _maxValue;
            }
            MainProgress.Width = Math.Min(this.Width, width);
            MainProgress.Visible = width > 0;
        }

        private void RecalcSecondaryWidth()
        {
            var width = 0;
            if (_secondaryValue > 0)
            {
                width = this.Width * _secondaryValue / _maxSecondaryValue;
            }
            SecondaryProgress.Width = Math.Min(this.Width, width);
            SecondaryProgress.Visible = width > 0;
        }



        public ZealProgressBar()
        {
            InitializeComponent();
        }
    }
}
