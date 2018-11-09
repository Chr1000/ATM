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
        public IEvent Event { get; set; }

        public SeperationAlertEventArgs(IEvent _event)
        {
            Event = _event;
        }
    }

    public class SeperationStopEventArgs : EventArgs
    {
        public IEvent Event { get; set; }

        public SeperationStopEventArgs(IEvent _event)
        {
            Event = _event;
        }
    }

    public interface ISeperationChecker
    {
        event EventHandler<SeperationAlertEventArgs> SeperationAlert;
        event EventHandler<SeperationStopEventArgs> SeperationStop;
    }
}
