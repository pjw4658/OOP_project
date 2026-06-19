using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using OOP_project.Source.Logic;
using OOP_project.Source.Models;
using OOP_project.Source.Items;
using project_cs.Source.UI.Components;
using project_cs.Source.Items;

namespace project_cs.Source.UI
{
    public partial class GameForm : Form
    {
        // ─────────────────────────────────────────────
        // 게임 코어 및 상태 변수
        // ─────────────────────────────────────────────
        private GameLogic gameLogic;
        private Board board;
        private GameData gameData;
        private System.Windows.Forms.Timer gameTimer;
        private int rows;
        private int cols;
        private Point dragStart;
        private Point dragEnd;
        private bool isDragging = false;
        private bool isPaused = false;

        // ─────────────────────────────────────────────
        // 아이템 시스템 (Inventory 통합) 및 이펙트 변수
        // ─────────────────────────────────────────────
        private ItemInventory inventory;
        private System.Windows.Forms.Timer hintBlinkTimer;
        private bool showHintHighlight = false;
        private bool isJokerMode = false;

        public GameForm(int rows = 10, int cols = 10)
        {
            InitializeComponent();
            this.rows = rows;
            this.cols = cols;

            pnlBoard.Paint += pnlBoard_Paint;
            pnlBoard.MouseDown += pnlBoard_MouseDown;
            pnlBoard.MouseMove += pnlBoard_MouseMove;
            pnlBoard.MouseUp += pnlBoard_MouseUp;

            typeof(Panel).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
                .SetValue(pnlBoard, true, null);

            initGame();
            initTimer();
        }

        private void initGame()
        {
            board = new Board(rows, cols);
            gameData = new GameData();
            board.generateApples();
            gameLogic = new GameLogic(board, gameData);
            gameLogic.startGame();
            lblBoardInfo.Text = $"{rows}x{cols} 보드";

            inventory = new ItemInventory();
            inventory.AddItem(new ShuffleItem());
            inventory.AddItem(new HintItem());
            inventory.AddItem(new JokerItem());

            itemButtonControl.OnShuffleClicked += ItemButtonControl_OnShuffleClicked;
            itemButtonControl.OnHintClicked += ItemButtonControl_OnHintClicked;
            itemButtonControl.OnJokerClicked += ItemButtonControl_OnJokerClicked;

            UpdateInventoryUI();
        }

        private void initTimer()
        {
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 500;
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Start();

            hintBlinkTimer = new System.Windows.Forms.Timer();
            hintBlinkTimer.Interval = 300;
            hintBlinkTimer.Tick += (s, e) =>
            {
                showHintHighlight = !showHintHighlight;
                drawBoard();
            };
        }

        private void UpdateInventoryUI()
        {
            listBox1.Items.Clear();

            Item shuffle = inventory["Shuffle_01"];
            Item hint = inventory["Hint_01"];
            Item joker = inventory["Joker_01"];

            if (shuffle != null)
                listBox1.Items.Add($"섞기 아이템 : {(shuffle.IsAvailable() ? "1" : "0")}개");

            if (hint != null)
                listBox1.Items.Add($"힌트 아이템 : {(hint.IsAvailable() ? "1" : "0")}개");

            if (joker != null)
                listBox1.Items.Add($"조커 아이템 : {(joker.IsAvailable() ? "1" : "0")}개");
        }

        // ─────────────────────────────────────────────
        // [추가됨] 일시정지 버튼 클릭 이벤트
        // ─────────────────────────────────────────────
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (gameLogic == null || gameLogic.isGameOver()) return;

            isPaused = !isPaused;

            if (isPaused)
            {
                gameLogic.pauseGame();
                gameTimer.Stop();
                hintBlinkTimer?.Stop();

                if (sender is Button btn) btn.Text = "계속하기";
            }
            else
            {
                gameLogic.resumeGame();
                gameTimer.Start();
                if (showHintHighlight) hintBlinkTimer?.Start();

                if (sender is Button btn) btn.Text = "일시정지";
            }
        }

        // ─────────────────────────────────────────────
        // 아이템 사용 버튼 이벤트 핸들러 (입력 차단 포함)
        // ─────────────────────────────────────────────

