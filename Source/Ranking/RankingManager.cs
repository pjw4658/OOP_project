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
        private List<RankingEntry> rankingList;
        private string filePath;

        public RankingManager()
        {
            this.rankingList = new List<RankingEntry>();
            this.filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ranking.txt");
            loadRankings();
        }

        /// <summary>
        /// 새로운 점수를 보드 크기 정보와 함께 등록하고 내림차순 정렬 후 저장합니다.
        /// </summary>
        public void addEntry(string playerName, int score, string boardSize)
        {
            RankingEntry entry = new RankingEntry(playerName, score, DateTime.Now, boardSize);
            rankingList.Add(entry);
            rankingList.Sort((x, y) => y.getScore().CompareTo(x.getScore()));
            saveRankings();
        }

        /// <summary>
        /// 특정 보드 크기의 상위 N개 랭킹을 반환합니다.
        /// </summary>
        public List<RankingEntry> getTopRankings(int limit, string boardSize)
        {
            List<RankingEntry> filtered = new List<RankingEntry>();
            foreach (RankingEntry entry in rankingList)
            {
                if (entry.getBoardSize() == boardSize)
                    filtered.Add(entry);
            }

            List<RankingEntry> topList = new List<RankingEntry>();
            int count = Math.Min(limit, filtered.Count);
            for (int i = 0; i < count; i++)
                topList.Add(filtered[i]);

            return topList;
        }

        /// <summary>
        /// 랭킹 데이터를 파일에 저장합니다. 파일 형식: 이름,점수,날짜,보드크기
        /// </summary>
        public void saveRankings()
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filePath, false);
                foreach (RankingEntry entry in rankingList)
                {
                    writer.WriteLine($"{entry.getPlayerName()},{entry.getScore()},{entry.getAchievedTime():yyyy-MM-dd HH:mm:ss},{entry.getBoardSize()}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[RankingManager 저장 오류] {e.Message}");
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// 파일에서 랭킹 데이터를 불러옵니다. boardSize가 없는 구 형식 항목은 "중형"으로 처리합니다.
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
                    if (parts.Length >= 3)
                    {
                        string name = parts[0];
                        int score = int.Parse(parts[1]);
                        DateTime time = DateTime.Parse(parts[2]);
                        string boardSize = parts.Length >= 4 ? parts[3] : "중형";
                        rankingList.Add(new RankingEntry(name, score, time, boardSize));
                    }
                }
                rankingList.Sort((x, y) => y.getScore().CompareTo(x.getScore()));
            }
            catch (Exception e)
            {
                Console.WriteLine($"[RankingManager 로드 오류] {e.Message}");
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
