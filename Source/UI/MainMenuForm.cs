using System;
using System.Windows.Forms;

namespace project_cs.Source.UI
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            rbMedium.Checked = true; // 기본값 중형
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int rows, cols;

            if (rbSmall.Checked)
            {
                rows = 7; cols = 7;
            }
            else if (rbLarge.Checked)
            {
                rows = 14; cols = 14;
            }
            else
            {
                rows = 10; cols = 10; // 기본 중형
            }

            GameForm gameForm = new GameForm(rows, cols);
            gameForm.Show();
            this.Hide();
        }
    }
}