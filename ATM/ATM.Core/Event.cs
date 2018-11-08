using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core
{
    public class Event : Track
    {
        public string EventType { get; set; }

        public Event(string eventType)
        {
            EventType = eventType;

            if (EventType == "LEFT AIRSPACE" || EventType == "ENTERED AIRSPACE")
            {

            }
        }
    }
}
