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
            IEvent eventFound1 = SeperationAlertList.Find(i => i.Track.Tag == args.Track.Tag);
            IEvent eventFound2 = SeperationAlertList.Find(i => i.ConflictingTrack.Tag == args.Track.Tag);
            foreach (var updatedTrack in args.UpdatedTracks)
            {   
                if (args.Track.Tag != updatedTrack.Tag)
                {
                    if(CheckForSeparation(args.Track, updatedTrack))
                    {
                        if (eventFound1 == null && eventFound2 == null)
                        {
                            //Event is getting a SeperationChecker object, this, and by this it can make an event to SeperationStop.
                            IEvent alertEvent = new Event(args.EventList, "SEPERATION ALERT", args.Track, DateTime.Now, updatedTrack, this);
                            SeperationAlertList.Add(alertEvent);
                            SeperationAlert?.Invoke(this, new SeperationAlertEventArgs(alertEvent));
                        }
                    }
                    else if(eventFound1 != null || eventFound2 != null)
                    {
                        if (eventFound1 != null)
                        {
                            if (updatedTrack.Tag == eventFound1.ConflictingTrack.Tag)
                            {
                                SeperationStop?.Invoke(this, new SeperationStopEventArgs(eventFound1));
                                SeperationAlertList.Remove(eventFound1);
                            }
                            
                        }
                        else if (updatedTrack.Tag == eventFound2.Track.Tag)
                        {
                            SeperationStop?.Invoke(this, new SeperationStopEventArgs(eventFound2));
                            SeperationAlertList.Remove(eventFound2);
                        }
                       
                    }
                }
            }

            bool CheckForSeparation(Track t1, Track t2)
            {
                int _altDiff = Math.Abs(t1.Altitude - t2.Altitude);

                if (_altDiff <= 300)
                {

                    int _xDiff = Math.Abs(t1.X - t2.X);
                    int _yDiff = Math.Abs(t1.Y - t2.Y);
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
