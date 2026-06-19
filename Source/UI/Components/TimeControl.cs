using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace project_cs.Source.UI.Components
{
    public partial class TimeControl : UserControl
    {
        private int maxTime;

        public TimeControl()
        {
            InitializeComponent();
            maxTime = 60;
            pbTimer.Maximum = maxTime;
            pbTimer.Value = maxTime;
        }

        // GameForm 타이머에서 매 초 호출
        public void updateTime(int remainingSeconds, int totalSeconds)
        {
            maxTime = totalSeconds;
            pbTimer.Maximum = totalSeconds;
            pbTimer.Value = Math.Max(0, remainingSeconds);
            lblTime.Text = $"{remainingSeconds}s";
        }
    }
}