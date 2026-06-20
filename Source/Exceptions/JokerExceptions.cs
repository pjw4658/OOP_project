using System;

namespace OOP_project.Source.Exceptions
{
    /// <summary>
    /// 드래그 선택 시 조커 관련 규칙 위반 (케이스 1)
    /// - 조커 사과만 선택한 경우
    /// - 조커 제외 숫자 합이 10 초과인 경우
    /// </summary>
    public class JokerSelectionException : Exception
    {
        public JokerSelectionException(string message) : base(message) { }
    }

    /// <summary>
    /// 조커 아이템 사용 시 잘못된 대상 지정 (케이스 2, 3)
    /// - 빈 칸이나 보드 밖을 클릭한 경우
    /// - 이미 조커 사과인 칸을 클릭한 경우
    /// </summary>
    public class JokerTargetException : Exception
    {
        public JokerTargetException(string message) : base(message) { }
    }
}
