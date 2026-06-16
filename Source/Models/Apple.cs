using System;

namespace OOP_project.Source.Models
{
    /// <summary>
    /// 모든 사과의 최상위 추상 부모 클래스입니다.
    /// 조장님 지침에 따라 클래스는 대문자, 변수 및 메서드는 소문자 시작 카멜케이스를 따릅니다.
    /// </summary>
    public abstract class Apple
    {
        protected int value;        // 사과의 숫자 값
        protected bool isRemoved;   // 삭제 유무 플래그

        public abstract int getValue();
        public abstract bool isJoker();
        public abstract void remove();
    }
}