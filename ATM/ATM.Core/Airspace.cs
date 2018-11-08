using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM
{
    public class Airspace : IAirspace
    {
        public int SWCornerX { get; set; }
        public int SWCornerY { get; set; }
        public int NECornerX { get; set; }
        public int NECornerY { get; set; }
        public int lowerAlt { get; set; }
        public int upperAlt { get; set; }

        public Airspace()
        {
            SWCornerY = 10000;
            SWCornerX = 10000;
            NECornerX = 90000;
            NECornerY = 90000;
            lowerAlt = 500;
            upperAlt = 20000;
        }

        public bool IsTrackInAirspace(Track track)
        {
            if (track.X >= SWCornerX && track.X <= NECornerX &&
                track.Y >= SWCornerY && track.Y <= NECornerY &&
                track.Altitude >= lowerAlt && track.Altitude <= upperAlt)
            {
                return true;
            }
            return false;
        }
    }
}
