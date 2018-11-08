using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Interfaces
{
    public class TracksFilteredEventArgs : EventArgs
    {
        public List<Track> FilteredTracks { get; set; }

        public TracksFilteredEventArgs(List<Track> filteredTracks)
        {
            FilteredTracks = filteredTracks;
        }
    }

    public interface IFilter
    {
        event EventHandler<TracksFilteredEventArgs> TracksFiltered;
    }
}
