using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ATM.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace ATM.Test.Unit
{
    [TestFixture]
    public class CalculatorUnitTest
    {
        private IUpdater _updater;
        private ICalculator _calculator;

        private Track _newTrackStartCal;
        private Track _prevTrackStartCal;
        private Track _calculatedTrack;

        private int _nEventsReceived;

        [SetUp]
        public void SetUp()
        {
            _updater = Substitute.For<IUpdater>();
            _calculator = new Calculator(_updater);

            _updater.TrackStartCal += (o, args) =>
            {
                _newTrackStartCal = args.NewTrack;
                _prevTrackStartCal = args.PrevTrack;
                _calculatedTrack = args.CalculatedTrack;
                _nEventsReceived++;
            };
        }
  
        [TestCase(25000, 25000, 25010, 25010, 7.0710678118654755, 45)]
        [TestCase(45000, 15000, 45050, 15010, 25.495097567963924, 78.690067525979785)]
        [TestCase(50100, 20000, 50050, 19945, 37.165171868296262, 222.27368900609375)]
        public void Initial_TrackStartCalcOneTrack_VelocityAndCourseIsCorrect(int t1x, int t1y, int t2x, int t2y, double result1, double result2)
        {
            Track prevtrack = new Track()
            {
                Tag = "NIC222",
                X = t1x,
                Y = t1y,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };


            Track newTrack = new Track()
            {
                Tag = "NIC222",
                X = t2x,
                Y = t2y,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213458789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            TrackStartCalEventArgs args = new TrackStartCalEventArgs(prevtrack, newTrack);
            _updater.TrackStartCal += Raise.EventWith(this, args);

            Assert.That(_calculatedTrack.Velocity, Is.EqualTo(result1));
            Assert.That(_calculatedTrack.Course, Is.EqualTo(result2));
        }
    }
}
