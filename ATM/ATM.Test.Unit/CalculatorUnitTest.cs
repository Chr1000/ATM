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
  
        [Test]
        public void Initial_TrackStartCalcOneTrack_VelocityAndCourseIsCorrect()
        {
            Track prevtrack = new Track()
            {
                Tag = "NIC222",
                X = 25000,
                Y = 25000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };


            Track newTrack = new Track()
            {
                Tag = "NIC222",
                X = 25010,
                Y = 25010,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213458789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            TrackStartCalEventArgs args = new TrackStartCalEventArgs(prevtrack, newTrack);
            _updater.TrackStartCal += Raise.EventWith(this, args);

            Assert.That(_calculatedTrack.Velocity, Is.EqualTo(7.0710678118654755));
            Assert.That(_calculatedTrack.Course, Is.EqualTo(45));
        }
    }
}
