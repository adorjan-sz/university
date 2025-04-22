namespace PlantVsZombies_WINFORMS.View
{
    partial class Field
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
            fieldButton = new Button();
            SuspendLayout();
            // 
            // fieldButton
            // 
            fieldButton.BackColor = Color.Green;
            fieldButton.Dock = DockStyle.Fill;
            fieldButton.FlatStyle = FlatStyle.Flat;
            fieldButton.Location = new Point(0, 0);
            fieldButton.Margin = new Padding(0);
            fieldButton.MaximumSize = new Size(40, 40);
            fieldButton.MinimumSize = new Size(40, 40);
            fieldButton.Name = "fieldButton";
            fieldButton.Size = new Size(40, 40);
            fieldButton.TabIndex = 0;
            fieldButton.UseVisualStyleBackColor = false;
            fieldButton.Click += fieldButton_Click;
            // 
            // Field
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(fieldButton);
            Margin = new Padding(0);
            MaximumSize = new Size(40, 40);
            MinimumSize = new Size(40, 40);
            Name = "Field";
            Size = new Size(40, 40);
            ResumeLayout(false);
        }

        #endregion

        private Button fieldButton;
    }
}
