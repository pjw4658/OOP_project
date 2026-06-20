using System;
using System.Windows.Forms;
using OOP_project.Source.Logic;
using OOP_project.Source.Ranking;

namespace project_cs.Source.UI
{
    public partial class MainMenuForm : Form
    {
        private RankingManager rankingManager = new RankingManager();

        public MainMenuForm()
        {
            InitializeComponent();
            rbMedium.Checked = true;
            rbNormal.Checked = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            rankingBoardControl1.loadRankings(rankingManager);
        }

        /// <summary>
        /// GameForm에서 돌아올 때 랭킹을 새로 불러와 화면에 반영합니다.
        /// </summary>
        public void RefreshRankings()
        {
            rankingBoardControl1.loadRankings(rankingManager);
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
                rows = 10; cols = 10;
            }

            Difficulty difficulty;

            if (rbEasy.Checked)
                difficulty = Difficulty.Easy;
            else if (rbHard.Checked)
                difficulty = Difficulty.Hard;
            else
                difficulty = Difficulty.Normal;

            GameForm gameForm = new GameForm(rows, cols, difficulty, this);
            gameForm.Show();
            this.Hide();
        }
    }
}