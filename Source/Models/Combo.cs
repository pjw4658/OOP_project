namespace OOP_PROJECT.Source.Models;

public class Combo
{
    private int comboCount;
    private int maxCombo;

    public Combo()
    {
        comboCount = 0;
        maxCombo = 0;
    }

    public void IncreaseCombo()
    {
        comboCount++;

        if (comboCount > maxCombo)
        {
            maxCombo = comboCount;
        }
    }

    public void ResetCombo()
    {
        comboCount = 0;
    }

    public int GetComboCount()
    {
        return comboCount;
    }

    public int GetMaxCombo()
    {
        return maxCombo;
    }

    public int GetComboBonus()
    {
        if (comboCount >= 5)
        {
            return 30;
        }
        else if (comboCount >= 3)
        {
            return 20;
        }
        else if (comboCount >= 2)
        {
            return 10;
        }

        return 0;
    }
}