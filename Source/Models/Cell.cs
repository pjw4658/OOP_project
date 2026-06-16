using System;

namespace OOP_project.Source.Models
{
    /// <summary>
    /// 보드의 한 칸을 나타내며 좌표와 사과 객체를 관리하는 클래스입니다.
    /// </summary>
    public class Cell
    {
        private Position position;
        private Apple currentApple;
        private bool isSelected;

        // GameLogic에서 cell.apple 형태로 사과에 접근하므로 속성(Property)을 개방합니다.
        // 속성 명칭 또한 소문자 시작 규칙을 적용합니다.
        public Apple apple
        {
            get => currentApple;
            set => currentApple = value;
        }

        public Cell(Position position)
        {
            this.position = position;
            this.currentApple = null;
            this.isSelected = false;
        }

        public bool hasApple() => currentApple != null;

        public bool removeApple()
        {
            if (currentApple != null)
            {
                currentApple.remove();
                currentApple = null;
                return true;
            }
            return false;
        }

        public void select() => isSelected = true;
        public void deselect() => isSelected = false;
        public Position getPosition() => position;
    }
}