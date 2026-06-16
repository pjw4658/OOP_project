using OOP_project.Source.Models;
using OOP_project.Source.Models;
using System;

namespace OOP_project.Source.Logic
{
    /// <summary>
    /// [최창무 담당] 점수, 시간, 콤보 등 현재 게임의 상태 데이터를 독립적으로 관리하는 클래스입니다.
    /// 조장님 지침에 따라 데이터 타입(클래스)은 대문자, 필드 및 메서드는 소문자 시작 카멜케이스로 조율되었습니다.
    /// </summary>
    public class GameData
    {
        // 멤버 변수 (소문자 시작 camelCase)
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

        public void resetData()
        {
            score = 0;
            currentTime = timeLimit;
            isGameOver = false;
            combo.resetCombo(); // Combo 클래스 메서드 양식 동기화
        }

        public void addScore(int point)
        {
            // 상운님 GameLogic에서 이미 콤보 배율 연산이 끝난 점수가 넘어오므로, 
            // 중복 합산 방지를 위해 순수 누적 연산만 수행하도록 안전 조치 완료
            score += point;
        }

        public void successMatch(int basePoint)
        {
            combo.increaseCombo();
            addScore(basePoint);
        }

        public void failMatch()
        {
            combo.resetCombo();
        }

        public void decreaseTime()
        {
            currentTime--;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isGameOver = true;
            }
        }

        // 외부 로직(GameLogic 등)과의 유기적 통신을 위한 데이터 개방 인터페이스 메서드 추가
        public Combo getCombo() => combo;
        public bool checkGameOver() => isGameOver;
        public void setGameOver(bool gameOver) => isGameOver = gameOver;
        public int getScore() => score;
        public int getCurrentTime() => currentTime;
        public void setCurrentTime(int time) => currentTime = time;
        public int getTimeLimit() => timeLimit;
        public int getComboCount() => combo.getComboCount();
        public int getMaxCombo() => combo.getMaxCombo();
        public int getComboBonus() => combo.getComboBonus();
    }
}