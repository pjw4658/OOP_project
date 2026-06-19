using System;
using System.Collections.Generic;
using System.Text;

namespace project_cs.Source.Interfaces
{
    // 화면에 자신을 그릴 수 있는 객체가 반드시 구현해야 하는 렌더링 계약입니다.
    // 보드, 사과, 이펙트 등 시각적 요소를 가진 모든 클래스에 적용됩니다.
    public interface IRenderable
    {
        // 객체의 비주얼(보드, 사과, 이펙트 등)을 화면에 그립니다.
        void Render();

    }
}
