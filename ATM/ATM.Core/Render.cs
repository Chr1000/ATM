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
        
        public Render(IUpdater updater)
        {
            updater.TracksUpdated += RenderTracks;
        }

        public event EventHandler<WriteLineEventArgs> WriteLine;
        public event EventHandler<EventArgs> Clear;

        private void RenderTracks(object o, TracksUpdatedEventArgs args)
        {
            Clear?.Invoke(this, EventArgs.Empty);
            WriteLine?.Invoke(this, new WriteLineEventArgs("---------------------------------------------------------TRACKS--------------------------------------------------------"));
            //
            foreach (var track in args.UpdatedTracks)
            {
                RenderTrack(track);
            }
            for (int i = 0; i < 10 - args.UpdatedTracks.Count; i++)
            {
                WriteLine?.Invoke(this, new WriteLineEventArgs(""));
            }
            WriteLine?.Invoke(this, new WriteLineEventArgs("---------------------------------------------------------EVENTS--------------------------------------------------------"));
            if (args.EventsList.Count > 0)
            {
                foreach (var events in args.EventsList)
                {
                    if (events == null)
                    {
                        break;
                    }
                    WriteLine?.Invoke(this, new WriteLineEventArgs("                          " + events.Print()));
                }
            }
            for (int i = 0; i < 10 - args.EventsList.Count; i++)
            {
                WriteLine?.Invoke(this, new WriteLineEventArgs(""));
            }
            WriteLine?.Invoke(this, new WriteLineEventArgs(DateTime.Now.ToString("dd-MMM-yyyy - HH':'mm") + " ---------------------------- FLIGHTS in airspace " + args.UpdatedTracks.Count + " ------------------------------------------------"));
        }

        private void RenderTrack(Track track)
        {
            WriteLine?.Invoke(this, new WriteLineEventArgs("Tag: " + track.Tag + ", X: " + track.X + ", Y: " + track.Y + ", Altitude: " + track.Altitude + ", Velocity: " + track.Velocity + ", Course: " + track.Course));
        }
    }
}
