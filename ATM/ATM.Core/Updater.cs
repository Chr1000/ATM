using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core;
using ATM.Interfaces;

namespace ATM
{
    public class Updater : IUpdater
    {
        public event EventHandler<TracksUpdatedEventArgs> TracksUpdated;

        public event EventHandler<SeperationCheckerEventArgs> SeperationChecker;

        private List<Track> UpdatedTracksList;
        private List<Event> UpdatedEventsList;
        private ICalculator _calculator;


        public Updater(IFilter filtering, ICalculator calcVelocityCourse)
        {
            UpdatedTracksList = new List<Track>();
            _calculator = calcVelocityCourse;

            filtering.TracksFiltered += UpdateTrack;
        }

        private void UpdateTrack(object o, TracksFilteredEventArgs args)
        {
            if (args.FilteredTracks.Count != 0 && UpdatedTracksList.Count == 0)
            {
                foreach (var track in args.FilteredTracks)
                {
                    UpdatedTracksList.Add(track);
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
                        //Kald “Track Entered Airspace” event her
                    }

                    else
                    {
                        filteredTrack.Course = _calculator.CalCourse(UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)],
                            filteredTrack);
                        filteredTrack.Velocity =
                            _calculator.CalVelocity(UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)], filteredTrack);
                        UpdatedTracksList[UpdatedTracksList.IndexOf(updatedTrack)] = filteredTrack;

                        SeperationChecker?.Invoke(this, new SeperationCheckerEventArgs(UpdatedTracksList, filteredTrack ));
                    }
                }
            }

            var handler = TracksUpdated;
            handler?.Invoke(this, new TracksUpdatedEventArgs(UpdatedTracksList));
        }
    }
}
