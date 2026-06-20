using OOP_project.Source.Logic;
using OOP_project.Source.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_project.Source.Items
{
    public class JokerItem : TargetedItem
    {
        // ─────────────────────────────────────────────
        // 생성자 
        // ─────────────────────────────────────────────
        
        /// ID, 이름, 사용 횟수(1회)를 부모(TargetedItem → Item) 생성자에 전달하며 초기화합니다.
        public JokerItem() : base("Joker_01", "조커 아이템", 1)
        {
        }

        public JokerItem(string itemId) : base(itemId, "조커 아이템", 1)
        {
        }

        // ─────────────────────────────────────────────
        // 메서드
        // ─────────────────────────────────────────────
        
        /// 대상 칸에 실제로 사과가 있는지 검증한 뒤 JokerApple 교체 작업을 수행합니다.
        /// <returns>교체 성공 시 true, 사용 불가 상태이거나 칸에 사과가 없으면 false</returns>
        public override bool Execute(GameLogic gameLogic, Cell targetCell)
        {
            // 사용 가능 여부 체크 (Item.IsAvailable: 남은 횟수 > 0)
            if (!IsAvailable()) return false;

            // 타겟이 유효하고, 해당 칸에 사과가 실제로 존재하는지 검증
            if (targetCell == null || targetCell.apple == null)
                return false;

            ConvertToJoker(targetCell); // 사과 → JokerApple 교체
            OnUsed();                   // 사용 횟수 차감
            return true;
        }

        /// 이 아이템의 종류가 ItemType.Joker임을 반환합니다.
        /// UI에서 아이콘을 결정할 때 참조합니다.
        public override ItemType GetItemType()
        {
            return ItemType.Joker;
        }

        
        /// 기존 사과를 제거한 자리에 새로운 JokerApple 객체를 끼워 넣습니다.
        private void ConvertToJoker(Cell targetCell)
        {
            // Step 1. 기존 사과를 칸에서 제거
            targetCell.removeApple();

            // Step 2. 빈 자리에 JokerApple 객체를 새로 생성하여 배치
            targetCell.apple = (new JokerApple());
        }
    }
}