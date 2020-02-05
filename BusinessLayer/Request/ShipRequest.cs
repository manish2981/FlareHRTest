using BusinessLayer.Enums;

namespace BusinessLayer.Request
{
    public class ShipRequest
    {
        public Coordinate Coordinate { get; set; }
        public ShipDirections Direction { get; set; }
        public ShipTypes ShipType { get; set; }
    }
}

