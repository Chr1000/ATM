using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM
{
    public class Calculator : ICalculator
    {
        public event EventHandler<CalculatedEventArgs> CalculatedTrack;

        public Calculator()
        {
            //updater.TrackStartCal += CalTrack;
        }

        public void CalTrack(object o, TrackStartCalEventArgs args)
        {
            Track track = args.NewTrack;
            track.Velocity = CalVelocity(args.PrevTrack, args.NewTrack);
            track.Course = CalCourse(args.PrevTrack, args.NewTrack);

            CalculatedTrack?.Invoke(this, new CalculatedEventArgs(track));

        }
        public double CalVelocity(Track prevTrack, Track newTrack)
        {
            double time = newTrack.TimeStamp.Subtract(prevTrack.TimeStamp).TotalSeconds;
            double distance = Math.Sqrt(Math.Pow(newTrack.X - prevTrack.X, 2) + Math.Pow(newTrack.Y - prevTrack.Y, 2));
            double velToReturn = distance / time;
            return velToReturn;
        }

        public double CalCourse(Track prevTrack, Track newTrack)
        {

            double X = Math.Abs(newTrack.X - prevTrack.X);
            double Y = Math.Abs(newTrack.Y - prevTrack.Y);
            double kurs = Math.Atan2(X, Y) * (180 / Math.PI);


            if (newTrack.X > prevTrack.X && newTrack.Y <= prevTrack.Y)
            {
                kurs += 90;
            }
            else if (newTrack.X <= prevTrack.X && newTrack.Y < prevTrack.Y)
            {
                kurs += 180;
            }
            else if (newTrack.X < prevTrack.X && newTrack.Y > prevTrack.Y)
            {
                kurs += 270;
            }
            return kurs;
        }
    }
}