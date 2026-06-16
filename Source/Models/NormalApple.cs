namespace OOP_project.Source.Models
{
    /// <summary>
    /// 일반 숫자 사과 클래스입니다.
    /// </summary>
    public class NormalApple : Apple
    {
        public NormalApple(int value)
        {
            this.value = value;
            this.isRemoved = false;
        }

        public override int getValue() => value;
        public override bool isJoker() => false; // 일반 사과이므로 false
        public override void remove() => isRemoved = true;
    }
}