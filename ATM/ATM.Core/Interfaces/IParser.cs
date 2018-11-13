using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Interfaces
{
    public class TracksChangedEventArgs : EventArgs
    {
        public List<Track> Tracks { get; set; }

        public TracksChangedEventArgs(List<Track> tracks)
        {
            Tracks = tracks;
        }
    }
    public interface IParser
    {
        event EventHandler<TracksChangedEventArgs> TracksChanged;
    }
}