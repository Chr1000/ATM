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
                if (CheckForSeparation(args.Track , updatedTrack))
                {
                    SeperationAlert?.Invoke(this, new SeperationAlertEventArgs(new Event(args.EventList, "SEPERATION ALERT", args.Track, DateTime.Now, updatedTrack )));
                }
            }

            bool CheckForSeparation(Track t1, Track t2)
            {

                int _altDiff = Math.Abs(t1.Altitude - t2.Altitude);

                if (_altDiff <= 300)
                {

                    int _xDiff = t1.X - t2.X;
                    int _yDiff = t1.Y - t2.Y;
                    int _horizontalDist = Convert.ToInt32(System.Math.Floor(Math.Sqrt(Math.Pow(_xDiff, 2) + Math.Pow(_yDiff, 2))));

                    if (_horizontalDist < 5000)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }


}
