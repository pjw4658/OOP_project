using System;

namespace OOP_project.Source.Ranking
{
    /// <summary>
    /// 개별 플레이어의 점수 기록 한 줄을 나타내는 데이터 모델 클래스입니다.
    /// </summary>
    public class RankingEntry
    {
        private string playerName;
        private int score;
        private DateTime achievedTime;
        private string boardSize;

        public RankingEntry(string playerName, int score, DateTime achievedTime, string boardSize = "중형")
        {
            this.playerName = playerName;
            this.score = score;
            this.achievedTime = achievedTime;
            this.boardSize = boardSize;
        }

        public string getPlayerName() => playerName;
        public int getScore() => score;
        public DateTime getAchievedTime() => achievedTime;
        public string getBoardSize() => boardSize;
    }
}