        private void ItemButtonControl_OnShuffleClicked(object sender, EventArgs e)
        {
            if (isPaused) return; // 일시정지 중 차단

            if (inventory.UseItem("Shuffle_01", gameLogic))
            {
                StopHintEffect();
                drawBoard();
                UpdateInventoryUI();
            }
            else
            {
                MessageBox.Show("이미 사용했거나 사용할 수 없습니다.");
            }
        }

        private void ItemButtonControl_OnHintClicked(object sender, EventArgs e)
        {
            if (isPaused) return; // 일시정지 중 차단

            if (inventory.UseItem("Hint_01", gameLogic))
            {
                showHintHighlight = true;
                hintBlinkTimer.Start();
                drawBoard();
                UpdateInventoryUI();
            }
            else
            {
                MessageBox.Show("더 이상 찾을 수 있는 조합이 없거나 아이템을 모두 소모했습니다.");
            }
        }

        private void ItemButtonControl_OnJokerClicked(object sender, EventArgs e)
        {
            if (isPaused) return; // 일시정지 중 차단

            if (inventory["Joker_01"] != null && inventory["Joker_01"].IsAvailable())
            {
                isJokerMode = true;
                StopHintEffect();
                MessageBox.Show("조커로 바꿀 사과를 클릭하세요!");
            }
            else
            {
                MessageBox.Show("조커 아이템을 이미 사용했습니다.");
            }
        }

        private void StopHintEffect()
        {
            hintBlinkTimer?.Stop();
            showHintHighlight = false;
        }

        // ─────────────────────────────────────────────
        // 메인 게임 타이머 및 마우스 컨트롤 
        // ─────────────────────────────────────────────

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            int remaining = gameLogic.getRemainingTime();
            int total = gameData.getTimeLimit();
            int comboCount = gameData.getComboCount();
            float multiplier = 1.0f + (gameData.getComboBonus() / 100f);

            gameData.getCombo().updateTimer(0.5f);

            float comboTimer = gameData.getCombo().getComboTimer();
            float comboTimeLimit = gameData.getCombo().getComboTimeLimit();

            timeControl1.updateTime(remaining, total);
            comboControl1.updateCombo(comboCount, multiplier);
            comboControl1.updateComboTimer((int)comboTimer);
            lblScore.Text = gameData.getScore().ToString();
            drawBoard();

