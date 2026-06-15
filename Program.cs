using System;
using System.Collections.Generic;
using System.Threading; // 시간 시뮬레이션을 위한 기능
using OOP_project.Source.Models;
using OOP_project.Source.Logics;

namespace OOP_project
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 사과 게임 로직 및 시간 시스템 정밀 검증 ===");

            // 1. 5행 5열의 보드 생성
            Board board = new Board(5, 5);
            
            // 2. 상운님의 GameLogic 객체 생성 (제한 시간 60초 설정)
            GameLogic game = new GameLogic(board, 60);
            game.StartGame();

            // 초기 보드 상태 출력
            board.DisplayBoardConsole();
            Console.WriteLine($"[게임 시작] 현재 점수: {game.GetScore()}점 | 남은 시간: {game.GetRemainingTime()}초");

            // 3. 의도적인 합 계산 테스트 (강제로 특정 셀을 꺼내어 검증)
            Console.WriteLine("\n[테스트 1] 임의의 칸 두 개를 드래그 선택했다고 가정하고 CheckSum을 돌립니다.");
            List<Cell> userSelection = new List<Cell>();
            
            // 안전하게 보드 내부의 (0,0) 칸과 (0,1) 칸을 리스트에 담음
            userSelection.Add(board.GetCell(new Position(0, 0)));
            userSelection.Add(board.GetCell(new Position(1, 0)));

            // 두 사과의 값을 확인해봅니다.
            int val1 = userSelection[0].apple.IsJoker ? 0 : userSelection[0].apple.Value;
            int val2 = userSelection[1].apple.IsJoker ? 0 : userSelection[1].apple.Value;
            Console.WriteLine($"선택한 사과 수치: {val1}와 {val2} (합: {val1 + val2})");

            // 상운님의 핵심 메서드 CheckSum 호출
            bool result = game.CheckSum(userSelection);
            if (result)
            {
                Console.WriteLine("-> 성공! 합이 10이거나 조커가 있어 사과가 터지고 점수가 올랐습니다.");
            }
            else
            {
                Console.WriteLine("-> 실패! 합이 10이 되지 않아 점수가 변하지 않았습니다.");
            }

            // 반영 후 실시간 상태 출력
            board.DisplayBoardConsole();
            Console.WriteLine($"[결과] 현재 점수: {game.GetScore()}점 | 남은 시간: {game.GetRemainingTime()}초");

            // 4. 시간 흐름 시뮬레이션 (3초 대기 후 남은 시간 변화 체크)
            Console.WriteLine("\n[테스트 2] 3초간 플레이어가 고민 중인 상황 시뮬레이션...");
            Thread.Sleep(3000); // 3초 대기

            Console.WriteLine($"[실시간 시간 통신 체크] 3초 후 남은 시간: {game.GetRemainingTime()}초");
        }
    }
}
