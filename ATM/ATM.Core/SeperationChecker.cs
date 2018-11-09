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

        private List<IEvent> SeperationAlertList;

        public event EventHandler<SeperationAlertEventArgs> SeperationAlert;

        public event EventHandler<SeperationStopEventArgs> SeperationStop;

        public SeperationChecker(IUpdater updater)
        {
            updater.SeperationChecker += CheckSeperation;
            SeperationAlertList = new List<IEvent>();
        }

        private void CheckSeperation(object o, SeperationCheckerEventArgs args)
        {
            foreach (var updatedTrack in args.UpdatedTracks)
            {
                Track track = SeperationAlertList.Find(i => i.Track.Tag == updatedTrack.Tag).Track;
                Track conflictingTrack = SeperationAlertList.Find(i => i.ConflictingTrack.Tag == updatedTrack.Tag).ConflictingTrack;
                if (args.Track.Tag != updatedTrack.Tag)
                {
                    if(CheckForSeparation(args.Track, updatedTrack) && track == null && conflictingTrack == null)
                    {
                        //Event is getting a SeperationChecker object, this, and by this it can make an event to SeperationStop.
                        IEvent alertEvent = new Event(args.EventList, "SEPERATION ALERT", args.Track, DateTime.Now, updatedTrack, this);
                        SeperationAlertList.Add(alertEvent);
                        SeperationAlert?.Invoke(this, new SeperationAlertEventArgs(alertEvent));
                    }
                    else if(SeperationAlertList.Count > 0)
                    {
                        foreach (var events in SeperationAlertList)
                        {
                            if (args.Track.Tag == events.Track.Tag || args.Track.Tag == events.ConflictingTrack.Tag)
                            {
                                SeperationStop?.Invoke(this, new SeperationStopEventArgs(events));
                            }
                        }
                    }
                }
            }

            bool CheckForSeparation(Track t1, Track t2)
            {
                int _altDiff = Math.Abs(t1.Altitude - t2.Altitude);

                if (_altDiff <= 300)
                {

                    int _xDiff = t1.X - t2.X;
                    int _yDiff = t1.Y - t2.Y;
                    int _horizontalDist = Convert.ToInt32(Math.Sqrt(Math.Pow(_xDiff, 2) + Math.Pow(_yDiff, 2)));

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
