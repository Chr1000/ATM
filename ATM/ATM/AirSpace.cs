using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM
{
    class AirSpace : IAirSpace
    {
        public int SWCornerX { get; set; }
        public int SWCornerY { get; set; }
        public int NECornerX { get; set; }
        public int NECornerY { get; set; }
        public int lowerAlt { get; set; }
        public int upperAlt { get; set; }

        public AirSpace()
        {
            SWCornerY = 10000;
            SWCornerX = 10000;
            NECornerX = 90000;
            NECornerY = 90000;
            lowerAlt = 500;
            upperAlt = 20000;
        }

        public bool WithinAirSpace(Track t)
        {
            if (t.X >= SWCornerX && t.X <= NECornerX &&
                t.Y >= SWCornerY && t.Y <= NECornerY &&
                t.Altitude >= lowerAlt && t.Altitude <= upperAlt)
            {
                return true;
            }
            return false;
        }

    }
}
