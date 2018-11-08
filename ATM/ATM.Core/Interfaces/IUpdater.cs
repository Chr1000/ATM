using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core;

namespace ATM.Interfaces
{
    public class TracksUpdatedEventArgs : EventArgs
    {
        public List<Track> UpdatedTracks { get; set; }

        public List<Event> EventsList { get; set; }

        public TracksUpdatedEventArgs(List<Track> updatedTracks)
        {
            UpdatedTracks = updatedTracks;
        }
    }

    public class SeperationCheckerEventArgs : EventArgs
    {
        public List<Track> UpdatedTracks { get; set; }

        public Track Track { get; set; }

        public SeperationCheckerEventArgs(List<Track> updatedTracks, Track track)
        {
            UpdatedTracks = updatedTracks;
            Track = track;
        }
    }

    public interface IUpdater
    {
        event EventHandler<TracksUpdatedEventArgs> TracksUpdated;
        event EventHandler<SeperationCheckerEventArgs> SeperationChecker;
    }
}
