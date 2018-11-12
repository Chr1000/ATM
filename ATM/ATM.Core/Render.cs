using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core.Interfaces;
using ATM.Interfaces;

namespace ATM
{
    public class Render : IRender
    {
        private IOutput _output;
        
        public Render(IUpdater updater, IOutput output, ISeperationChecker checker)
        {
            _output = output;
            updater.TracksUpdated += RenderTracks;
            //checker.SeperationAlert += RaisSeperationAlert;
            updater.TrackEntered += RaiseTrackEnteredEvent;
            updater.TrackLefted += RaiseTrackLeftedEvent;
            checker.SeperationAlert += RaiseSeperationAlertEvent;

        }

        private void RenderTracks(object o, TracksUpdatedEventArgs args)
        {
            _output.Clear();
            _output.WriteLine("---------------------------------------------------------TRACKS--------------------------------------------------------");
            foreach (var track in args.UpdatedTracks)
            {
                RenderTrack(track);
            }
            for (int i = 0; i < 10 - args.UpdatedTracks.Count; i++)
            {
                _output.WriteLine("");
            }
            _output.WriteLine("---------------------------------------------------------EVENTS--------------------------------------------------------");
            if (args.EventsList.Count > 0)
            {
                foreach (var events in args.EventsList)
                {
                    if (events == null)
                    {
                        break;
                    }
                    _output.WriteLine("                          " + events.Print());
                }
            }
            for (int i = 0; i < 10 - args.EventsList.Count; i++)
            {
                _output.WriteLine("");
            }
            _output.WriteLine(DateTime.Now.ToString("dd-MMM-yyyy - HH':'mm") + " ---------------------------- FLIGHTS in airspace " + args.UpdatedTracks.Count +" ------------------------------------------------");
        }

        //private void RaisSeperationAlert(object o, SeperationAlertEventArgs args)
        //{
        //    string seperationEvent = "SEPERATION EVENT! " + args.ConflictingTrack1.Tag + " and " + args.ConflictingTrack2.Tag + " are conflicting. " + DateTime.Now.ToString("h:mm:ss tt");
        //    EventList.Add(seperationEvent);
        //}

        private void RaiseTrackEnteredEvent(object o, TrackEnteredAirspaceEventArgs args)
        {

        }

        private void RaiseTrackLeftedEvent(object o, TrackLeftedAirspaceEventArgs args)
        {

        }

        private void RaiseSeperationAlertEvent(object o, SeperationAlertEventArgs args)
        {

        }

        private void RenderTrack(Track track)
        {
            _output.WriteLine("Tag: " + track.Tag + ", X: " + track.X + ", Y: " + track.Y + ", Altitude: " + track.Altitude + ", Velocity: " + track.Velocity + ", Course: " + track.Course);
        }
    }
}
