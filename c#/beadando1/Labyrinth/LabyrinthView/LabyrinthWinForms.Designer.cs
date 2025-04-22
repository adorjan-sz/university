namespace LabyrinthView
{
    partial class LabyrinthWinForms
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            _newGameToolStripMenuItem = new ToolStripMenuItem();
            _QuitToolStripMenuItem1 = new ToolStripMenuItem();
            _PauseToolStripMenuItem = new ToolStripMenuItem();
            beállitásokToolStripMenuItem = new ToolStripMenuItem();
            _EasyToolStripMenuItem = new ToolStripMenuItem();
            _MediumToolStripMenuItem = new ToolStripMenuItem();
            HardToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            _timeLabel = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, beállitásokToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(264, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _newGameToolStripMenuItem, _QuitToolStripMenuItem1, _PauseToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // _newGameToolStripMenuItem
            // 
            _newGameToolStripMenuItem.Name = "_newGameToolStripMenuItem";
            _newGameToolStripMenuItem.Size = new Size(113, 22);
            _newGameToolStripMenuItem.Text = "Új játék";
            _newGameToolStripMenuItem.Click += _newGameToolStripMenuItem_Click;
            // 
            // _QuitToolStripMenuItem1
            // 
            _QuitToolStripMenuItem1.Name = "_QuitToolStripMenuItem1";
            _QuitToolStripMenuItem1.Size = new Size(113, 22);
            _QuitToolStripMenuItem1.Text = "Kilépés";
            _QuitToolStripMenuItem1.Click += _QuitToolStripMenuItem1_Click;
            // 
            // _PauseToolStripMenuItem
            // 
            _PauseToolStripMenuItem.Name = "_PauseToolStripMenuItem";
            _PauseToolStripMenuItem.Size = new Size(113, 22);
            _PauseToolStripMenuItem.Text = "Szünet";
            _PauseToolStripMenuItem.Click += _PauseToolStripMenuItem_Click;
            // 
            // beállitásokToolStripMenuItem
            // 
            beállitásokToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _EasyToolStripMenuItem, _MediumToolStripMenuItem, HardToolStripMenuItem });
            beállitásokToolStripMenuItem.Name = "beállitásokToolStripMenuItem";
            beállitásokToolStripMenuItem.Size = new Size(75, 20);
            beállitásokToolStripMenuItem.Text = "Beállitások";
            // 
            // _EasyToolStripMenuItem
            // 
            _EasyToolStripMenuItem.Name = "_EasyToolStripMenuItem";
            _EasyToolStripMenuItem.Size = new Size(117, 22);
            _EasyToolStripMenuItem.Text = "Könyű";
            _EasyToolStripMenuItem.Click += _EasyToolStripMenuItem_Click;
            // 
            // _MediumToolStripMenuItem
            // 
            _MediumToolStripMenuItem.Name = "_MediumToolStripMenuItem";
            _MediumToolStripMenuItem.Size = new Size(117, 22);
            _MediumToolStripMenuItem.Text = "Közepes";
            _MediumToolStripMenuItem.Click += _MediumToolStripMenuItem_Click;
            // 
            // HardToolStripMenuItem
            // 
            HardToolStripMenuItem.Name = "HardToolStripMenuItem";
            HardToolStripMenuItem.Size = new Size(117, 22);
            HardToolStripMenuItem.Text = "Nehéz";
            HardToolStripMenuItem.Click += HardToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Control;
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Location = new Point(0, 341);
            label1.Name = "label1";
            label1.Size = new Size(29, 17);
            label1.TabIndex = 1;
            label1.Text = "Idő:";
            // 
            // _timeLabel
            // 
            _timeLabel.AutoSize = true;
            _timeLabel.Location = new Point(35, 341);
            _timeLabel.Name = "_timeLabel";
            _timeLabel.Size = new Size(49, 15);
            _timeLabel.TabIndex = 2;
            _timeLabel.Text = "00:00:00";
            // 
            // LabyrinthWinForms
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(264, 361);
            Controls.Add(_timeLabel);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            MaximumSize = new Size(350, 400);
            MinimumSize = new Size(280, 400);
            Name = "LabyrinthWinForms";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem _newGameToolStripMenuItem;
        private ToolStripMenuItem _QuitToolStripMenuItem1;
        private ToolStripMenuItem beállitásokToolStripMenuItem;
        private ToolStripMenuItem _EasyToolStripMenuItem;
        private ToolStripMenuItem _MediumToolStripMenuItem;
        private ToolStripMenuItem HardToolStripMenuItem;
        private Label label1;
        private Label _timeLabel;
        private ToolStripMenuItem _PauseToolStripMenuItem;
    }
}
