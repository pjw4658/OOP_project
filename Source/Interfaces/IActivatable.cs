using System;
using System.Collections.Generic;
using System.Text;

namespace project_cs.Source.Interfaces
{
    // 활성화/비활성화 상태를 가지는 객체가 반드시 구현해야 하는 계약입니다.
    // 버튼, 아이템 슬롯, 게임 오브젝트 등 활성 상태가 변하는 모든 클래스에 적용됩니다.
    
    public interface IActivatable
    {
        // 현재 활성화(사용 가능) 상태인지 여부를 나타내는 프로퍼티입니다.
        bool IsActive { get; }

        // 대상을 활성화 상태로 전환합니다.
        void Activate();

        // 대상을 비활성화(잠금/쿨타임) 상태로 전환합니다.
        void Deactivate();
    }
}
