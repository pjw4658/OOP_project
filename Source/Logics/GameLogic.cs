using System;
using System.Collections.Generic;
using OOP_project.Source.Models;

namespace OOP_project.Source.Logic
{
    /// <summary>
    /// [이상운/박재우 담당] 게임의 규칙 검증, 시간 제어 및 사과 조합 탐색을 총괄하는 핵심 로직 클래스입니다.
    /// 조장님의 지침(클래스 대문자, 메서드/변수 소문자 시작) 및 순수 로직 구조를 준수합니다.
    /// </summary>
    public class GameLogic
    {
        // 멤버 변수 (소문자 시작 camelCase)
        private Board board;                  // 연결된 게임 보드 객체
        private GameData gameData;            // [최창무 담당] 게임 데이터 객체 참조
        private List<Cell> selectedCells;     // 플레이어가 드래그 중인 셀 목록 실시간 저장 변수
        private DateTime gameStartTime;       // 게임이 시작된 실제 시스템 시간 기록

        /// <summary>
        /// 생성자입니다. Board 객체와 창무 조원의 GameData 객체를 주입받아 로직을 초기화합니다.
        /// </summary>
        public GameLogic(Board board, GameData gameData)
        {
            this.board = board;
            this.gameData = gameData;
            this.selectedCells = new List<Cell>();
        }

        /// <summary>
        /// 게임을 정식으로 시작합니다. 데이터 상태를 초기화하고 보드에 사과를 새로 배치합니다.
        /// </summary>
        public void startGame()
        {
            gameData.resetData();
            gameStartTime = DateTime.Now;
            board.generateApples();
            selectedCells.Clear();
        }

        /// <summary>
        /// 현재 남은 시간을 초 단위로 정밀 계산하여 gameData에 동기화하고 반환합니다.
        /// </summary>
        public int getRemainingTime()
        {
            if (gameData.checkGameOver()) return 0;

            TimeSpan elapsed = DateTime.Now - gameStartTime;
            int remaining = gameData.getTimeLimit() - (int)elapsed.TotalSeconds;

            if (remaining <= 0)
            {
                remaining = 0;
                gameData.setGameOver(true);
            }

            gameData.setCurrentTime(remaining);
            return remaining;
        }

        /// <summary>
        /// 사용자가 선택한 셀들의 사과 수치 합을 검사하여 규칙 충족 시 제거하는 메서드입니다.
        /// </summary>
        public bool checkSum(List<Cell> selectedCells)
        {
            this.selectedCells = selectedCells;

            if (isGameOver() || this.selectedCells == null || this.selectedCells.Count == 0)
            {
                return false;
            }

            int sum = 0;
            bool hasJoker = false;

            foreach (Cell cell in this.selectedCells)
            {
                if (cell == null || !cell.hasApple())
                {
                    return false;
                }

                if (cell.apple.isJoker())
                {
                    hasJoker = true;
                }
                else
                {
                    sum += cell.apple.getValue();
                }
            }

            bool isValidSelection = false;
            if (hasJoker)
            {
                isValidSelection = true;
            }
            else
            {
                if (sum == 10)
                {
                    isValidSelection = true;
                }
            }

            if (isValidSelection)
            {
                gameData.getCombo().increaseCombo();

                foreach (Cell cell in this.selectedCells)
                {
                    cell.removeApple();
                }

                float multiplier = 1.0f + (gameData.getCombo().getComboBonus() / 100f);
                int earnedScore = calculateScore(this.selectedCells, multiplier);

                if (hasJoker)
                {
                    gameData.addScore(100 + earnedScore);
                }
                else
                {
                    gameData.addScore(earnedScore);
                }

                board.refillEmptyCells();
                this.selectedCells.Clear();
                return true;
            }

            gameData.getCombo().resetCombo();
            this.selectedCells.Clear();
            return false;
        }

        /// <summary>
        /// 제거된 셀의 개수 기반 배율 점수를 연산합니다.
        /// </summary>
        public int calculateScore(List<Cell> cell, float multiplier)
        {
            if (cell == null) return 0;
            int baseScore = cell.Count * 10;
            return (int)(baseScore * multiplier);
        }

        /// <summary>
        /// 게임오버 상태 여부를 판정하여 반환합니다.
        /// </summary>
        public bool isGameOver()
        {
            return gameData.checkGameOver() || getRemainingTime() <= 0;
        }

        /// <summary>
        /// [박재우 담당] 게임 오버 후 보드 전체에서 합이 10인 직사각형 조합을 탐색하여 리스트로 반환합니다.
        /// </summary>
        public List<AvailableApple> findMissedApple()
        {
            List<AvailableApple> result = new List<AvailableApple>();

            try
            {
                for (int r1 = 0; r1 < board.rows; r1++)
                {
                    for (int c1 = 0; c1 < board.cols; c1++)
                    {
                        for (int r2 = r1; r2 < board.rows; r2++)
                        {
                            for (int c2 = c1; c2 < board.cols; c2++)
                            {
                                List<Cell> cells = getCellsInRect(r1, c1, r2, c2);
                                AvailableApple comboApple = new AvailableApple(cells);

                                if (comboApple.isValid())
                                {
                                    comboApple.show(); // 박재우 조원이 구현한 기본 이펙트 메서드 호출
                                    result.Add(comboApple);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[findMissedApple 오류 발생] {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// 지정된 두 좌표로 형성되는 직사각형 내부의 유효한 셀 목록을 추출하는 헬퍼 메서드입니다.
        /// </summary>
        private List<Cell> getCellsInRect(int r1, int c1, int r2, int c2)
        {
            List<Cell> cells = new List<Cell>();

            for (int r = r1; r <= r2; r++)
            {
                for (int c = c1; c <= c2; c++)
                {
                    Position pos = new Position(c, r);
                    Cell cell = board.getCell(pos);

                    if (cell != null && cell.hasApple())
                    {
                        cells.Add(cell);
                    }
                }
            }

            return cells; // 붕 떠있던 리턴 구문과 괄호 쌍을 완벽 복구했습니다.
        }
    }
}