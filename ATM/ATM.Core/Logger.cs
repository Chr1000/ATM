using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core.Interfaces;
using ATM.Interfaces;

namespace ATM.Core
{
    public class Logger : ILogger
    {

        public Logger(IUpdater updater, ISeperationChecker checker)
        {
            updater.TrackEntered += logEnteredEvent;
            updater.TrackLefted += logLeftedEvent;
            checker.SeperationAlert += logSeperationEvent;
        }

        private void logEnteredEvent(object o, TrackEnteredAirspaceEventArgs args)
        {
             Log(args.Event);
        }

        private void logLeftedEvent(object o, TrackLeftedAirspaceEventArgs args)
        {
             Log(args.Event);
        }

        private void logSeperationEvent(object o, SeperationAlertEventArgs args)
        {
            Log(args.Event);
        }

        private static void Log(IEvent _event)
        {
            string path = "Log.txt";

            using (StreamWriter w = File.AppendText(path))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("  :");
                w.WriteLine("  :{0}", _event.Print());
                w.WriteLine("-------------------------------");
            }
        }
    }
}
