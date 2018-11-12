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


        public List<Track> UpdatedTracksList;
        private IAirspace _airspace;

        public Filter(IAirspace airspace, IParser parser)
        {
            UpdatedTracksList = new List<Track>();
            _airspace = airspace;
            parser.TracksChanged += FilterTrack;
        }


        public void FilterTrack(object o, TracksChangedEventArgs args)
        {
            List<Track> newFilteredTracks = new List<Track>();
            foreach (var track in args.Tracks)
            {
                if (_airspace.IsTrackInAirspace(track))
                {
                    newFilteredTracks.Add(track);
                }
                else
                {
                    
                    foreach (var trackInUpdatedList in UpdatedTracksList )
                    {
                        if (track.Tag == trackInUpdatedList.Tag)
                        {
                            //Kald “Track Left Airspace”- event her
                            //FilteredTracksList.Remove(trackInFilList);
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
