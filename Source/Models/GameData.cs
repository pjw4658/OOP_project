namespace OOP_PROJECT.Source.Models;

public class GameData
{
    private Combo combo;
    private int score;
    private int timeLimit;
    private int currentTime;
    private bool isGameOver;

    public GameData()
    {
        combo = new Combo();
        score = 0;
        timeLimit = 60;
        currentTime = timeLimit;
        isGameOver = false;
    }

    public void ResetData()
    {
        score = 0;
        currentTime = timeLimit;
        isGameOver = false;
        combo.ResetCombo();
    }

    public void AddScore(int point)
    {
        int bonus = combo.GetComboBonus();
        score += point + bonus;
    }

    public void SuccessMatch(int basePoint)
    {
        combo.IncreaseCombo();
        AddScore(basePoint);
    }

    public void FailMatch()
    {
        combo.ResetCombo();
    }

    public void DecreaseTime()
    {
        currentTime--;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isGameOver = true;
        }
    }

    public bool CheckGameOver()
    {
        return isGameOver;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetCurrentTime()
    {
        return currentTime;
    }

    public int GetTimeLimit()
    {
        return timeLimit;
    }

    public int GetComboCount()
    {
        return combo.GetComboCount();
    }

    public int GetMaxCombo()
    {
        return combo.GetMaxCombo();
    }

    public int GetComboBonus()
    {
        return combo.GetComboBonus();
    }
}