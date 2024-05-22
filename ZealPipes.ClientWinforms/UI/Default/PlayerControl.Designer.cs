namespace ZealPipes.ClientWinforms.UI.Default
{
    partial class PlayerControl
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
            lblPlayerName = new Label();
            lblPlayerLevel = new Label();
            lblPlayerClass = new Label();
            lblPlayerDiety = new Label();
            XpGauge = new SharedComponents.GaugeControl();
            HpGauge = new SharedComponents.GaugeControl();
            ManaGauge = new SharedComponents.GaugeControl();
            EnduranceGauge = new SharedComponents.GaugeControl();
            SuspendLayout();
            // 
            // lblPlayerName
            // 
            lblPlayerName.Dock = DockStyle.Top;
            lblPlayerName.Font = new Font("Segoe UI", 9F);
            lblPlayerName.Location = new Point(0, 0);
            lblPlayerName.Margin = new Padding(2, 0, 2, 0);
            lblPlayerName.Name = "lblPlayerName";
            lblPlayerName.Size = new Size(159, 12);
            lblPlayerName.TabIndex = 0;
            lblPlayerName.Text = "Player Name";
            lblPlayerName.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblPlayerLevel
            // 
            lblPlayerLevel.AutoSize = true;
            lblPlayerLevel.Location = new Point(3, 126);
            lblPlayerLevel.Margin = new Padding(2, 0, 2, 0);
            lblPlayerLevel.Name = "lblPlayerLevel";
            lblPlayerLevel.Size = new Size(14, 12);
            lblPlayerLevel.TabIndex = 1;
            lblPlayerLevel.Text = "lvl";
            // 
            // lblPlayerClass
            // 
            lblPlayerClass.AutoSize = true;
            lblPlayerClass.Location = new Point(22, 126);
            lblPlayerClass.Name = "lblPlayerClass";
            lblPlayerClass.Size = new Size(25, 12);
            lblPlayerClass.TabIndex = 3;
            lblPlayerClass.Text = "class";
            // 
            // lblPlayerDiety
            // 
            lblPlayerDiety.AutoSize = true;
            lblPlayerDiety.Location = new Point(3, 138);
            lblPlayerDiety.Name = "lblPlayerDiety";
            lblPlayerDiety.Size = new Size(26, 12);
            lblPlayerDiety.TabIndex = 4;
            lblPlayerDiety.Text = "diety";
            // 
            // XpGauge
            // 
            XpGauge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            XpGauge.Caption = "XP";
            XpGauge.GaugeColor = Color.Transparent;
            XpGauge.Location = new Point(2, 53);
            XpGauge.Margin = new Padding(0, 0, 2, 0);
            XpGauge.Name = "XpGauge";
            XpGauge.Size = new Size(157, 15);
            XpGauge.TabIndex = 5;
            XpGauge.Value = 0;
            // 
            // HpGauge
            // 
            HpGauge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            HpGauge.Caption = "F1";
            HpGauge.GaugeColor = Color.Transparent;
            HpGauge.Location = new Point(2, 14);
            HpGauge.Margin = new Padding(0, 0, 2, 0);
            HpGauge.Name = "HpGauge";
            HpGauge.Size = new Size(157, 16);
            HpGauge.TabIndex = 5;
            HpGauge.Value = 0;
            // 
            // ManaGauge
            // 
            ManaGauge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ManaGauge.Caption = "M";
            ManaGauge.GaugeColor = Color.Transparent;
            ManaGauge.Location = new Point(2, 27);
            ManaGauge.Margin = new Padding(0, 0, 2, 0);
            ManaGauge.Name = "ManaGauge";
            ManaGauge.Size = new Size(157, 16);
            ManaGauge.TabIndex = 5;
            ManaGauge.Value = 0;
            // 
            // EnduranceGauge
            // 
            EnduranceGauge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            EnduranceGauge.Caption = "EN";
            EnduranceGauge.GaugeColor = Color.Transparent;
            EnduranceGauge.Location = new Point(2, 40);
            EnduranceGauge.Margin = new Padding(0, 0, 2, 0);
            EnduranceGauge.Name = "EnduranceGauge";
            EnduranceGauge.Size = new Size(157, 16);
            EnduranceGauge.TabIndex = 5;
            EnduranceGauge.Value = 0;
            // 
            // PlayerControl
            // 
            AutoScaleDimensions = new SizeF(5F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(EnduranceGauge);
            Controls.Add(ManaGauge);
            Controls.Add(HpGauge);
            Controls.Add(XpGauge);
            Controls.Add(lblPlayerDiety);
            Controls.Add(lblPlayerClass);
            Controls.Add(lblPlayerLevel);
            Controls.Add(lblPlayerName);
            Font = new Font("Segoe UI", 7F);
            Margin = new Padding(2);
            Name = "PlayerControl";
            Size = new Size(159, 152);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPlayerName;
        private Label lblPlayerLevel;
        private Label lblPlayerClass;
        private Label lblPlayerDiety;
        private SharedComponents.GaugeControl XpGauge;
        private SharedComponents.GaugeControl HpGauge;
        private SharedComponents.GaugeControl ManaGauge;
        private SharedComponents.GaugeControl EnduranceGauge;
    }
}
