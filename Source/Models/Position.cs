namespace OOP_project.Source.Models
{
    /// <summary>
    /// X, Y 좌표를 관리하는 경량 데이터 구조체(struct)입니다.
    /// </summary>
    public struct Position
    {
        private int x;
        private int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX() => x;
        public int getY() => y;
    }
}