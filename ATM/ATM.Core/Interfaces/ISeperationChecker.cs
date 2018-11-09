using ATM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Interfaces
{
    public class SeperationAlertEventArgs : EventArgs
    {
        public Event Event { get; set; }

        public SeperationAlertEventArgs(Event _event)
        {
            Event = _event;
        }
    }

    public interface ISeperationChecker
    {
        event EventHandler<SeperationAlertEventArgs> SeperationAlert;
    }
}
