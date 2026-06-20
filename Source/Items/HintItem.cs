using OOP_project.Source.Logic;
using OOP_project.Source.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_project.Source.Items
{
    public class HintItem : SingleUseItem
    {
        // ─────────────────────────────────────────────
        // 멤버 변수
        // ─────────────────────────────────────────────
        
        /// 합이 10이 되는 사과 그룹들을 캐싱해두는 변수입니다.
        /// Execute 호출 시 한 번만 계산하여 여기에 저장합니다.
        private List<AvailableApple> _cachedHints;

        /// 한 번에 화면에 표시할 최대 힌트 조합 개수를 제한합니다.
        private int _maxHintGroups;


        // ─────────────────────────────────────────────
        // 속성
        // ─────────────────────────────────────────────
        
        /// UI나 GameLogic이 이미 계산된 힌트 결과를 참조할 때 사용합니다.
        public List<AvailableApple> CachedHints => _cachedHints;


        // ─────────────────────────────────────────────
        // 생성자
        // ─────────────────────────────────────────────
        
        /// 아이템 ID와 이름을 초기화하고, 캐시 리스트의 메모리 공간을 미리 할당합니다.
        public HintItem() : base("Hint_01", "힌트 보기")
        {
            _cachedHints = new List<AvailableApple>();
            _maxHintGroups = 3;
        }

        public HintItem(string itemId) : base(itemId, "힌트 보기")
        {
            _cachedHints = new List<AvailableApple>();
            _maxHintGroups = 3;
        }


        // ─────────────────────────────────────────────
        // 메서드
        // ─────────────────────────────────────────────
        
        /// GameLogic 에서 합이 10이 되는 사과 그룹들을 받아와,
        /// 조합이 하나라도 있으면 하이라이트를 수행하고 사용 처리합니다.
        /// <returns>힌트 조합이 있어 성공적으로 발동하면 true, 그렇지 않으면 false</returns>
        public override bool Execute(GameLogic gameLogic)
        {
            // 사용 가능 여부 체크 (SingleUseItem.IsAvailable: 횟수 > 0 && 미사용)
            if (!IsAvailable()) return false;
            
            List<AvailableApple> groups = gameLogic.findMissedApple();

            // 힌트로 보여줄 조합이 없으면 발동하지 않음 (아이템 소모 방지)
            if (groups == null || groups.Count == 0)
                return false;

            _cachedHints = groups;                  // 결과 캐싱
            HighlightGroups(_cachedHints);          // 시각 효과 트리거
            OnUsed();                               // _hasBeenUsed = true, 사용 횟수 차감
            return true;
        }

        /// 이 아이템의 종류가 ItemType.Hint임을 반환합니다.
        /// UI에서 아이콘을 결정할 때 참조합니다.
        public override ItemType GetItemType()
        {
            return ItemType.Hint;
        }


        /// 찾아낸 AvailableApple 그룹들에 시각적 하이라이트를 트리거하는 메서드입니다.
        /// 각 AvailableApple의 Show()를 호출하면 내부의 OnShowHighlight 이벤트가 발생하고,
        /// 이를 구독한 UI 컴포넌트가 실제 이펙트(깜빡임, 노란 테두리 등)를 렌더링합니다.
        public void HighlightGroups(List<AvailableApple> groups)
        {
            // 실제 보여줄 개수 = 탐색된 그룹 수와 _maxHintGroups 중 작은 값
            int showCount = Math.Min(groups.Count, _maxHintGroups);

            for (int i = 0; i < showCount; i++)
            {
                // Show()가 OnShowHighlight 이벤트를 발생시켜
                // 구독자(UI/렌더러)에게 해당 그룹의 List<Cell>을 전달합니다.
                groups[i].show();
            }
        }
    }
}