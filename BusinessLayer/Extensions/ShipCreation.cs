using BusinessLayer.Enums;

namespace BusinessLayer.Extensions
{
    public class ShipCreation
    {
        public static Battleships CreateShip(ShipTypes type)
        {
            switch (type)
            {
                case ShipTypes.Destroyer:
                    return new Battleships(ShipTypes.Destroyer, 2);
                case ShipTypes.Cruiser:
                    return new Battleships(ShipTypes.Cruiser, 3);
                case ShipTypes.Submarine:
                    return new Battleships(ShipTypes.Submarine, 3);
                case ShipTypes.Battleship:
                    return new Battleships(ShipTypes.Battleship, 4);
                default:
                    return new Battleships(ShipTypes.Carrier, 5);
            }
        }
    }
}
