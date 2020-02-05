using BusinessLayer.Extensions;
using BusinessLayer.Request;
using BusinessLayer.Enums;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ShipPlacementTests
    {
        [Test]
        public void CanNotPlaceShipOffBoard()
        {
            Panel board = new Panel();
            ShipRequest request = new ShipRequest()
            {
                Coordinate = new Coordinate(15, 10),
                Direction = ShipDirections.Up,
                ShipType = ShipTypes.Destroyer
            };

            var response = board.PlaceShip(request);

            Assert.AreEqual(ShipPlacements.NotEnoughSpace, response);
        }

        [Test]
        public void CanNotPlaceShipPartiallyOnBoard()
        {
            Panel board = new Panel();
            ShipRequest request = new ShipRequest()
            {
                Coordinate = new Coordinate(10, 10),
                Direction = ShipDirections.Right,
                ShipType = ShipTypes.Carrier
            };

            var response = board.PlaceShip(request);

            Assert.AreEqual(ShipPlacements.NotEnoughSpace, response);
        }

        [Test]
        public void CanNotOverlapShips()
        {
            Panel board = new Panel();

            // let's put a carrier at (10,10), (9,10), (8,10), (7,10), (6,10)
            var carrierRequest = new ShipRequest()
            {
                Coordinate = new Coordinate(10, 10),
                Direction = ShipDirections.Left,
                ShipType = ShipTypes.Carrier
            };

            var carrierResponse = board.PlaceShip(carrierRequest);

            Assert.AreEqual(ShipPlacements.Ok, carrierResponse);

            // now let's put a destroyer overlapping the y coordinate
            var destroyerRequest = new ShipRequest()
            {
                Coordinate = new Coordinate(9, 9),
                Direction = ShipDirections.Down,
                ShipType = ShipTypes.Destroyer
            };

            var destroyerResponse = board.PlaceShip(destroyerRequest);

            Assert.AreEqual(ShipPlacements.Overlap, destroyerResponse);
        }
    }
}
