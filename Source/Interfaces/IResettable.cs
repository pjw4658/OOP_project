using System;
using System.Collections.Generic;
using System.Text;

namespace project_cs.Source.Interfaces
{
    // 상태를 초기화하고 준비 여부를 확인할 수 있는 객체가 반드시 구현해야 하는 계약입니다.
    // 게임 시작 또는 판 전환 시 상태를 되돌려야 하는 모든 클래스에 적용됩니다.
    public interface IResettable
    {
        // 객체의 상태를 게임 시작 또는 판 전환 초기 상태로 완전히 리셋합니다.
        void Reset();

        // 객체가 초기화를 마치고 정상 작동할 수 있는 준비가 되었는지 여부를 반환합니다.
        // <returns>준비 완료 상태이면 true, 아직 초기화 중이면 false</returns>
        bool IsReady();
    }
}
