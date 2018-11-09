using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Interfaces
{
    public class CalculatedEventArgs : EventArgs
    {
        public Track Track { get; set; }

        public CalculatedEventArgs(Track track)
        {
            Track = track;
        }
    }

    public interface ICalculator
    {
        event EventHandler<CalculatedEventArgs> CalculatedTrack;

        double CalVelocity(Track prevTrack, Track newTrack);
        double CalCourse(Track prevTrack, Track newTrack);
    }
}
