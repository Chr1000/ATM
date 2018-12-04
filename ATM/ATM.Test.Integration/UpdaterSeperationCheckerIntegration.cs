using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ATM.Core.Interfaces;
using ATM.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NSubstitute;
using Assert = NUnit.Framework.Assert;
using System.Threading;
using ATM.Core;

namespace ATM.Test.Integration
{
    [TestFixture]
    class UpdaterSeperationCheckerIntegration
    {

        private IUpdater _updater;
        private ISeperationChecker _seperationChecker;

        //For Raising fake events
        private List<Track> _activeTracks;
        private List<IEvent> _activeEvents;
        //For checking output
        private IEvent _raisedEvent;

        [SetUp]

        public void SetUp()
        {
            _updater = Substitute.For<IUpdater>();
            _seperationChecker = new SeperationChecker(_updater);
            _activeTracks = new List<Track>();
            _activeEvents = new List<IEvent>();

            _seperationChecker.SeperationAlert += (o, args) =>
            {
                _raisedEvent = args.Event;
            };

            _seperationChecker.SeperationStop += (o, args) =>
            {
                _raisedEvent = args.Event;
            };
        }

        [Test]
        public void SeperationEvent_is_raised_on_detection()
        {

            Track track1 = new Track()
            {
                Tag = "JHG654",
                X = 30000,
                Y = 30000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            _activeTracks.Add(track1);

            Track track2 = new Track()
            {
                Tag = "NIC888",
                X = 30000,
                Y = 30000,
                Altitude = 4800,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            _activeTracks.Add(track2);

            SeperationCheckerEventArgs SCEargs = new SeperationCheckerEventArgs(_activeEvents, _activeTracks, track1);

            _updater.SeperationChecker += Raise.EventWith(this, SCEargs);

            Assert.That(_raisedEvent.Track, Is.EqualTo(track2));
            Assert.That(_raisedEvent.ConflictingTrack, Is.EqualTo(track1));
        }

        [Test]
        public void SeperationStopEvent_is_raised_on_event1()
        {

            Track track1 = new Track()
            {
                Tag = "JHG654",
                X = 30000,
                Y = 30000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            _activeTracks.Add(track1);

            Track track2 = new Track()
            {
                Tag = "NIC888",
                X = 30000,
                Y = 30000,
                Altitude = 4800,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };

            Track track2updated = new Track()
            {
                Tag = "NIC888",
                X = 30000,
                Y = 30000,
                Altitude = 4000,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            _activeTracks.Add(track2updated);

            IEvent alertEvent = new Event(_activeEvents, "SEPERATION ALERT", track2, DateTime.Now, track1, _seperationChecker);
            _activeEvents.Add(alertEvent);

            SeperationCheckerEventArgs SCEargs = new SeperationCheckerEventArgs(_activeEvents, _activeTracks, track2updated);

            _updater.SeperationChecker += Raise.EventWith(this, SCEargs);

            Assert.That(_raisedEvent.Track, Is.EqualTo(track2));
            Assert.That(_raisedEvent.ConflictingTrack, Is.EqualTo(track1));
        }

        [Test]
        public void SeperationStopEvent_is_raised_on_event2()
        {

            Track track1 = new Track()
            {
                Tag = "JHG654",
                X = 30000,
                Y = 30000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            _activeTracks.Add(track1);

            Track track2 = new Track()
            {
                Tag = "NIC888",
                X = 30000,
                Y = 30000,
                Altitude = 4800,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };

            Track track2updated = new Track()
            {
                Tag = "NIC888",
                X = 30000,
                Y = 30000,
                Altitude = 4000,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            _activeTracks.Add(track2updated);

            IEvent alertEvent = new Event(_activeEvents, "SEPERATION ALERT", track1, DateTime.Now, track2, _seperationChecker);
            _activeEvents.Add(alertEvent);

            SeperationCheckerEventArgs SCEargs = new SeperationCheckerEventArgs(_activeEvents, _activeTracks, track2updated);

            _updater.SeperationChecker += Raise.EventWith(this, SCEargs);

            Assert.That(_raisedEvent.Track, Is.EqualTo(track1));
            Assert.That(_raisedEvent.ConflictingTrack, Is.EqualTo(track2));
        }
    }
}