using System;
using System.Collections.Generic;
using System.IO;
using OOP_project.Source.Interfaces;

namespace OOP_project.Source.Ranking
{
    /// <summary>
    /// IRankingService를 상속받아 랭킹 데이터 정렬 및 파일 입출력을 처리하는 실질적인 매니저 클래스입니다.
    /// </summary>
    public class RankingManager : IRankingService
    {
        private List<RankingEntry> rankingList; // 컬렉션 활용
        private string filePath;

        public RankingManager()
        {
            this.rankingList = new List<RankingEntry>();
            // 실행 파일 디렉토리에 'ranking.txt'로 자동 저장 및 로드되도록 경로 세팅
            this.filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ranking.txt");
            loadRankings(); // 객체 생성 시 파일에서 기존 데이터를 자동으로 읽어옴
        }

        /// <summary>
        /// 새로운 유저 점수를 등록하고 자동으로 내림차순 정렬한 뒤 영구 저장합니다.
        /// </summary>
        public void addEntry(string playerName, int score)
        {
            RankingEntry entry = new RankingEntry(playerName, score, DateTime.Now);
            rankingList.Add(entry);
            
            //람다식을 사용한 점수 내림차순(높은 순) 정렬 기능 적용
            rankingList.Sort((x, y) => y.getScore().CompareTo(x.getScore()));
            
            saveRankings(); // 실시간 저장 트리거
        }

        /// <summary>
        /// 지정한 개수(limit) 만큼의 상위 등수 리스트를 반환합니다. (예: Top 10 추출)
        /// </summary>
        public List<RankingEntry> getTopRankings(int limit)
        {
            List<RankingEntry> topList = new List<RankingEntry>();
            int count = Math.Min(limit, rankingList.Count);
            
            for (int i = 0; i < count; i++)
            {
                topList.Add(rankingList[i]);
            }
            
            return topList;
        }

        /// <summary>
        ///  의미 있는 try-catch-finally를 활용한 텍스트 파일 저장 로직입니다.
        /// </summary>
        public void saveRankings()
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filePath, false); // 덮어쓰기 모드
                foreach (RankingEntry entry in rankingList)
                {
                    // 콤마(,)를 구분자로 규격 저장
                    writer.WriteLine($"{entry.getPlayerName()},{entry.getScore()},{entry.getAchievedTime():yyyy-MM-dd HH:mm:ss}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[RankingManager 데이터 파일 저장 런타임 오류] {e.Message}");
            }
            finally
            {
                // 자원 해제를 보장하는 파이널리 블록 구현
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        ///프로그램 가동 시 이전 세션의 파일 데이터를 파싱하여 리스트에 복구합니다.
        /// </summary>
        public void loadRankings()
        {
            if (!File.Exists(filePath)) return;

            rankingList.Clear();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        string name = parts[0];
                        int score = int.Parse(parts[1]);
                        DateTime time = DateTime.Parse(parts[2]);
                        rankingList.Add(new RankingEntry(name, score, time));
                    }
                }
                
                // 데이터 무결성을 위해 로드 후 다시 한 번 람다 정렬 보장
                rankingList.Sort((x, y) => y.getScore().CompareTo(x.getScore()));
            }
            catch (Exception e)
            {
                Console.WriteLine($"[RankingManager 데이터 파일 로드 런타임 오류] {e.Message}");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
