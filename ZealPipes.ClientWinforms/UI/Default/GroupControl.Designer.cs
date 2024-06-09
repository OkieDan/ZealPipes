namespace ZealPipes.ClientWinforms.UI.Default
{
    partial class GroupControl
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
            groupMemberControl1 = new SharedComponents.GroupMemberControl();
            groupMemberControl2 = new SharedComponents.GroupMemberControl();
            groupMemberControl3 = new SharedComponents.GroupMemberControl();
            groupMemberControl4 = new SharedComponents.GroupMemberControl();
            groupMemberControl5 = new SharedComponents.GroupMemberControl();
            SuspendLayout();
            // 
            // groupMemberControl1
            // 
            groupMemberControl1.Dock = DockStyle.Top;
            groupMemberControl1.GaugeCaption = "F2";
            groupMemberControl1.GaugeColor = Color.Empty;
            groupMemberControl1.GaugeValue = 0;
            groupMemberControl1.GroupMemberName = "GroupMemberName";
            groupMemberControl1.Location = new Point(0, 0);
            groupMemberControl1.Name = "groupMemberControl1";
            groupMemberControl1.Size = new Size(172, 31);
            groupMemberControl1.TabIndex = 0;
            // 
            // groupMemberControl2
            // 
            groupMemberControl2.Dock = DockStyle.Top;
            groupMemberControl2.GaugeCaption = "F3";
            groupMemberControl2.GaugeColor = Color.Empty;
            groupMemberControl2.GaugeValue = 0;
            groupMemberControl2.GroupMemberName = "GroupMemberName";
            groupMemberControl2.Location = new Point(0, 31);
            groupMemberControl2.Name = "groupMemberControl2";
            groupMemberControl2.Size = new Size(172, 31);
            groupMemberControl2.TabIndex = 1;
            // 
            // groupMemberControl3
            // 
            groupMemberControl3.Dock = DockStyle.Top;
            groupMemberControl3.GaugeCaption = "F4";
            groupMemberControl3.GaugeColor = Color.Empty;
            groupMemberControl3.GaugeValue = 0;
            groupMemberControl3.GroupMemberName = "GroupMemberName";
            groupMemberControl3.Location = new Point(0, 62);
            groupMemberControl3.Name = "groupMemberControl3";
            groupMemberControl3.Size = new Size(172, 31);
            groupMemberControl3.TabIndex = 2;
            // 
            // groupMemberControl4
            // 
            groupMemberControl4.Dock = DockStyle.Top;
            groupMemberControl4.GaugeCaption = "F5";
            groupMemberControl4.GaugeColor = Color.Empty;
            groupMemberControl4.GaugeValue = 0;
            groupMemberControl4.GroupMemberName = "GroupMemberName";
            groupMemberControl4.Location = new Point(0, 93);
            groupMemberControl4.Name = "groupMemberControl4";
            groupMemberControl4.Size = new Size(172, 31);
            groupMemberControl4.TabIndex = 3;
            // 
            // groupMemberControl5
            // 
            groupMemberControl5.Dock = DockStyle.Top;
            groupMemberControl5.GaugeCaption = "F5";
            groupMemberControl5.GaugeColor = Color.Empty;
            groupMemberControl5.GaugeValue = 0;
            groupMemberControl5.GroupMemberName = "GroupMemberName";
            groupMemberControl5.Location = new Point(0, 124);
            groupMemberControl5.Name = "groupMemberControl5";
            groupMemberControl5.Size = new Size(172, 31);
            groupMemberControl5.TabIndex = 4;
            // 
            // GroupControl
            // 
            AutoScaleDimensions = new SizeF(5F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupMemberControl5);
            Controls.Add(groupMemberControl4);
            Controls.Add(groupMemberControl3);
            Controls.Add(groupMemberControl2);
            Controls.Add(groupMemberControl1);
            Font = new Font("Segoe UI", 7F);
            Margin = new Padding(2);
            Name = "GroupControl";
            Size = new Size(172, 160);
            ResumeLayout(false);
        }

        #endregion

        private SharedComponents.GroupMemberControl groupMemberControl1;
        private SharedComponents.GroupMemberControl groupMemberControl2;
        private SharedComponents.GroupMemberControl groupMemberControl3;
        private SharedComponents.GroupMemberControl groupMemberControl4;
        private SharedComponents.GroupMemberControl groupMemberControl5;
    }
}
