using System.Collections.Generic;
using OOP_project.Source.Ranking;

namespace OOP_project.Source.Interfaces
{
    /// <summary>
    /// 랭킹 시스템의 외부 통신 규격을 정의하는 인터페이스입니다.
    /// </summary>
    public interface IRankingService
    {
        void addEntry(string playerName, int score, string boardSize);
        List<RankingEntry> getTopRankings(int limit, string boardSize);
        void saveRankings();
        void loadRankings();
    }
}
