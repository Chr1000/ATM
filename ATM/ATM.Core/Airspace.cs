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

        public Airspace(IFilter filter)
        {
            SWCornerY = 10000;
            SWCornerX = 10000;
            NECornerX = 90000;
            NECornerY = 90000;
            lowerAlt = 500;
            upperAlt = 20000;

            filter.IsTrackInAirspace += IsTrackInAirspace;
        }

        private void IsTrackInAirspace(object o, IsTrackInAirspaceEventArgs args)
        {
            if (args.Track.X >= SWCornerX && args.Track.X <= NECornerX &&
                args.Track.Y >= SWCornerY && args.Track.Y <= NECornerY &&
                args.Track.Altitude >= lowerAlt && args.Track.Altitude <= upperAlt)
            {
                args.IsInAirspace = true;
            }
            else
            {
                args.IsInAirspace = false;
            }
        }
    }
}
