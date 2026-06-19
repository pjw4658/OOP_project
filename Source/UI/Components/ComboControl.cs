using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace project_cs.Source.UI.Components
{
    public partial class ComboControl : UserControl
    {
        private int maxComboTime = 5; // 콤보 유지시간 최대값 (초)
        private int currentComboTime = 0;

        public ComboControl()
        {
            InitializeComponent();
            pbComboTimer.Maximum = maxComboTime;
            pbComboTimer.Value = 0;
        }

        // 콤보 배율 업데이트 (GameForm 타이머에서 호출)
        public void updateCombo(int comboCount, float multiplier)
        {
            lblCombo.Text = $"x{multiplier:F1}";
        }

        // 콤보 타이머 업데이트
        public void updateComboTimer(int remainingTime)
        {
            currentComboTime = Math.Max(0, remainingTime);
            pbComboTimer.Value = Math.Min(currentComboTime, maxComboTime);
        }

        private void AppleButton_Load(object sender, EventArgs e)
        {
        }
    }
}