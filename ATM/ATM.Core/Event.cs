using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ATM.Core.Interfaces;
using ATM.Interfaces;

namespace ATM.Core
{
    public class Event : IEvent
    {
        public List<IEvent> EventList { get; set; }

        private DateTime _time;
        private Track _track;
        private Track _conflictingTrack;
        private string _eventType;

        private Timer timer;

        public Event(List<IEvent> eventList, string eventType, Track track, DateTime time, Track conflictingTrack = null)
        {
            EventList = eventList;
            _eventType = eventType;
            _track = track;
            _time = time;
            _conflictingTrack = conflictingTrack;
            //updater.Re skal fjerne hvis en track ikke er seperation mode mere.

            EventList.Add(this);

            if (eventType == "Track Entered Airspace" || eventType == "Track Left Airspace")
            {
                timer = new Timer();
                timer.Interval = 5000; // 5 sek
                timer.Elapsed += Tick;
                timer.Start();
            }

        }

        private void Tick(object sender, EventArgs e)
        {
            EventList.Remove(this);
        }

        public string Print()
        {
            string returnString = _eventType + "!! " + _track.Tag + " ";
            if (_eventType != "Track Entered Airspace" && _eventType != "Track Left Airspace")
            {
                returnString = returnString + "conflicting with " + _conflictingTrack;
            }

            returnString = returnString + " " + _time;

            return returnString;
        }
    }
}
