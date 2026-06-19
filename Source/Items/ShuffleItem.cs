using OOP_project.Source.Logic;
using OOP_project.Source.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_project.Source.Items
{
    public class ShuffleItem : SingleUseItem
    {
        // ─────────────────────────────────────────────
        // 멤버 변수
        // ─────────────────────────────────────────────

        /// 사과를 섞을 때 사용할 난수 생성용 시드(Seed) 값입니다.
        private int _shuffleSeed;

        // ─────────────────────────────────────────────
        // 생성자
        // ─────────────────────────────────────────────

        /// 부모(SingleUseItem) 생성자를 통해 ID와 이름을 초기화하고,
        /// 매 생성마다 다른 셔플 결과를 내도록 시드를 랜덤 설정합니다.
        public ShuffleItem() : base("Shuffle_01", "셔플 아이템")
        {
            _shuffleSeed = Environment.TickCount; 
        }

        // ─────────────────────────────────────────────
        // 메서드
        // ─────────────────────────────────────────────
        
        /// <param name="board">섞기를 적용할 보드 객체</param>
        /// <returns>성공 시 true, 이미 사용했거나 사용 불가 상태이면 false</returns>
        public override bool Execute(GameLogic gameLogic)
        {
            // 사용 가능 여부 체크 (SingleUseItem.IsAvailable: 횟수 > 0 && 미사용)
            if (!IsAvailable()) return false;

            PerformShuffle(gameLogic.Board);  // 핵심 셔플 알고리즘 수행
            OnUsed();               // _hasBeenUsed = true, 사용 횟수 차감
            return true;
        }

        /// 이 아이템의 종류가 ItemType.Shuffle임을 반환합니다.
        /// UI에서 아이콘을 결정할 때 참조합니다.
        public override ItemType GetItemType()
        {
            return ItemType.Shuffle;
        }

        /// 보드를 순회하여 사과가 존재하는 셀만 추려낸 뒤,
        /// Fisher-Yates 셔플로 사과들의 위치를 무작위로 교환하고 화면을 갱신합니다.
        /// <param name="board">섞기를 적용할 보드 객체</param>
        private void PerformShuffle(Board board)
        {
            // Step 1. 사과가 있는 셀만 추출 
            List<Cell> occupiedCells = new List<Cell>();

            for (int row = 0; row < board.rows; row++)
            {
                for (int col = 0; col < board.cols; col++)
                {
                    Position position = new Position(col, row); // x=열(col), y=행(row)
                    Cell cell = board.getCell(position);
                    if (cell != null && cell.apple != null)
                        occupiedCells.Add(cell);
                }
            }

            // 섞을 사과가 2개 미만이면 의미 없으므로 조기 종료
            if (occupiedCells.Count < 2) return;

            // Step 2. 각 셀에서 사과 객체만 별도 추출 
            List<Apple> apples = new List<Apple>();
            foreach (Cell cell in occupiedCells)
                apples.Add(cell.apple);

            // Step 3. Fisher-Yates 알고리즘으로 무작위 섞기 
            // 동일한 시드면 동일한 결과가 재현되므로, 리플레이/디버깅에 활용 가능합니다.
            Random random = new Random(_shuffleSeed);
            for (int i = apples.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (apples[i], apples[j]) = (apples[j], apples[i]);
            }

            // Step 4. 섞인 사과를 원래 셀 위치에 재배치
            // Cell에 사과를 심는 책임을 ShuffleItem이 직접 담당합니다.
            for (int i = 0; i < occupiedCells.Count; i++)
                SetApple(occupiedCells[i], apples[i]);

            // ── Step 5. 변경 사항을 화면에 갱신 
            // 해당 부분은 UI 연결 후 보드의 화면 갱신 메서드를 호출하도록 수정 필요 
            // board.Redraw();
        }

        /// 지정한 셀에 사과 객체를 배치하는 메서드입니다. 
        private void SetApple(Cell cell, Apple apple)
        {
            cell.apple = apple;
        }
    
    }
}