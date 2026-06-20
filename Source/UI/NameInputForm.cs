using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace project_cs.Source.UI
{
    public partial class NameInputForm : Form
    {
        public string PlayerName => txtName.Text.Trim();

        public NameInputForm(int score, string boardSize, string level)
        {
            InitializeComponent();
            lblMessage.Text = $"[{boardSize}-{level}]  최종 점수: {score}점\n이름을 입력해 주세요.";

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("닉네임을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
