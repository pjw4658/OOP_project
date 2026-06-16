namespace OOP_project.Source.Models
{
    /// <summary>
    /// 와일드카드 역할을 하는 특수 조커 사과 클래스입니다.
    /// </summary>
    public class JokerApple : Apple
    {
        public JokerApple()
        {
            this.value = 0; // 조커는 고정된 값이 없으므로 0으로 초기화
            this.isRemoved = false;
        }

        public override int getValue() => 0;
        public override bool isJoker() => true; // 조커사과이므로 true
        public override void remove() => isRemoved = true;
    }
}