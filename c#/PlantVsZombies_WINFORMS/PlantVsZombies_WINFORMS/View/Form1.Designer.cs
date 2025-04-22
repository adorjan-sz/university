namespace PlantVsZombies_WINFORMS
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            boardPanel = new FlowLayoutPanel();
            statusStrip = new StatusStrip();
            sunsText = new ToolStripStatusLabel();
            timeText = new ToolStripStatusLabel();
            pauseButton = new Button();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // boardPanel
            // 
            boardPanel.Location = new Point(50, 50);
            boardPanel.MaximumSize = new Size(400, 200);
            boardPanel.MinimumSize = new Size(400, 200);
            boardPanel.Name = "boardPanel";
            boardPanel.Size = new Size(400, 200);
            boardPanel.TabIndex = 0;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { sunsText, timeText });
            statusStrip.Location = new Point(0, 428);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(800, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // sunsText
            // 
            sunsText.Name = "sunsText";
            sunsText.Size = new Size(0, 16);
            // 
            // timeText
            // 
            timeText.Name = "timeText";
            timeText.Size = new Size(0, 16);
            // 
            // pauseButton
            // 
            pauseButton.Dock = DockStyle.Top;
            pauseButton.Location = new Point(0, 0);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(800, 32);
            pauseButton.TabIndex = 2;
            pauseButton.Text = "Pause/Resume";
            pauseButton.UseVisualStyleBackColor = true;
            pauseButton.Click += pauseButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pauseButton);
            Controls.Add(statusStrip);
            Controls.Add(boardPanel);
            Name = "Form1";
            Text = "Form1";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel boardPanel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel sunsText;
        private ToolStripStatusLabel timeText;
        private Button pauseButton;
    }
}