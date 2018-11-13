using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core;
using ATM.Core.Interfaces;

namespace ATM.Interfaces
{
    public class TracksUpdatedEventArgs : EventArgs
    {
        public List<Track> UpdatedTracks { get; set; }

        public List<IEvent> EventsList { get; set; }

        public TracksUpdatedEventArgs(List<Track> updatedTracks, List<IEvent> eventsList)
        {
            UpdatedTracks = updatedTracks;
            EventsList = eventsList;
        }
    }

    public class SeperationCheckerEventArgs : EventArgs
    {
        public Track Track { get; set; }

        public List<Track> UpdatedTracks { get; set; }

        public List<IEvent> EventList { get; set; }

        public SeperationCheckerEventArgs(List<IEvent> eventList, List<Track> updatedTrackList, Track track)
        {
            EventList = eventList;
            UpdatedTracks = updatedTrackList;
            Track = track;
        }
    }

    public class TrackEnteredAirspaceEventArgs : EventArgs
    {
        public IEvent Event { get; set; }

        public TrackEnteredAirspaceEventArgs(IEvent _event)
        {
            Event = _event;
        }
    }

    public class TrackLeftedAirspaceEventArgs : EventArgs
    {
        public IEvent Event { get; set; }

        public TrackLeftedAirspaceEventArgs(IEvent _event)
        {
            Event = _event;
        }
    }

    public class TrackStartCalEventArgs : EventArgs
    {
        public Track PrevTrack { get; set; }

        public Track NewTrack { get; set; }

        public Track CalculatedTrack { get; set; }

        public TrackStartCalEventArgs(Track prevTrack, Track newTrack)
        {
            PrevTrack = prevTrack;
            NewTrack = newTrack;
        }
    }


    public interface IUpdater
    {
        event EventHandler<TrackStartCalEventArgs> TrackStartCal; 
        event EventHandler<TracksUpdatedEventArgs> TracksUpdated;
        event EventHandler<SeperationCheckerEventArgs> SeperationChecker;
        event EventHandler<TrackEnteredAirspaceEventArgs> TrackEntered;
        event EventHandler<TrackLeftedAirspaceEventArgs> TrackLefted;
    }
}
