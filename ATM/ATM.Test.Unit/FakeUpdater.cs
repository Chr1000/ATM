using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM.Test.Unit
{
    class FakeUpdater : IUpdater
    {
        public event EventHandler<TrackStartCalEventArgs> TrackStartCal;
        public event EventHandler<TracksUpdatedEventArgs> TracksUpdated;
        public event EventHandler<SeperationCheckerEventArgs> SeperationChecker;
        public event EventHandler<TrackEnteredAirspaceEventArgs> TrackEntered;
        public event EventHandler<TrackLeftedAirspaceEventArgs> TrackLefted;
    }
}
