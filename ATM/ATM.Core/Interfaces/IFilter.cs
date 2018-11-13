using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core;

namespace ATM.Interfaces
{
    public class TracksFilteredEventArgs : EventArgs
    {
        public List<Track> FilteredTracks { get; set; }

        public List<Track> UpdatedTracks { get; set; }

        public TracksFilteredEventArgs(List<Track> filteredTracks)
        {
            FilteredTracks = filteredTracks;
        }
    }

    public class TrackLeftAirspaceEventArgs : EventArgs
    {
        public Track Track { get; set; }

        public TrackLeftAirspaceEventArgs(Track track)
        {
            Track = track;
        }
    }

    public class IsTrackInAirspaceEventArgs : EventArgs
    {
        public Track Track { get; set; }

        public bool IsInAirspace { get; set; }

        public IsTrackInAirspaceEventArgs (Track track)
        {
            Track = track;
        }
    }

    public interface IFilter
    {
        event EventHandler<IsTrackInAirspaceEventArgs> IsTrackInAirspace;
        event EventHandler<TracksFilteredEventArgs> TracksFiltered;
        event EventHandler<TrackLeftAirspaceEventArgs> TrackLeft;
    }
}
