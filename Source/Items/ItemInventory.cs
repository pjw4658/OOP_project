using OOP_project.Source.Items;
using OOP_project.Source.Logic;
using OOP_project.Source.Models;
using project_cs.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_cs.Source.Items
{
    // 플레이어가 보유한 아이템들을 저장하고, 관리하며,
    // 게임 상황에 맞춰 꺼내어 실행시키는 컨테이너(가방) 겸 아이템 매니저 클래스입니다.
    // 판이 바뀌거나 게임이 재시작될 때 초기화가 필요하므로 IResettable을 구현합니다.
    public class ItemInventory : IResettable
    {
        // ─────────────────────────────────────────────
        // 멤버 변수
        // ─────────────────────────────────────────────

        /// 아이템 ID를 키(Key)로, 아이템 객체를 값(Value)으로 저장하는 딕셔너리입니다.
        /// ID 기반으로 O(1) 시간에 아이템을 찾을 수 있습니다.
        private Dictionary<string, Item> _items;

        /// 아이템이 인벤토리에 추가된 순서를 기억하는 리스트입니다.
        /// 딕셔너리는 순서를 보장하지 않으므로, UI 표시 순서 유지를 위해 병행 관리합니다.
        private List<string> _itemOrder;


        // ─────────────────────────────────────────────
        // 속성
        // ─────────────────────────────────────────────

        /// 현재 인벤토리에 들어있는 아이템의 총 개수를 반환합니다.
        public int Count => _items.Count;

        /// [인덱서] inventory["ID"] 형태로 아이템에 직접 접근할 수 있게 해줍니다.
        /// 존재하지 않는 ID를 조회하면 null을 반환합니다.
        /// 안전한 접근이 필요하다면 TryGetItem()을 사용하세요.
        public Item this[string itemId] =>
            _items.ContainsKey(itemId) ? _items[itemId] : null;

        // ─────────────────────────────────────────────
        // 생성자
        // ─────────────────────────────────────────────

        /// ItemInventory 기본 생성자입니다.
        /// 인벤토리 생성 시 내부 딕셔너리와 순서 리스트를 초기화합니다.
        public ItemInventory()
        {
            _items = new Dictionary<string, Item>();
            _itemOrder = new List<string>();
        }

        // ─────────────────────────────────────────────
        // 메서드
        // ─────────────────────────────────────────────

        /// 새로운 아이템을 인벤토리에 추가합니다.
        /// 동일한 ID의 아이템이 이미 있으면 중복 추가를 막습니다.
        public void AddItem(Item item)
        {
            if (item == null) return;
            if (_items.ContainsKey(item.ItemId)) return; // 중복 방지

            _items[item.ItemId] = item;
            _itemOrder.Add(item.ItemId);
        }

        /// 타겟 지정이 필요 없는 아이템(ShuffleItem, HintItem)을 실행합니다.
        /// 보드판 전체에 영향을 줄 때 호출합니다.
        /// <returns>아이템 사용 성공 시 true, 아이템이 없거나 실행 실패 시 false</returns>
        public bool UseItem(string itemId, GameLogic gameLogic)
        {
            if (!TryGetItem(itemId, out Item item)) return false;

            return item.Execute(gameLogic);
        }

        /// 특정 칸을 찍어서 쓰는 아이템(JokerItem)을 실행합니다.
        /// 플레이어가 클릭한 칸 정보를 targetCell로 함께 전달합니다.
        /// <returns>아이템 사용 성공 시 true, 아이템이 없거나 TargetedItem이 아니거나 실행 실패 시 false</returns>
        public bool UseItem(string itemId, Board board, Cell targetCell)
        {
            if (!TryGetItem(itemId, out Item item)) return false;

            // TargetedItem인지 확인 후 타겟 전용 Execute 호출
            if (item is TargetedItem targetedItem)
            {
                return targetedItem.Execute(board, targetCell);
            }

            // TargetedItem이 아닌 아이템에 타겟을 넘기려 했으므로 실패 처리
            return false;
        }


        /// 현재 사용 가능한(RemainingUses > 0) 아이템들만 골라 리스트로 반환합니다.
        /// UI에서 "사용 가능한 아이템 버튼"만 활성화할 때 유용합니다.
        /// 인벤토리에 추가된 순서를 유지하여 반환합니다.
        /// <returns>사용 가능한 아이템들의 리스트</returns>
        public List<Item> GetAvailableItems()
        {
            List<Item> availableItems = new List<Item>();

            foreach (string id in _itemOrder)
            {
                if (TryGetItem(id, out Item item) && item.IsAvailable())
                {
                    availableItems.Add(item);
                }
            }

            return availableItems;
        }

        /// 아이템 ID로 딕셔너리에서 아이템을 안전하게 찾습니다.
        /// 있으면 true와 함께 item을 반환하고, 없으면 false와 null을 반환합니다.
        /// 직접 인덱서([])를 쓰는 것보다 런타임 에러 위험이 없으므로 권장되는 접근 방식입니다.
        /// <returns>아이템이 존재하면 true, 없으면 false</returns>
        public bool TryGetItem(string itemId, out Item item)
        {
            return _items.TryGetValue(itemId, out item);
        }

        // ─────────────────────────────────────────────
        // IResettable 구현
        // ─────────────────────────────────────────────

        /// 인벤토리를 완전히 비워 게임 시작 / 판 전환 초기 상태로 리셋합니다.
        /// 보유 아이템 목록과 순서 정보가 모두 제거됩니다.
        public void Reset()
        {
            _items.Clear();
            _itemOrder.Clear();
        }

        /// 인벤토리가 정상 작동할 준비가 되었는지 확인합니다.
        /// 내부 컬렉션이 null 없이 초기화된 상태이면 true를 반환합니다.
        public bool IsReady()
        {
            return _items != null && _itemOrder != null;
        }
    }
}
