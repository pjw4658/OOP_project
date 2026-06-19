using System.Collections.Generic;
using OOP_project.Source.Ranking;

namespace OOP_project.Source.Interfaces
{
    /// <summary>
    /// 랭킹 시스템의 외부 통신 규격을 정의하는 인터페이스입니다.
    /// </summary>
    public interface IRankingService
    {
        void addEntry(string playerName, int score);          // 새로운 랭킹 기록 추가
        List<RankingEntry> getTopRankings(int limit);         // 상위 N개의 랭킹 리스트 가져오기
        void saveRankings();                                  // 랭킹 데이터를 로컬 파일에 저장
        void loadRankings();                                  // 로컬 파일에서 랭킹 데이터 불러오기
    }
}
