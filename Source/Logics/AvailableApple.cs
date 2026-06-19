using System;
using System.Collections.Generic;

namespace OOP_project.Source.Models
{
    /// <summary>
    /// [박재우 담당] 합이 10이 되는 유효한 사과 조합을 관리하는 클래스입니다.
    /// </summary>
    public class AvailableApple
    {
        private List<Cell> availableCells;

        public AvailableApple(List<Cell> cells)
        {
            this.availableCells = cells;
        }

        public List<Cell> getCells() => availableCells;

        public bool isValid()
        {
            if (availableCells == null || availableCells.Count == 0) return false;

            int sum = 0;
            bool hasJoker = false;

            foreach (Cell cell in availableCells)
            {
                if (cell.apple.isJoker())
                {
                    hasJoker = true;
                }
                else
                {
                    sum += cell.apple.getValue();
                }
            }

            return hasJoker || sum == 10;
        }

        public void show()
        {
            // 힌트 제공 시 화면에 하이라이트 표시 이펙트를 넣는 트리거 영역
            // 지금은 콘솔 검증용이므로 간단한 로그 출력이나 비워두셔도 무방합니다.
        }
    }
}