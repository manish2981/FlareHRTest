using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Enums
{
    public enum ShipHitCheck
    {
        Invalid,
        Duplicate,
        Miss,
        Hit,
        HitAndSunk,
        Victory
    }
}
