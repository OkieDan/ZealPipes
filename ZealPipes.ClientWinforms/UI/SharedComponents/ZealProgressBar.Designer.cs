namespace ZealPipes.ClientWinforms.UI.SharedComponents
{
    partial class ZealProgressBar
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
            panel1 = new Panel();
            MainProgress = new Panel();
            SecondaryProgress = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(MainProgress);
            panel1.Controls.Add(SecondaryProgress);
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(167, 14);
            panel1.TabIndex = 4;
            // 
            // MainProgress
            // 
            MainProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MainProgress.BackColor = Color.Lime;
            MainProgress.Location = new Point(0, 0);
            MainProgress.Name = "MainProgress";
            MainProgress.Size = new Size(167, 9);
            MainProgress.TabIndex = 4;
            // 
            // SecondaryProgress
            // 
            SecondaryProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SecondaryProgress.BackColor = Color.DeepPink;
            SecondaryProgress.Location = new Point(0, 10);
            SecondaryProgress.Margin = new Padding(0);
            SecondaryProgress.Name = "SecondaryProgress";
            SecondaryProgress.Size = new Size(167, 2);
            SecondaryProgress.TabIndex = 5;
            // 
            // ZealProgressBar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Margin = new Padding(0);
            Name = "ZealProgressBar";
            Size = new Size(167, 14);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel SecondaryProgress;
        private Panel MainProgress;
    }
}
