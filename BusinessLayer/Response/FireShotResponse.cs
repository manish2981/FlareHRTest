using BusinessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Response
{
    public class FireShotResponse
    {
        public ShipHitCheck ShotStatus { get; set; }
        public string ShipImpacted { get; set; }
    }
}
