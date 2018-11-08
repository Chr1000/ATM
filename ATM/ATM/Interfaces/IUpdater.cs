using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Interfaces
{
    public class TracksUpdatedEventArgs : EventArgs
    {
        public List<Track> UpdatedTracks { get; set; }

        public TracksUpdatedEventArgs(List<Track> updatedTracks)
        {
            UpdatedTracks = updatedTracks;
        }
    }

    public interface IUpdater
    {
        event EventHandler<TracksUpdatedEventArgs> TracksUpdated;
    }
}
