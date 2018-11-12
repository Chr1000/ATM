using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;
using ATM.Core.Interfaces;
using ATM.Interfaces;

namespace ATM.Core
{
    public class Event : IEvent
    {
        public List<IEvent> EventList { get; set; }
        public Track Track { get; }
        public Track ConflictingTrack { get; }

        private DateTime _time;
        private string _eventType;
        private Timer timer;

        public Event(List<IEvent> eventList, string eventType, Track track, DateTime time, Track conflictingTrack = null, ISeperationChecker checker = null)
        {
            EventList = eventList;
            _eventType = eventType;
            Track = track;
            _time = time;
            ConflictingTrack = conflictingTrack;
            //updater.Re skal fjerne hvis en track ikke er seperation mode mere.

            EventList.Add(this);

            if (checker == null)
            {
                timer = new Timer();
                timer.Interval = 5000; // 5 sek
                timer.Elapsed += DeleteThisEvent;
                timer.Start();
            }
            else
            {
                checker.SeperationStop += DeleteThisIfSeperationEvent;
            }

        }

        private void DeleteThisEvent(object sender, EventArgs e)
        {
            EventList.Remove(this);
        }

        private void DeleteThisIfSeperationEvent(object o, SeperationStopEventArgs args)
        {
            if (args.Event == this)
            {
                EventList.Remove(this);
            }
            
        }

        public string Print()
        {
            string returnString = _eventType + "!! " + Track.Tag + " ";
            if (_eventType != "Track Entered Airspace" && _eventType != "Track Left Airspace")
            {
                returnString = returnString + "conflicting with " + ConflictingTrack.Tag;
            }

            returnString = returnString + " " + _time;

            return returnString;
        }
    }
}
