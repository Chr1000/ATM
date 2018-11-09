using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core.Interfaces;
using ATM.Interfaces;

namespace ATM.Core
{
    public class SeperationChecker : ISeperationChecker
    {
        public SeperationChecker(IUpdater updater)
        {
            updater.SeperationChecker += CheckSeperation;
        }
        public event EventHandler<SeperationAlertEventArgs> SeperationAlert;

        private void CheckSeperation(object o, SeperationCheckerEventArgs args)
        {
            foreach (var updatedTrack in args.UpdatedTracks)
            {
                if (args.Track.Tag == updatedTrack.Tag)
                {
                }
                /*else if ((Math.Abs(args.Track.Altitude - updatedTrack.Altitude) >= 300 ||
                         (args.Track.X + 5000 <= updatedTrack.X && args.Track.X - 5000 <= updatedTrack.X) &&
                         (args.Track.Y + 5000 >= updatedTrack.Y && args.Track.Y - 5000 <= updatedTrack.Y)))
                {
                    SeperationAlert?.Invoke(this, new SeperationAlertEventArgs(args.EventList , args.Track, updatedTrack));
                }*/
            }
        }
    }


}
