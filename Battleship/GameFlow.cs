using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Extensions;
using BusinessLayer.Request;
using BusinessLayer.Response;
using BusinessLayer.Enums;
using Battleship;

namespace BattleShip
{
    class GameFlow
    {
        GamePlayers gm;

        public GameFlow()
        {
            gm = new GamePlayers() { IsPlayer1 = false, Player1 = new Player(), Player2 = new Player() };
        }

        public void Start()
        {
            GameSetup GameSetup = new GameSetup(gm);
            GameSetup.Setup();

            do
            {
                GameSetup.SetBoard();
                FireShotResponse shotresponse;
                do
                {

                    ControlOutput.ResetScreen(new Player[] { gm.Player1, gm.Player2 });
                    ControlOutput.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player2 : gm.Player1);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1, gm.IsPlayer1 ? 2 : 1);
                    Coordinate ShotPoint = new Coordinate(1, 1);
                    shotresponse = Shot(gm.IsPlayer1 ? gm.Player2 : gm.Player1, gm.IsPlayer1 ? gm.Player1 : gm.Player2, out ShotPoint);

                    ControlOutput.ResetScreen(new Player[] { gm.Player1, gm.Player2 });
                    ControlOutput.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player2 : gm.Player1);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1, gm.IsPlayer1 ? 2 : 1);
                    ControlOutput.ShowShotResult(shotresponse, ShotPoint, gm.IsPlayer1 ? gm.Player1.Name : gm.Player2.Name);
                    if (shotresponse.ShotStatus != ShipHitCheck.Victory)
                    {
                        Console.WriteLine("Press any key to continue to switch to " + (gm.IsPlayer1 ? gm.Player1.Name : gm.Player2.Name));
                        gm.IsPlayer1 = !gm.IsPlayer1;
                        Console.ReadKey();
                    }
                } while (shotresponse.ShotStatus != ShipHitCheck.Victory);

            } while (ControlInput.CheckQuit());
        }


        private FireShotResponse Shot(Player victim, Player Shoter, out Coordinate ShotPoint)
        {
            FireShotResponse fire; Coordinate WhereToShot;
            do
            {
                WhereToShot = ControlInput.GetShotLocationFromUser();
                fire = victim.PlayerBoard.FireShot(WhereToShot);
                if (fire.ShotStatus == ShipHitCheck.Invalid || fire.ShotStatus == ShipHitCheck.Duplicate) { }
            } while (fire.ShotStatus == ShipHitCheck.Duplicate || fire.ShotStatus == ShipHitCheck.Invalid);
            ShotPoint = WhereToShot;
            return fire;
        }
    }
}
