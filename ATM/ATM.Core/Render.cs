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
        private List<string> AlertList;
        public Render(IUpdater trackUpdate, IOutput output, ISeperationChecker checker)
        {
            AlertList = new List<string>();
            _output = output;
            trackUpdate.TracksUpdated += RenderTracks;
            checker.SeperationAlert += RaisSeperationAlert;

        }

        private void RenderTracks(object o, TracksUpdatedEventArgs args)
        {
            _output.Clear();
            foreach (var alerts in AlertList)
            {
                _output.WriteLine(alerts);
            }
            foreach (var track in args.UpdatedTracks)
            {
                RenderTrack(track);
            }
        }

        private void RaisSeperationAlert(object o, SeperationAlertEventArgs args)
        {
            DateTime data = new DateTime();
            string alert = "SEPERATION EVENT! " + args.ConflictingTrack1.Tag + " and " + args.ConflictingTrack2.Tag + " are conflicting. " + data;
            AlertList.Add(alert);
        }

        public void RenderTrack(Track track)
        {
            _output.WriteLine("Tag: " + track.Tag + ", X: " + track.X + ", Y: " + track.Y + ", Altitude: " + track.Altitude + ", Velocity: " + track.Velocity + ", Course: " + track.Course);
        }
    }
}
