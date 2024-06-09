namespace ZealPipes.ClientWinforms.UI.SharedComponents
{
    partial class GroupMemberControl
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
            PlayerNameLabel = new Label();
            gaugeControl1 = new GaugeControl();
            SuspendLayout();
            // 
            // PlayerNameLabel
            // 
            PlayerNameLabel.Dock = DockStyle.Top;
            PlayerNameLabel.Location = new Point(0, 0);
            PlayerNameLabel.Name = "PlayerNameLabel";
            PlayerNameLabel.Size = new Size(250, 15);
            PlayerNameLabel.TabIndex = 0;
            PlayerNameLabel.Text = "Name";
            // 
            // gaugeControl1
            // 
            gaugeControl1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gaugeControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            gaugeControl1.Caption = "F1";
            gaugeControl1.GaugeColor = Color.Empty;
            gaugeControl1.Location = new Point(0, 15);
            gaugeControl1.Margin = new Padding(0);
            gaugeControl1.Name = "gaugeControl1";
            gaugeControl1.PrimaryValue = 100;
            gaugeControl1.SecondaryValue = 100;
            gaugeControl1.Size = new Size(250, 15);
            gaugeControl1.TabIndex = 1;
            // 
            // GroupMemberControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gaugeControl1);
            Controls.Add(PlayerNameLabel);
            Name = "GroupMemberControl";
            Size = new Size(250, 30);
            ResumeLayout(false);
        }

        #endregion

        private Label PlayerNameLabel;
        private GaugeControl gaugeControl1;
    }
}
