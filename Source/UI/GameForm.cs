using System;
using System.Windows.Forms;
using OOP_project.Source.Logics;
using OOP_project.Source.Models;

namespace project_cs.Source.UI
{
    public partial class GameForm : Form
    {
        private GameLogic gameLogic;
        private Board board;

        public GameForm()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            // 기본 중형 보드 10x10으로 초기화
            board = new Board(10, 10);
            board.GenerateApples();
            gameLogic = new GameLogic(board);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}