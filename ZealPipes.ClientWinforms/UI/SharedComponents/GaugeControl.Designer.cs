namespace ZealPipes.ClientWinforms.UI.SharedComponents
{
    partial class GaugeControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            zealProgressBar1 = new ZealProgressBar();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(19, 15);
            label1.TabIndex = 0;
            label1.Text = "F1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Right;
            label2.Location = new Point(179, 0);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 2;
            label2.Text = "100%";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // zealProgressBar1
            // 
            zealProgressBar1.Location = new Point(19, 2);
            zealProgressBar1.Margin = new Padding(0);
            zealProgressBar1.MaximumSecondaryValue = 100;
            zealProgressBar1.MaximumValue = 100;
            zealProgressBar1.Name = "zealProgressBar1";
            zealProgressBar1.SecondaryValue = 100;
            zealProgressBar1.Size = new Size(160, 13);
            zealProgressBar1.TabIndex = 3;
            zealProgressBar1.Value = 75;
            // 
            // GaugeControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(zealProgressBar1);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(0);
            Name = "GaugeControl";
            Size = new Size(214, 15);
            Load += GaugeControl_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private ZealProgressBar zealProgressBar1;
    }
}
