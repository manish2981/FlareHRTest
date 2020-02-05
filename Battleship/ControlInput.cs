using System;
using System.Collections.Generic;
using BusinessLayer.Extensions;
using BusinessLayer.Request;
using BusinessLayer.Response;
using BusinessLayer.Enums;

namespace BattleShip
{
    public class ControlInput
    {
        public static string[] GetNameFromUser()
        {
            string strLevel = "", player1 = "", player2 = "";
            do
            {
                Console.Write("Enter Player 1 Name: ");
                player1 = Console.ReadLine();
            } while (player1.Trim() == "");
            do
            {
                Console.Write("Enter Player 2 Name: ");
                player2 = Console.ReadLine();
            } while (player2.Trim() == "");
            return new string[] { player1, player2, strLevel };
        }

        public static ShipDirections getDirection(string direction)
        {
            switch (direction.ToLower())
            {
                case "l":
                    return ShipDirections.Left;
                case "r":
                    return ShipDirections.Right;
                case "u":
                    return ShipDirections.Up;
                default:
                    return ShipDirections.Down;
            }

        }

        public static ShipRequest GetLocationFromUser(string ShipType)
        {
            ShipRequest result = null;
            do
            {
                Console.Write("- " + ShipType + ": ");
                result = GetLocation(Console.ReadLine());
                if (result is null) ;
                else return result;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please input location and direiction. Ex:) a2, r");
                Console.ForegroundColor = ConsoleColor.White;
            } while (result is null);
            return result;
        }

        public static ShipRequest GetLocation(string location)
        {
            string strX, strY, strDirection; int x, y;

            if (location.Split(',').Length == 2)
            {
                if (location.Split(',')[0].Trim().Length > 1)
                {
                    strX = location.Split(',')[0].Trim().Substring(0, 1);
                    strY = location.Split(',')[0].Trim().Substring(1);
                    strDirection = location.Split(',')[1].ToLower().Trim();

                    x = GetNumberFromLetter(strX);
                    if (x > 0 && x < 11 && int.TryParse(strY, out y) && y > 0 && y < 11
                        && (strDirection == "l"
                        || strDirection == "r"
                        || strDirection == "u"
                        || strDirection == "d"))
                    {
                        ShipRequest ShipToPlace = new ShipRequest();
                        ShipToPlace.Direction = getDirection(strDirection);
                        ShipToPlace.Coordinate = new Coordinate(x, y);
                        return ShipToPlace;
                    }
                }
            }
            return null;
        }
        public static ShipRequest GetLocationFromComputer()
        {
            ShipRequest ShipToPlace = new ShipRequest();
            ShipToPlace.Direction = getDirection(GetRandom.GetDirection());
            ShipToPlace.Coordinate = new Coordinate(GetRandom.GetLocation(), GetRandom.GetLocation());
            return ShipToPlace;
        }

        public static Coordinate GetShotLocationFromUser()
        {
            string result = ""; int x, y;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Which location do you want to shot? ");
                result = Console.ReadLine();
                if (result.Trim().Length > 1)
                {
                    x = GetNumberFromLetter(result.Substring(0, 1));
                    if (x > 0 && int.TryParse(result.Substring(1), out y))
                    {
                        return new Coordinate(x, y);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Location!");
                    }
                }
            }
        }

        public static Coordinate GetShotLocationFromComputer(Panel victimboard)
        {
            return new Coordinate(GetRandom.GetLocation(), GetRandom.GetLocation());
        }

        static Coordinate GetRightLocationToShot(Panel victimboard)
        {
            List<Coordinate> tmpList = new List<Coordinate> { };
            for (int i = 0; i < victimboard.Ships.Length; i++)
            {
                Battleships tmpShip = victimboard.Ships[i];
                for (int j = 0; j < tmpShip.PanelPositions.Length; j++)
                {
                    if (victimboard.CheckCoordinate(tmpShip.PanelPositions[j]) == ShipTargets.Unknown)
                        tmpList.Add(tmpShip.PanelPositions[j]);
                }
            }

            return tmpList[GetRandom.r.Next(0, tmpList.Count - 1)];

        }

        static int GetNumberFromLetter(string letter)
        {
            int result = -1;
            switch (letter.ToLower())
            {
                case "a":
                    result = 1;
                    break;
                case "b":
                    result = 2;
                    break;
                case "c":
                    result = 3;
                    break;
                case "d":
                    result = 4;
                    break;
                case "e":
                    result = 5;
                    break;
                case "f":
                    result = 6;
                    break;
                case "g":
                    result = 7;
                    break;
                case "h":
                    result = 8;
                    break;
                case "i":
                    result = 9;
                    break;
                case "j":
                    result = 10;
                    break;
                default:
                    break;
            }
            return result;
        }

        public static bool CheckQuit()
        {
            Console.WriteLine("Press F5 to replay or any key to quit...");
            return Console.ReadKey().Key == ConsoleKey.F5;
        }
    }
}
