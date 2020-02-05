using System;
using BusinessLayer.Extensions;
using BusinessLayer.Request;
using BusinessLayer.Response;
using BusinessLayer.Enums;
using Battleship;

namespace BattleShip
{
    public class GameSetup
    {
        GamePlayers _gm;
        public GameSetup(GamePlayers gm)
        {
            _gm = gm;
        }

        public void Setup()
        {
            Console.ForegroundColor = ConsoleColor.White;
            ControlOutput.ShowHeader();

            string[] userSetUp = ControlInput.GetNameFromUser();

            _gm.Player1.Name = userSetUp[0];

            _gm.Player2.Name = userSetUp[1];
        }

        public void SetBoard()
        {

            _gm.IsPlayer1 = BusinessLayer.Response.GetRandom.WhoseFirst();

            _gm.Player1.PlayerBoard = new Panel();
            PlaceShipOnBoard(_gm.Player1);

            _gm.Player2.PlayerBoard = new Panel();
            PlaceShipOnBoard(_gm.Player2);
            Console.WriteLine("All ship were placed successfully for both players ! Press any key to continue...");
            Console.ReadKey();
        }

        public void PlaceShipOnBoard(Player player)
        {
            for (ShipTypes s = ShipTypes.Destroyer; s <= ShipTypes.Carrier; s++)
            {
                ShipRequest ShipToPlace = new ShipRequest();
                ShipPlacements result;
                do
                {
                    ShipToPlace = ControlInput.GetLocationFromComputer();
                    ShipToPlace.ShipType = s;
                    result = player.PlayerBoard.PlaceShip(ShipToPlace);

                } while (result != ShipPlacements.Ok);
            }
        }
    }
}
