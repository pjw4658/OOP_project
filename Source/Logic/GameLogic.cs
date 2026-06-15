using System;
using System.Collections.Generic;
using OOP_project.Source.Models;

namespace OOP_project.Source.Logics
{
    /// <summary>
    /// [이상운/박재우 담당] 게임의 전체적인 규칙, 점수 배율 연산, 시간 제어 및 사과 조합 탐색을 총괄하는 핵심 로직 클래스입니다.
    /// 팀 그라운드 룰(camelCase) 및 API 명세서 구조를 완벽히 준수합니다.
    /// </summary>
    public class gameLogic
    {
        // 멤버 변수 (자체 데이터 필드를 제거하고 명세서 3-4 규칙에 따라 가방 객체와 보드만 참조)
        private board board;                  // 연결된 게임 보드 객체
        private gameData gameData;            // [최창무 담당] 점수, 시간 등을 관리하는 데이터 객체
        private List<cell> selectedCells;     // 플레이어가 드래그 중인 셀 목록 실시간 저장 변수
        private DateTime gameStartTime;       // 게임이 시작된 실제 시스템 시간 기록

        /// <summary>
        /// 생성자입니다. Board 객체와 창무 조원의 gameData 객체를 주입받아 로직을 초기화합니다.
        /// </summary>
        public gameLogic(board board, gameData gameData)
        {
            this.board = board;
            this.gameData = gameData;
            this.selectedCells = new List<cell>();
        }

        /// <summary>
        /// [이상운 담당] 게임을 정식으로 시작합니다. 데이터 상태를 초기화하고 보드에 사과를 새로 배치합니다.
        /// </summary>
        public void startGame()
        {
            gameData.resetData();          // 창무 조원 파트 데이터 리셋 트리거
            gameStartTime = DateTime.Now;  // 현재 시스템 시간을 시작 시간으로 등록
            board.generateApples();        // 보드 사과 무작위 생성 규칙 실행
            selectedCells.Clear();
        }

        /// <summary>
        /// [시간 관리 담당 - 이상운] 현재 남은 시간을 초 단위로 정밀 계산하여 gameData에 동기화하고 반환합니다.
        /// </summary>
        public int getRemainingTime()
        {
            if (gameData.isGameOverValue) return 0;

            // 시작 시간으로부터 흘러간 시간 계산
            TimeSpan elapsed = DateTime.Now - gameStartTime;
            int remaining = gameData.timeLimitValue - (int)elapsed.TotalSeconds;

            // 제한 시간이 다 되었을 경우 게임오버 처리
            if (remaining <= 0)
            {
                remaining = 0;
                gameData.isGameOverValue = true;
            }

            gameData.currentTimeValue = remaining; // gameData 실시간 잔여 시간 갱신
            return remaining;
        }

        /// <summary>
        /// [핵심 로직 담당 - 이상운] 사용자가 선택한 셀들의 사과 수치 합을 검사하여 규칙 충족 시 제거하는 메서드입니다.
        /// </summary>
        public bool checkSum(List<cell> selectedCells)
        {
            this.selectedCells = selectedCells;

            // 1. 예외 처리: 게임오버 상태이거나 선택된 셀이 없으면 즉시 실패
            if (isGameOver() || this.selectedCells == null || this.selectedCells.Count == 0)
            {
                return false;
            }

            int sum = 0;
            bool hasJoker = false;

            // 2. 선택된 셀들의 사과 상태 검사 및 수치 합산 (명세서의 메서드 기반 호출로 교정)
            foreach (cell cell in this.selectedCells)
            {
                if (cell == null || !cell.hasApple())
                {
                    return false; 
                }

                // 명세서 2-1 Apple 계층 구조 규칙 확인 (isJoker 메서드 호출)
                if (cell.apple.isJoker())
                {
                    hasJoker = true; 
                }
                else
                {
                    sum += cell.apple.getValue(); // getValue 메서드 호출
                }
            }

            // 3. 사과 게임 규칙 검증
            bool isValidSelection = false;
            if (hasJoker)
            {
                isValidSelection = true; // 조커 사과 포함 시 프리패스 제거 가능
            }
            else
            {
                if (sum == 10)
                {
                    isValidSelection = true; // 일반 사과는 합이 10이어야 함
                }
            }

            // 4. 조건 만족 시 실제 데이터 처리 (gameData의 콤보 및 점수 반영 API 연동)
            if (isValidSelection)
            {
                // 성공 시 창무 조원 데이터 내부의 콤보 카운트 증가
                gameData.combo.increaseCombo();

                // Cell에 구현해둔 removeApple API를 순회 호출
                foreach (cell cell in this.selectedCells)
                {
                    cell.removeApple();
                }

                // 콤보 보너스를 계산하여 배율(multiplier)로 환산
                float multiplier = 1.0f + (gameData.combo.getComboBonus() / 100f);
                
                // 명세서 3-4 지침에 따른 점수 연산 메서드 호출
                int earnedScore = calculateScore(this.selectedCells, multiplier);

                // gameData의 addScore API를 활용해 최종 데이터 누적
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
            gameData.combo.resetCombo();
            this.selectedCells.Clear();
            return false; 
        }

        /// <summary>
        /// [명세서 3-4 필수 메서드 - 이상운] 제거된 셀의 개수(사과 1개당 10점)에 콤보 배율을 곱해 최종 점수를 계산합니다.
        /// </summary>
        public int calculateScore(List<cell> cell, float multiplier)
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
            return gameData.isGameOverValue || getRemainingTime() <= 0;
        }


        // =========================================================================
        // [박재우 조원 분담 범위] 아래는 게임 오버 시 사과 조합을 탐색하는 영역입니다.
        // =========================================================================

        /// <summary>
        /// [박재우 담당] 게임 오버 후 보드에 남아있는 모든 직사각형 범위를 순회하며 합이 10이 되는 사과 조합을 탐색합니다.
        /// </summary>
        public List<availableApple> findMissedApple()
        {
            List<availableApple> result = new List<availableApple>();

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
                                List<cell> cells = getCellsInRect(r1, c1, r2, c2);
                                availableApple comboApple = new availableApple(cells);

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
                // 교수님 지침서 요구사항: 의미 있는 예외 처리 구현
                Console.WriteLine($"[findMissedApple 런타임 오류 발생] {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// [박재우 담당 헬퍼] 지정된 두 좌표(r1,c1 ~ r2,c2)로 형성되는 직사각형 내부의 유효한 셀 목록을 추출합니다.
        /// </summary>
        private List<cell> getCellsInRect(int r1, int c1, int r2, int c2)
        {
            List<cell> cells = new List<cell>();

            for (int r = r1; r <= r2; r++)
            {
                for (int c = c1; c <= c2; c++)
                {
                    position pos = new position(c, r);
                    cell cell = board.getCell(pos);

                    if (cell != null && cell.hasApple())
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
