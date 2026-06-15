using System;
using System.Collections.Generic;
using OOP_project.Source.Models;

namespace OOP_project.Source.Logics
{
    /// <summary>
    /// 게임의 전체적인 규칙, 점수, 제한 시간 및 사과 제거 조건을 관리하는 게임 로직 클래스입니다.
    /// </summary>
    public class GameLogic
    {
        // 멤버 변수
        private Board board;            // 연결된 게임 보드 객체
        private int score;              // 현재 플레이어의 점수
        private int timeLimit;          // 게임 총 제한 시간 (초 단위)
        private DateTime gameStartTime; // 게임이 시작된 실제 시간 기록
        private bool isGameOver;        // 게임오버 플래그

        /// <summary>
        /// 생성자입니다. 플레이할 보드 객체와 제한 시간을 받아 로직을 초기화합니다.
        /// </summary>
        /// <param name="board"> 구현한 Board 객체</param>
        /// <param name="timeLimitSeconds">제한 시간(초)</param>
        public GameLogic(Board board, int timeLimitSeconds)
        {
            this.board = board;
            this.timeLimit = timeLimitSeconds;
            this.score = 0;
            this.isGameOver = false;
        }

        /// <summary>
        /// 게임을 정식으로 시작합니다. 보드에 사과를 새로 배치하고 타이머를 가동합니다.
        /// </summary>
        public void StartGame()
        {
            score = 0;
            isGameOver = false;
            gameStartTime = DateTime.Now; // 현재 시스템 시간을 시작 시간으로 등록
            board.GenerateApples();       // 보드 사과 무작위 생성 규칙 실행
        }

        /// <summary>
        /// [시간 관리 담당] 현재 남은 시간을 초 단위로 정밀 계산하여 반환합니다.
        /// 시간이 종료되면 자동으로 게임오버 처리를 합니다.
        /// </summary>
        public int GetRemainingTime()
        {
            if (isGameOver) return 0;

            // 시작 시간으로부터 흘러간 시간 계산
            TimeSpan elapsed = DateTime.Now - gameStartTime;
            int remaining = timeLimit - (int)elapsed.TotalSeconds;

            // 제한 시간이 다 되었을 경우 게임오버 처리
            if (remaining <= 0)
            {
                remaining = 0;
                isGameOver = true;
            }

            return remaining;
        }

        /// <summary>
        /// [핵심 로직 담당] 사용자가 선택한 셀들의 사과 수치 합을 검사하여 규칙 충족 시 제거하는 메서드입니다.
        /// </summary>
        /// <param name="selectedCells">플레이어가 마우스/터치로 드래그하여 선택한 Cell들의 리스트</param>
        /// <returns>합이 10이 되거나 조커 조건에 맞아 제거 및 리필에 성공하면 true, 실패하면 false</returns>
        public bool CheckSum(List<Cell> selectedCells)
        {
            // 1. 예외 처리: 게임오버 상태이거나 선택된 셀이 없으면 즉시 실패
            if (IsGameOver() || selectedCells == null || selectedCells.Count == 0)
            {
                return false;
            }

            int sum = 0;
            bool hasJoker = false;

            // 2. 선택된 셀들의 사과 상태 검사 및 수치 합산
            foreach (Cell cell in selectedCells)
            {
                // 셀이 비어있거나 사과가 이미 없는 칸이라면 유효하지 않은 드래그로 판단
                if (cell == null || !cell.HasApple())
                {
                    return false; 
                }

                //  Apple의 계층 구조 규칙 확인
                if (cell.apple.IsJoker)
                {
                    hasJoker = true; // 조커 사과 포함 여부 체크
                }
                else
                {
                    sum += cell.apple.Value; // 일반 사과 값 누적
                }
            }

            // 3. 사과 게임 규칙 검증
            bool isValidSelection = false;

            if (hasJoker)
            {
                // [규칙 A] 조커 사과가 드래그 영역에 포함되어 있다면 특수 와일드카드로 인정되어 무조건 제거 가능
                isValidSelection = true;
            }
            else
            {
                // [규칙 B] 일반 사과들만 있을 때는 총합이 정확히 '10'이어야 제거 가능
                if (sum == 10)
                {
                    isValidSelection = true;
                }
            }

            // 4. 조건 만족 시 실제 데이터 처리 (점수 가산, 사과 제거 및 리필)
            if (isValidSelection)
            {
                // Cell에 구현해둔 RemoveApple API를 순회하며 호출하여 사과 터트림
                foreach (Cell cell in selectedCells)
                {
                    cell.RemoveApple();
                }

                // 점수 가산 규칙 (일반 성공은 개당 10점, 조커는 보너스 100점)
                if (hasJoker)
                {
                    score += 100 + (selectedCells.Count * 10);
                }
                else
                {
                    score += selectedCells.Count * 10;
                }

                // 사과가 터진 자리에서 Board에 구현해둔 리필 기능 작동
                board.RefillEmptyCells();

                return true; // 규칙 검사 및 반영 성공
            }

            return false; // 합이 10이 되지 않아 실패
        }

        // 현재 점수 가져오기 외부 API
        public int GetScore() => score;

        // 게임오버 상태 여부 반환 API
        public bool IsGameOver() => isGameOver || GetRemainingTime() <= 0;

        // 게임오버 후 보드 전체에서 합이 10인 직사각형 조합 탐색
        // 찾은 조합마다 Show() 호출하여 UI 하이라이트 표시 후 리스트 반환
        public List<AvailableApple> FindMissedApple()
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
                                List<Cell> cells = GetCellsInRect(r1, c1, r2, c2);
                                AvailableApple combo = new AvailableApple(cells);

                                if (combo.IsValid())
                                {
                                    combo.Show();
                                    result.Add(combo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[FindMissedApple 오류] {e.Message}");
            }

            return result;
        }

        // FindMissedApple() 내부 헬퍼
        // 직사각형 범위(r1,c1 ~ r2,c2) 안의 사과 있는 셀만 추출
        private List<Cell> GetCellsInRect(int r1, int c1, int r2, int c2)
        {
            List<Cell> cells = new List<Cell>();

            for (int r = r1; r <= r2; r++)
            {
                for (int c = c1; c <= c2; c++)
                {
                    Position pos = new Position(c, r);
                    Cell cell = board.GetCell(pos);

                    if (cell != null && cell.HasApple())
                    {
                        cells.Add(cell);
                    }
                }
            }

            return cells;
        }
    }
}
