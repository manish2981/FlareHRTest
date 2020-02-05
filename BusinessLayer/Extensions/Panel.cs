using BusinessLayer.Enums;
using BusinessLayer.Request;
using BusinessLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Extensions
{
    public class Panel
    {
        public const int xCoordinator = 10;
        public const int yCoordinator = 10;
        private Dictionary<Coordinate, ShipTargets> ShotHistory;
        private int _currentShipIndex;

        public Battleships[] Ships { get; private set; }

        public Panel()
        {
            ShotHistory = new Dictionary<Coordinate, ShipTargets>();
            Ships = new Battleships[5];
            _currentShipIndex = 0;
        }

        public FireShotResponse FireShot(Coordinate coordinate)
        {
            var response = new FireShotResponse();

            // is this coordinate on the board?
            if (!IsValidCoordinate(coordinate))
            {
                response.ShotStatus = ShipHitCheck.Invalid;
                return response;
            }

            // did they already try this position?
            if (ShotHistory.ContainsKey(coordinate))
            {
                response.ShotStatus = ShipHitCheck.Duplicate;
                return response;
            }

            CheckPanelsForHit(coordinate, response);
            CheckForVictory(response);

            return response;
        }

        public ShipTargets CheckCoordinate(Coordinate coordinate)
        {
            if (ShotHistory.ContainsKey(coordinate))
            {
                return ShotHistory[coordinate];
            }
            else
            {
                return ReturnUnknown();
            }
        }

        public ShipTargets ReturnUnknown()
        {
            return ShipTargets.Unknown;
        }

        public ShipPlacements PlaceShip(ShipRequest request)
        {
            if (_currentShipIndex > 4)
                throw new Exception("You can not add another ship, 5 is the limit!");

            if (!IsValidCoordinate(request.Coordinate))
                return ShipPlacements.NotEnoughSpace;

            Battleships newShip = ShipCreation.CreateShip(request.ShipType);
            switch (request.Direction)
            {
                case ShipDirections.Down:
                    return PlaceShipDown(request.Coordinate, newShip);
                case ShipDirections.Up:
                    return PlaceShipUp(request.Coordinate, newShip);
                case ShipDirections.Left:
                    return PlaceShipLeft(request.Coordinate, newShip);
                default:
                    return PlaceShipRight(request.Coordinate, newShip);
            }
        }

        private void CheckForVictory(FireShotResponse response)
        {
            if (response.ShotStatus == ShipHitCheck.HitAndSunk)
            {
                // did they win?
                if (Ships.All(s => s.IsSunk))
                    response.ShotStatus = ShipHitCheck.Victory;
            }
        }

        private void CheckPanelsForHit(Coordinate coordinate, FireShotResponse response)
        {
            response.ShotStatus = ShipHitCheck.Miss;

            foreach (var ship in Ships)
            {
                // no need to check sunk Panels
                if (ship.IsSunk)
                    continue;

                ShipHitCheck status = ship.FireAtShip(coordinate);

                switch (status)
                {
                    case ShipHitCheck.HitAndSunk:
                        response.ShotStatus = ShipHitCheck.HitAndSunk;
                        response.ShipImpacted = ship.ShipName;
                        ShotHistory.Add(coordinate, ShipTargets.Hit);
                        break;
                    case ShipHitCheck.Hit:
                        response.ShotStatus = ShipHitCheck.Hit;
                        response.ShipImpacted = ship.ShipName;
                        ShotHistory.Add(coordinate, ShipTargets.Hit);
                        break;
                }

                // if they hit something, no need to continue looping
                if (status != ShipHitCheck.Miss)
                    break;
            }

            if (response.ShotStatus == ShipHitCheck.Miss)
            {
                ShotHistory.Add(coordinate, ShipTargets.Miss);
            }
        }

        private bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.XCoordinate >= 1 && coordinate.XCoordinate <= xCoordinator &&
            coordinate.YCoordinate >= 1 && coordinate.YCoordinate <= yCoordinator;
        }

        private ShipPlacements PlaceShipRight(Coordinate coordinate, Battleships newShip)
        {
            // y coordinate gets bigger
            int positionIndex = 0;
            int maxY = coordinate.YCoordinate + newShip.PanelPositions.Length;

            for (int i = coordinate.YCoordinate; i < maxY; i++)
            {
                var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);
                if (!IsValidCoordinate(currentCoordinate))
                    return Enums.ShipPlacements.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return Enums.ShipPlacements.Overlap;

                newShip.PanelPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return Enums.ShipPlacements.Ok;
        }

        private ShipPlacements PlaceShipLeft(Coordinate coordinate, Battleships newShip)
        {
            // y coordinate gets smaller
            int positionIndex = 0;
            int minY = coordinate.YCoordinate - newShip.PanelPositions.Length;

            for (int i = coordinate.YCoordinate; i > minY; i--)
            {
                var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);

                if (!IsValidCoordinate(currentCoordinate))
                    return Enums.ShipPlacements.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return Enums.ShipPlacements.Overlap;

                newShip.PanelPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return Enums.ShipPlacements.Ok;
        }

        private ShipPlacements PlaceShipUp(Coordinate coordinate, Battleships newShip)
        {
            // x coordinate gets smaller
            int positionIndex = 0;
            int minX = coordinate.XCoordinate - newShip.PanelPositions.Length;

            for (int i = coordinate.XCoordinate; i > minX; i--)
            {
                var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                if (!IsValidCoordinate(currentCoordinate))
                    return Enums.ShipPlacements.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return Enums.ShipPlacements.Overlap;

                newShip.PanelPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return Enums.ShipPlacements.Ok;
        }

        private ShipPlacements PlaceShipDown(Coordinate coordinate, Battleships newShip)
        {
            // y coordinate gets bigger
            int positionIndex = 0;
            int maxX = coordinate.XCoordinate + newShip.PanelPositions.Length;

            for (int i = coordinate.XCoordinate; i < maxX; i++)
            {
                var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                if (!IsValidCoordinate(currentCoordinate))
                    return Enums.ShipPlacements.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return Enums.ShipPlacements.Overlap;

                newShip.PanelPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return Enums.ShipPlacements.Ok;
        }

        private void AddShipToBoard(Battleships newShip)
        {
            Ships[_currentShipIndex] = newShip;
            _currentShipIndex++;
        }

        private bool OverlapsAnotherShip(Coordinate coordinate)
        {
            foreach (var ship in Ships)
            {
                if (ship != null)
                {
                    if (ship.PanelPositions.Contains(coordinate))
                        return true;
                }
            }

            return false;
        }
    }
}
