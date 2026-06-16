using System;

namespace OOP_project.Source.Models
{
    /// <summary>
    /// 게임 보드를 구성하고 셀 생성, 사과 배치를 관리하는 클래스입니다.
    /// </summary>
    public class Board
    {
        private int boardRows;
        private int boardCols;
        private Cell[,] cells;
        private Random random;

        // GameLogic에서 board.rows, board.cols 필드에 접근하므로 카멜케이스 속성을 열어줍니다.
        public int rows => boardRows;
        public int cols => boardCols;

        public Board(int rows, int cols)
        {
            this.boardRows = rows;
            this.boardCols = cols;
            this.random = new Random();
            createBoard();
        }

        public bool createBoard()
        {
            cells = new Cell[boardRows, boardCols];
            for (int r = 0; r < boardRows; r++)
            {
                for (int c = 0; c < boardCols; c++)
                {
                    cells[r, c] = new Cell(new Position(c, r));
                }
            }
            return true;
        }

        public void generateApples()
        {
            for (int r = 0; r < boardRows; r++)
            {
                for (int c = 0; c < boardCols; c++)
                {
                    // 7% 확률로 특수 조커 사과 생성, 그 외에는 1~9 사이의 일반 사과 생성
                    if (random.Next(0, 100) < 7)
                    {
                        cells[r, c].apple = new JokerApple();
                    }
                    else
                    {
                        cells[r, c].apple = new NormalApple(random.Next(1, 10));
                    }
                }
            }
        }

        public void refillEmptyCells()
        {
            for (int r = 0; r < boardRows; r++)
            {
                for (int c = 0; c < boardCols; c++)
                {
                    if (!cells[r, c].hasApple())
                    {
                        cells[r, c].apple = new NormalApple(random.Next(1, 10));
                    }
                }
            }
        }

        public Cell getCell(Position position)
        {
            int x = position.getX();
            int y = position.getY();

            if (x >= 0 && x < boardCols && y >= 0 && y < boardRows)
            {
                return cells[y, x];
            }
            return null;
        }

        public void clearBoard()
        {
            for (int r = 0; r < boardRows; r++)
            {
                for (int c = 0; c < boardCols; c++)
                {
                    cells[r, c].removeApple();
                }
            }
        }

        /// <summary>
        /// Program.cs에서 콘솔 정밀 검증용으로 호출하는 보드 그리기 메서드입니다.
        /// </summary>
        public void displayBoardConsole()
        {
            for (int r = 0; r < boardRows; r++)
            {
                for (int c = 0; c < boardCols; c++)
                {
                    if (cells[r, c].hasApple())
                    {
                        if (cells[r, c].apple.isJoker())
                            Console.Write("J ");
                        else
                            Console.Write($"{cells[r, c].apple.getValue()} ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}