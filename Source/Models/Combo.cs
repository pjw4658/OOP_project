namespace OOP_project.Source.Models
{
    public class Combo
    {
        private int comboCount;
        private int maxCombo;
        private float comboTimer;      // 콤보 유지 남은 시간
        private float comboTimeLimit;  // 콤보 유지 제한 시간

        public Combo()
        {
            comboCount = 0;
            maxCombo = 0;
            comboTimer = 0f;
            comboTimeLimit = 5f; // 5초 유지
        }

        public void increaseCombo()
        {
            comboCount++;
            comboTimer = comboTimeLimit; // 콤보 성공 시 타이머 리셋
            if (comboCount > maxCombo)
            {
                maxCombo = comboCount;
            }
        }

        public void resetCombo()
        {
            comboCount = 0;
            comboTimer = 0f;
        }

        // 매 틱마다 호출 (deltaTime: 경과 시간(초))
        public void updateTimer(float deltaTime)
        {
            if (comboCount == 0) return;

            comboTimer -= deltaTime;
            if (comboTimer <= 0f)
            {
                resetCombo();
            }
        }

        public float getComboTimer() => comboTimer;
        public float getComboTimeLimit() => comboTimeLimit;
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