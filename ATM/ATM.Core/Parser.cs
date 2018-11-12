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
        //TEST
        private int count = -70;
        private int F1x = 20000;
        private int F1y = 10050;

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

            if (count >= 1 && count <= 27)
            {
                tracks.Add(ReadTrackData("NIC666;" + F1x + ";"+ (F1y - 2*count) + ";" + 5000 +";20181004123456789"));
            }
        
            count = count + 1;

            if (tracks.Count != 0)
            {
                TracksChanged?.Invoke(this, new TracksChangedEventArgs(tracks));
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