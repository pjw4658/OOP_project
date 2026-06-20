using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OOP_project.Source.Ranking;

namespace project_cs.Source.UI.Components
{
    public partial class RankingBoardControl : UserControl
    {
        public RankingBoardControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// RankingManager의 데이터를 읽어 소형/중형/대형 탭의 DataGridView에 표시합니다.
        /// </summary>
        public void loadRankings(RankingManager rankingManager)
        {
            rankingManager.loadRankings();
            fillGrid(dgvsRanking, rankingManager.getTopRankings(10, "소형"));
            fillGrid(dgvmRanking, rankingManager.getTopRankings(10, "중형"));
            fillGrid(dgvbRanking, rankingManager.getTopRankings(10, "대형"));
        }

        private void fillGrid(DataGridView dgv, List<RankingEntry> entries)
        {
            dgv.Rows.Clear();
            for (int i = 0; i < entries.Count; i++)
            {
                dgv.Rows.Add(
                    i + 1,
                    entries[i].getPlayerName(),
                    entries[i].getScore(),
                    entries[i].getAchievedTime().ToString("yyyy-MM-dd HH:mm")
                );
            }
        }

        private void dgvbRanking_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
