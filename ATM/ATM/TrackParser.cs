using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class TrackParser
    {

        public Track ReadTrackData(string trackData)
        {
            string[] seperatedStrings = trackData.Split(';');

            var track = new Track();
            track.Tag = seperatedStrings[0];
            track.X = Int32.Parse(seperatedStrings[1]);
            track.Y = Int32.Parse(seperatedStrings[2]);
            track.Altitude = Int32.Parse(seperatedStrings[3]);
            track.TimeStamp = DateTime.ParseExact(seperatedStrings[4], "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            track.Course = 0;
            track.Velocity = 0;

            return track;
        }
    }
}
