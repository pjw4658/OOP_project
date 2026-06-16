using OOP_project.Source.Logic;
using OOP_project.Source.Models;
using OOP_project.Source.Models;
using System;
using System.Collections.Generic;

namespace OOP_project.Source.Logic
{
    /// <summary>
    /// [이상운/박재우 담당] 게임의 전체적인 규칙, 점수 배율 연산, 시간 제어 및 사과 조합 탐색을 총괄하는 핵심 로직 클래스입니다.
    /// 조장님 그라운드 룰(클래스 대문자, 변수/메서드 소문자 시작) 및 디렉토리 구조를 완벽히 준수합니다.
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
        /// [이상운 담당] 게임을 정식으로 시작합니다. 데이터 상태를 초기화하고 보드에 사과를 새로 배치합니다.
        /// </summary>
        public void startGame()
        {
            gameData.resetData();          // GameData 초기화
            gameStartTime = DateTime.Now;  // 현재 시스템 시간을 시작 시간으로 등록
            board.generateApples();        // 보드 사과 무작위 생성 규칙 실행
            selectedCells.Clear();
        }

        /// <summary>
        /// [시간 관리 담당 - 이상운] 현재 남은 시간을 초 단위로 정밀 계산하여 gameData에 동기화하고 반환합니다.
        /// </summary>
        public int getRemainingTime()
        {
            if (gameData.checkGameOver()) return 0;

            // 시작 시간으로부터 흘러간 시간 계산
            TimeSpan elapsed = DateTime.Now - gameStartTime;
            int remaining = gameData.getTimeLimit() - (int)elapsed.TotalSeconds;

            // 제한 시간이 다 되었을 경우 게임오버 처리
            if (remaining <= 0)
            {
                remaining = 0;
                gameData.setGameOver(true);
            }

            gameData.setCurrentTime(remaining); // gameData 실시간 잔여 시간 갱신
            return remaining;
        }

        /// <summary>
        /// [핵심 로직 담당 - 이상운] 사용자가 선택한 셀들의 사과 수치 합을 검사하여 규칙 충족 시 제거하는 메서드입니다.
        /// </summary>
        public bool checkSum(List<Cell> selectedCells)
        {
            this.selectedCells = selectedCells;

            // 1. 예외 처리: 게임오버 상태이거나 선택된 셀이 없으면 즉시 실패
            if (isGameOver() || this.selectedCells == null || this.selectedCells.Count == 0)
            {
                return false;
            }

            int sum = 0;
            bool hasJoker = false;

            // 2. 선택된 셀들의 사과 상태 검사 및 수치 합산
            foreach (Cell cell in this.selectedCells)
            {
                if (cell == null || !cell.hasApple())
                {
                    return false;
                }

                // 명세서 규칙 확인 (isJoker, getValue 메서드 호출)
                if (cell.apple.isJoker())
                {
                    hasJoker = true;
                }
                else
                {
                    sum += cell.apple.getValue();
                }
            }

            // 3. 사과 게임 규칙 검증
            bool isValidSelection = false;
            if (hasJoker)
            {
                isValidSelection = true; // 조커 사과 포함 시 무조건 매치 성공
            }
            else
            {
                if (sum == 10)
                {
                    isValidSelection = true; // 일반 사과는 합이 10이어야 함
                }
            }

            // 4. 조건 만족 시 실제 데이터 처리
            if (isValidSelection)
            {
                // 성공 시 창무 조원 데이터 내부의 콤보 카운트 증가
                gameData.getCombo().increaseCombo();

                // Cell에 구현해둔 removeApple 호출
                foreach (Cell cell in this.selectedCells)
                {
                    cell.removeApple();
                }

                // 콤보 보너스를 계산하여 배율(multiplier)로 환산
                float multiplier = 1.0f + (gameData.getCombo().getComboBonus() / 100f);

                // 명세서 지침에 따른 점수 연산 메서드 호출
                int earnedScore = calculateScore(this.selectedCells, multiplier);

                // gameData의 addScore를 활용해 최종 데이터 누적
                if (hasJoker)
                {
                    gameData.addScore(100 + earnedScore); // 조커 보너스 가산
                }
                else
                {
                    gameData.addScore(earnedScore);
                }

                board.refillEmptyCells(); // 비어 있는 칸 새 사과 리필
                this.selectedCells.Clear();
                return true;
            }

            // 5. 실패 시 연속 콤보 초기화
            gameData.getCombo().resetCombo();
            this.selectedCells.Clear();
            return false;
        }

        /// <summary>
        /// [명세서 필수 메서드 - 이상운] 제거된 셀의 개수(사과 1개당 10점)에 콤보 배율을 곱해 최종 점수를 계산합니다.
        /// </summary>
        public int calculateScore(List<Cell> cell, float multiplier)
        {
            if (cell == null) return 0;

            int baseScore = cell.Count * 10;
            return (int)(baseScore * multiplier);
        }

        /// <summary>
        /// 게임오버 상태 여부를 판정하여 반환하는 외부 API입니다.
        /// </summary>
        public bool isGameOver()
        {
            return gameData.checkGameOver() || getRemainingTime() <= 0;
        }

        /// <summary>
        /// [박재우 담당] 게임 오버 후 보드에 남아있는 모든 직사각형 범위를 순회하며 합이 10이 되는 사과 조합을 탐색합니다.
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

                                // 합이 10이 되는 유효한 사과 조합인지 검증
                                if (comboApple.isValid())
                                {
                                    comboApple.show(); // 화면 하이라이트 트리거 호출
                                    result.Add(comboApple);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[findMissedApple 런타임 오류 발생] {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// [박재우 담당 헬퍼] 지정된 두 좌표로 형성되는 직사각형 내부의 유효한 셀 목록을 추출합니다.
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

            return cells;
        }
    }
}