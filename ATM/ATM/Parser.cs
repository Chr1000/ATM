using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;
using TransponderReceiver;

namespace ATM
{
    public class Parser : IParser
    {
        public event EventHandler<TracksChangedEventArgs> TracksChanged;
        private List<Track> tracks;


        public Parser(ITransponderReceiver transponderReceiver)
        {
            tracks = new List<Track>();

            transponderReceiver.TransponderDataReady += UpdateTrack;
        }

        private void UpdateTrack(object o, RawTransponderDataEventArgs args)
        {
            tracks.Clear();
            foreach (var data in args.TransponderData)
            {
                var track = ReadTrackData(data);
                tracks.Add(track);
            }

            if (tracks.Count != 0)
            {
                var handler = TracksChanged;
                handler?.Invoke(this, new TracksChangedEventArgs(tracks));
            }
        }

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