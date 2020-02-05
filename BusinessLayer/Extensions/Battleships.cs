using BusinessLayer.Enums;
using BusinessLayer.Request;
using System.Linq;

namespace BusinessLayer.Extensions
{
    public class Battleships
    {
        public ShipTypes ShipType { get; private set; }
        public string ShipName { get { return ShipType.ToString(); } }
        public Coordinate[] PanelPositions { get; set; }
        private int _lifeRemaining;
        public bool IsSunk { get { return _lifeRemaining == 0; } }

        public Battleships(ShipTypes shipType, int numberOfSlots)
        {
            ShipType = shipType;
            _lifeRemaining = numberOfSlots;
            PanelPositions = new Coordinate[numberOfSlots];
        }

        public ShipHitCheck FireAtShip(Coordinate position)
        {
            if (PanelPositions.Contains(position))
            {
                _lifeRemaining--;

                if (_lifeRemaining == 0)
                    return ShipHitCheck.HitAndSunk;

                return ShipHitCheck.Hit;
            }

            return ShipHitCheck.Miss;
        }
    }
}
