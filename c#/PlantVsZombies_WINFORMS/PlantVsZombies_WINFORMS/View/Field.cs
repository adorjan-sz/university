using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlantVsZombies_WINFORMS.View
{
    public partial class Field : UserControl
    {
        private int x;
        private int y;

        public int X
        {
            get { return x; }
            set
            {
                x = value;
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                y = value;
            }
        }
        public event EventHandler? ButtonClicked;
        public Field(int x, int y)
        {
            InitializeComponent();
        }

        private void fieldButton_Click(object sender, EventArgs e)
        {
            ButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeToZombieColor()
        {
            fieldButton.BackgroundImage = Properties.Resources.zombie1;
            fieldButton.BackColor = Color.Red;
        }

        public void ChangeToPeashooterColor()
        {
            fieldButton.BackgroundImage = Properties.Resources.peashooter1;
            fieldButton.BackColor = Color.LightGreen;
        }

        internal void ChangeToBlankColor()
        {
            fieldButton.BackgroundImage = null;
            fieldButton.BackColor = Color.Green;
        }
    }
}
