namespace OOP_project.Source.Models
{
    /// <summary>
    /// [최창무 담당] 연속 성공 콤보 횟수와 점수 보너스 가산을 제어하는 클래스입니다.
    /// </summary>
    public class Combo
    {
        private int comboCount;
        private int maxCombo;

        public Combo()
        {
            comboCount = 0;
            maxCombo = 0;
        }

        public void increaseCombo()
        {
            comboCount++;
            if (comboCount > maxCombo)
            {
                maxCombo = comboCount;
            }
        }

        public void resetCombo()
        {
            comboCount = 0;
        }

        public int getComboCount() => comboCount;
        public int getMaxCombo() => maxCombo;

        public int getComboBonus()
        {
            if (comboCount >= 5) return 30;
            if (comboCount >= 3) return 20;
            if (comboCount >= 2) return 10;
            return 0;
        }
    }
}