            if (remaining <= 0)
            {
                gameTimer.Stop();
                hintBlinkTimer?.Stop();
                MessageBox.Show("게임 오버!");
            }
        }

        private void pnlBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (isPaused) return; // 일시정지 중 드래그 방지
            if (isJokerMode) return;

            isDragging = true;
            dragStart = e.Location;
            dragEnd = e.Location;
        }

        private void pnlBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            dragEnd = e.Location;
            pnlBoard.Invalidate();
        }

        private void pnlBoard_MouseUp(object sender, MouseEventArgs e)
        {
            if (isPaused) return; // 일시정지 중 클릭 방지

            if (isJokerMode)
            {
                int cellWidth = pnlBoard.Width / cols;
                int cellHeight = pnlBoard.Height / rows;
                int clickedCol = e.X / cellWidth;
                int clickedRow = e.Y / cellHeight;

                if (clickedCol >= 0 && clickedCol < cols && clickedRow >= 0 && clickedRow < rows)
                {
                    Cell targetCell = board.getCell(new Position(clickedCol, clickedRow));

                    if (inventory.UseItem("Joker_01", gameLogic, targetCell))
                    {
                        isJokerMode = false;
                        drawBoard();
                        UpdateInventoryUI();
                    }
                }
                return;
            }

            if (!isDragging) return;
            isDragging = false;
            dragEnd = e.Location;

            List<Cell> selected = getSelectedCells();

            if (selected.Count > 0)
            {
                bool success = gameLogic.checkSum(selected);
                if (success)
                {
                    lblScore.Text = gameData.getScore().ToString();
                    StopHintEffect();
                }
            }

            pnlBoard.Invalidate();
        }

        private List<Cell> getSelectedCells()
        {
            int cellWidth = pnlBoard.Width / cols;
            int cellHeight = pnlBoard.Height / rows;

            int startCol = Math.Min(dragStart.X, dragEnd.X) / cellWidth;
            int startRow = Math.Min(dragStart.Y, dragEnd.Y) / cellHeight;
            int endCol = Math.Max(dragStart.X, dragEnd.X) / cellWidth;
            int endRow = Math.Max(dragStart.Y, dragEnd.Y) / cellHeight;

            startCol = Math.Max(0, Math.Min(startCol, cols - 1));
            startRow = Math.Max(0, Math.Min(startRow, rows - 1));
            endCol = Math.Max(0, Math.Min(endCol, cols - 1));
            endRow = Math.Max(0, Math.Min(endRow, rows - 1));

            List<Cell> selected = new List<Cell>();
            for (int r = startRow; r <= endRow; r++)
            {
                for (int c = startCol; c <= endCol; c++)
                {
                    Cell cell = board.getCell(new Position(c, r));
                    if (cell != null && cell.hasApple())
                    {
                        selected.Add(cell);
                    }
                }
            }

            return selected;
        }

        private void drawBoard()
        {
            pnlBoard.Invalidate();
        }

        // ─────────────────────────────────────────────
        // 렌더링 영역 (시각화 코드)
        // ─────────────────────────────────────────────

        private void pnlBoard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int cellWidth = pnlBoard.Width / cols;
            int cellHeight = pnlBoard.Height / rows;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Cell cell = board.getCell(new Position(c, r));
                    if (cell == null || !cell.hasApple()) continue;

                    int x = c * cellWidth;
                    int y = r * cellHeight;

                    g.FillRectangle(Brushes.LightGreen, x + 1, y + 1, cellWidth - 2, cellHeight - 2);

                    Font font = new Font("맑은 고딕", cellWidth * 0.3f);
                    if (cell.apple.isJoker())
                    {
                        g.FillRectangle(Brushes.Orange, x + 1, y + 1, cellWidth - 2, cellHeight - 2);
                        SizeF jSize = g.MeasureString("J", font);
                        g.DrawString("J", font, Brushes.DarkRed, x + (cellWidth - jSize.Width) / 2, y + (cellHeight - jSize.Height) / 2);
                    }
                    else
                    {
                        string value = cell.apple.getValue().ToString();
                        SizeF textSize = g.MeasureString(value, font);
                        float textX = x + (cellWidth - textSize.Width) / 2;
                        float textY = y + (cellHeight - textSize.Height) / 2;
                        g.DrawString(value, font, Brushes.DarkGreen, textX, textY);
                    }

                    g.DrawRectangle(Pens.Green, x, y, cellWidth - 1, cellHeight - 1);
                }
            }

            HintItem currentHintItem = inventory["Hint_01"] as HintItem;

            if (currentHintItem != null && currentHintItem.CachedHints != null && showHintHighlight)
            {
                using (Pen hintPen = new Pen(Color.Gold, 4))
                {
                    int showCount = Math.Min(currentHintItem.CachedHints.Count, 3);
                    for (int i = 0; i < showCount; i++)
                    {
                        AvailableApple group = currentHintItem.CachedHints[i];
                        List<Cell> hintCells = group.getCells();

                        if (hintCells != null)
                        {
                            foreach (Cell c in hintCells)
                            {
                                Position pos = c.getPosition();
                                int hx = pos.getX() * cellWidth;
                                int hy = pos.getY() * cellHeight;
                                g.DrawRectangle(hintPen, hx + 2, hy + 2, cellWidth - 4, cellHeight - 4);
                            }
                        }
                    }
                }
            }

            if (isDragging)
            {
                int x = Math.Min(dragStart.X, dragEnd.X);
                int y = Math.Min(dragStart.Y, dragEnd.Y);
                int w = Math.Abs(dragEnd.X - dragStart.X);
                int h = Math.Abs(dragEnd.Y - dragStart.Y);

                using (Pen dragPen = new Pen(Color.Red, 2))
                {
                    g.DrawRectangle(dragPen, x, y, w, h);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            drawBoard();
        }
    }
}