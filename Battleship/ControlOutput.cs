using System;
using BusinessLayer.Extensions;
using BusinessLayer.Request;
using BusinessLayer.Response;
using BusinessLayer.Enums;
using Battleship;

namespace BattleShip
{
    class ControlOutput
    {
        static int counttime = 0;

        private static void ClearFlashScreen(Object state)
        {
            if (counttime < 2)
                counttime += 1;
            else
            {
                Console.Clear();
                counttime = 0;
            }
        }

        public static void ShowHeader()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("--------------------------------------");
            Console.Write("       BATTLESHIP      ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("--------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ShowPlayers(Player[] player)
        {
            string str = "Player 1: " + player[0].Name + "\t\t\t\t\t Player 2: " + player[1].Name + "";
            Console.WriteLine(str);
            Console.WriteLine("");
        }




        public static void ShowWhoseTurn(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(player.Name + " turn... ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;

        }

        static string GetLetterFromNumber(int number)
        {
            string result = "";
            switch (number)
            {
                case 1:
                    result = "A";
                    break;
                case 2:
                    result = "B";
                    break;
                case 3:
                    result = "C";
                    break;
                case 4:
                    result = "D";
                    break;
                case 5:
                    result = "E";
                    break;
                case 6:
                    result = "F";
                    break;
                case 7:
                    result = "G";
                    break;
                case 8:
                    result = "H";
                    break;
                case 9:
                    result = "I";
                    break;
                case 10:
                    result = "J";
                    break;
                default:
                    break;
            }
            return result;
        }

        public static void DrawHistory(Player player, int playerOrder)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("  ");
            for (int i = 0; i < 2; i++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    Console.Write(y);
                    Console.Write(" ");
                }
                Console.Write("\t\t\t\t");
            }
            Console.WriteLine();
            for (int x = 1; x <= 10; x++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(GetLetterFromNumber(x) + " ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int y = 1; y <= 10; y++)
                {
                    ShipTargets history = playerOrder == 1 ? player.PlayerBoard.CheckCoordinate(new Coordinate(x, y)) : player.PlayerBoard.ReturnUnknown();
                    switch (history)
                    {
                        case ShipTargets.Hit:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("H");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case ShipTargets.Miss:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("M");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case ShipTargets.Unknown:
                            Console.Write(" ");
                            break;
                    }
                    Console.Write("|");
                }
                Console.Write("\t\t\t\t");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(GetLetterFromNumber(x) + " ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int y = 1; y <= 10; y++)
                {
                    ShipTargets history = playerOrder == 2 ? player.PlayerBoard.CheckCoordinate(new Coordinate(x, y)) : player.PlayerBoard.ReturnUnknown();
                    switch (history)
                    {
                        case ShipTargets.Hit:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("H");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case ShipTargets.Miss:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("M");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case ShipTargets.Unknown:
                            Console.Write(" ");
                            break;
                    }
                    Console.Write("|");
                }
                Console.WriteLine();

            }
            Console.WriteLine();

        }

        public static void ShowShotResult(FireShotResponse shotresponse, Coordinate c, string playername)
        {
            String str = "";
            switch (shotresponse.ShotStatus)
            {
                case ShipHitCheck.Duplicate:
                    Console.ForegroundColor = ConsoleColor.Red;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Duplicate shot location!";
                    break;
                case ShipHitCheck.Hit:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Hit!";
                    break;
                case ShipHitCheck.HitAndSunk:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Hit and Sunk, " + shotresponse.ShipImpacted + "!";
                    break;
                case ShipHitCheck.Invalid:
                    Console.ForegroundColor = ConsoleColor.Red;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Invalid hit location!";
                    break;
                case ShipHitCheck.Miss:
                    Console.ForegroundColor = ConsoleColor.Red;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Miss!";
                    break;
                case ShipHitCheck.Victory:
                    Console.ForegroundColor = ConsoleColor.Green;
                    str = "Shot location: " + GetLetterFromNumber(c.XCoordinate) + c.YCoordinate.ToString() + "\t result: Hit and Sunk, " + shotresponse.ShipImpacted + "! \n\n";
                    str += "       ******\n";
                    str += "       ******\n";
                    str += "        **** \n";
                    str += "         **  \n";
                    str += "         **  \n";
                    str += "       ******\n";
                    str += "Game Over, " + playername + " wins!";
                    Console.ReadLine();
                    break;
            }
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }

        public static void ResetScreen(Player[] player)
        {
            GamePlayers gs = new GamePlayers();
            Console.Clear();
            ControlOutput.ShowHeader();
            ControlOutput.ShowPlayers(player);
        }
    }
}
