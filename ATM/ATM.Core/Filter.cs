using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core;
using ATM.Interfaces;

namespace ATM
{
    public class Filter : IFilter
    {
        public event EventHandler<TracksFilteredEventArgs> TracksFiltered;

        public event EventHandler<TrackLeftAirspaceEventArgs> TrackLeft;

        public event EventHandler<IsTrackInAirspaceEventArgs> IsTrackInAirspace;


        public List<Track> UpdatedTracksList;

        public Filter(IParser parser)
        {
            UpdatedTracksList = new List<Track>();
            parser.TracksChanged += FilterTrack;
        }


        public void FilterTrack(object o, TracksChangedEventArgs args)
        {
            List<Track> newFilteredTracks = new List<Track>();
            foreach (var track in args.Tracks)
            {
                var newEvent = new IsTrackInAirspaceEventArgs(track);
                IsTrackInAirspace?.Invoke(this, newEvent);
                if (newEvent.IsInAirspace)
                {
                    newFilteredTracks.Add(track);
                }
                else
                {
                    foreach (var trackInUpdatedList in UpdatedTracksList )
                    {
                        if (track.Tag == trackInUpdatedList.Tag)
                        {
                            TrackLeft?.Invoke(this, new TrackLeftAirspaceEventArgs(track));
                            break;
                        }
                    }
                }
            }
            TracksFilteredEventArgs Args = new TracksFilteredEventArgs(newFilteredTracks);
            TracksFiltered?.Invoke(this, Args);
            UpdatedTracksList = Args.UpdatedTracks;
        }
    }
}
