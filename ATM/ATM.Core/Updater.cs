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


        public Updater(IFilter filter, ICalculator calculator)
        {
            UpdatedTracksList = new List<Track>();
            EventsList = new List<IEvent>();

            _calculator = calculator;
            calculator.CalculatedTrack += UpdatedTrack;

            filter.TracksFiltered += EvalTrack;
            filter.TrackLeft += TrackLeftedFunc;
        }

        private void UpdatedTrack(object o, CalculatedEventArgs args)
        {
            var updatedTrack = UpdatedTracksList.Find(i => i.Tag == args.Track.Tag);

            if (updatedTrack == null)
            {
                return;
            }
            UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)] = args.Track;

            SeperationChecker?.Invoke(this, new SeperationCheckerEventArgs(EventsList, UpdatedTracksList, args.Track));
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
                        //TrackStartCal?.Invoke(this, new TrackStartCalEventArgs(UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)],
                        //    filteredTrack));
                        filteredTrack.Course = _calculator.CalCourse(UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)],
                            filteredTrack) ;
                        filteredTrack.Velocity =
                            _calculator.CalVelocity(UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)], filteredTrack);
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
