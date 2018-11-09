using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Interfaces
{
    public interface IEvent
    {
        List<IEvent> EventList { get; set; }
        string Print();
    }
}
