﻿using System;
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


        public List<Track> FilteredTracksList;
        private IAirspace _airspace;

        public Filter(IAirspace airspace, IParser parser)
        {
            FilteredTracksList = new List<Track>();
            _airspace = airspace;
            parser.TracksChanged += FilterTrack;
        }


        public void FilterTrack(object o, TracksChangedEventArgs args)
        {
            foreach (var track in args.Tracks)
            {
                if (_airspace.IsTrackInAirspace(track))
                {
                    FilteredTracksList.Add(track);
                }
                else
                {
                    foreach (var trackInFilList in FilteredTracksList )
                    {
                        if (track.Tag == trackInFilList.Tag)
                        {
                            //Kald “Track Left Airspace”- event her
                            TrackLeft?.Invoke(this, new TrackLeftAirspaceEventArgs(track));
                        }
                    }
                }
            }
            TracksFiltered?.Invoke(this, new TracksFilteredEventArgs(FilteredTracksList));
        }
    }
}
