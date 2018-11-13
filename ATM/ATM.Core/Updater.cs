using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ATM.Core;
using ATM.Core.Interfaces;
using ATM.Interfaces;

namespace ATM
{
    public class Updater : IUpdater
    {
        public event EventHandler<TrackStartCalEventArgs> TrackStartCal;

        public event EventHandler<TracksUpdatedEventArgs> TracksUpdated;

        public event EventHandler<SeperationCheckerEventArgs> SeperationChecker;

        public event EventHandler<TrackEnteredAirspaceEventArgs> TrackEntered;

        public event EventHandler<TrackLeftedAirspaceEventArgs> TrackLefted;

        private List<Track> UpdatedTracksList;
        private List<IEvent> EventsList;

        private ICalculator _calculator;


        public Updater(IFilter filter)
        {
            UpdatedTracksList = new List<Track>();
            EventsList = new List<IEvent>();

            filter.TracksFiltered += EvalTrack;
            filter.TrackLeft += TrackLeftedFunc;
        }

        private void TrackLeftedFunc(object o, TrackLeftAirspaceEventArgs args)
        {
            var trackToRemove = UpdatedTracksList.Find(i => i.Tag == args.Track.Tag);
            if (trackToRemove == null)
            {
                return;
            }
            UpdatedTracksList.Remove(trackToRemove);
            TrackLefted?.Invoke(this, new TrackLeftedAirspaceEventArgs(new Event(EventsList, "Track Left Airspace", args.Track, DateTime.Now)));
            TracksUpdated?.Invoke(this, new TracksUpdatedEventArgs(UpdatedTracksList, EventsList));
        }

        private void EvalTrack(object o, TracksFilteredEventArgs args)
        {
            if (args.FilteredTracks.Count != 0 && UpdatedTracksList.Count == 0)
            {
                foreach (var track in args.FilteredTracks)
                {
                    UpdatedTracksList.Add(track);
                    TrackEntered?.Invoke(this, new TrackEnteredAirspaceEventArgs(new Event(EventsList, "Track Entered Airspace", track, DateTime.Now)));
                }
            }

            else if (args.FilteredTracks.Count != 0 && UpdatedTracksList.Count != 0)
            {
                foreach (var filteredTrack in args.FilteredTracks)
                {
                    var updatedTrack = UpdatedTracksList.Find(i => i.Tag == filteredTrack.Tag);

                    if (updatedTrack == null)
                    {
                        UpdatedTracksList.Add(filteredTrack);
                        TrackEntered?.Invoke(this, new TrackEnteredAirspaceEventArgs(new Event(EventsList , "Track Entered Airspace", filteredTrack, DateTime.Now)));
                    }
                    else
                    {
                        var newCalEvent = new TrackStartCalEventArgs(
                            UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)],
                            filteredTrack);
                        TrackStartCal?.Invoke(this, newCalEvent);
                        filteredTrack.Course = newCalEvent.CalculatedTrack.Course;
                        filteredTrack.Velocity = newCalEvent.CalculatedTrack.Velocity;
                        UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)] = filteredTrack;

                        SeperationChecker?.Invoke(this, new SeperationCheckerEventArgs(EventsList, UpdatedTracksList, filteredTrack));
                    }
                }
            }
            args.UpdatedTracks = UpdatedTracksList;
            TracksUpdated?.Invoke(this, new TracksUpdatedEventArgs(UpdatedTracksList, EventsList));
        }
    }
}
