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

namespace ATM.Test.Unit
{
    /// <summary>
    /// Summary description for UpdaterUnitTest
    /// </summary>
    [TestFixture]
    public class UpdaterUnitTest
    {
        private IParser _parser;
        private IAirspace _airspace;
        private IFilter _filter;
        private ICalculator _calculator;
        private IUpdater _updater;

        private Track _track;
        private Track _track2;
        private Track _sepTrack;

        private Track _newTrackStartCal;
        private Track _prevTrackStartCal;

        private IEvent _eventEntered;
        private IEvent _eventLefted;
        private List<Track> _tracksFilteredList;
        private List<Track> _tracksUpdatedeInSeperationList;
        private List<Track> _updatedTrackList;

        private int _nEventsReceived;

        [SetUp]
        public void SetUp()
        {
            _track = new Track();

            _tracksFilteredList = new List<Track>();
            _updatedTrackList = new List<Track>();

            _parser = Substitute.For<IParser>();
            _filter = new Filter(_parser);
            _airspace = new Airspace(_filter);
            _updater = new Updater(_filter);
            _calculator = new Calculator(_updater);

            _filter.IsTrackInAirspace += (o, args) =>
            {
                _track = args.Track;
                _nEventsReceived++;
            };

            _filter.TracksFiltered += (o, args) =>
            {
                _tracksFilteredList = args.FilteredTracks;
                _nEventsReceived++;
            };

            _filter.TrackLeft += (o, args) =>
            {
                _nEventsReceived++;
            };

            _updater.SeperationChecker += (o, args) =>
            {
                _sepTrack = args.Track;
                _tracksUpdatedeInSeperationList = args.UpdatedTracks;
                _nEventsReceived++;
            };

            _updater.TrackEntered += (o, args) =>
            {
                _eventEntered = args.Event;
                _nEventsReceived++;
            };

            _updater.TrackLefted += (o, args) =>
            {
                _eventLefted = args.Event;
                _nEventsReceived++;
            };

            _updater.TrackStartCal += (o, args) =>
            {
                _newTrackStartCal = args.NewTrack;
                _prevTrackStartCal = args.PrevTrack;
                _nEventsReceived++;
            };

            _updater.TracksUpdated += (o, args) =>
            {
                _updatedTrackList = args.UpdatedTracks;
                _nEventsReceived++;
            };

        }

        [Test]
        public void Initial_IsTrackInAirspaceOneTrack_TracksIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            Track track = new Track(){Tag = "NIC111", X = 25000, Y = 25000, Altitude = 5000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture  ) };
            listToTest.Add(track);
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(args);

            Assert.That(_track, Is.EqualTo(track));
        }

        [Test]
        public void Initial_TracksFilteredOneTrack_TrackIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            Track track = new Track()
            {
                Tag = "NIC111", X = 25000, Y = 25000, Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)

            };
            listToTest.Add(track);
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(args);

            Assert.That(_tracksFilteredList[0], Is.EqualTo(track));
        }

        [Test]
        public void Initial_TracksFilteredFiveTracks_TracksCountIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            _filter = new Filter(_parser);
            listToTest.Add(new Track() { Tag = "NIC111", X = 25000, Y = 65000, Altitude = 15000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "GJO222", X = 35000, Y = 55000, Altitude = 5000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "ASD333", X = 45000, Y = 45000, Altitude = 7000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "ASD444", X = 55000, Y = 8000, Altitude = 9000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "NIP111", X = 5000, Y = 25000, Altitude = 3000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(args);

            Assert.That(_tracksFilteredList.Count, Is.EqualTo(3));
        }

        //TRACKLEFT event er ikke testet da den er testet via TRACKLEFTED evnet. 

        [Test]
        public void Initial_TrackEnteredOneTrack_TrackInEventIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            Track track = new Track()
            {
                Tag = "NIC888",
                X = 25000,
                Y = 25000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest.Add(track);
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(this, args);

            List<Track> listToTest2 = new List<Track>();
            Track newTrack = new Track()
            {
                Tag = "NIC888",
                X = 25010,
                Y = 25002,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest2.Add(newTrack);
            TracksChangedEventArgs args2 = new TracksChangedEventArgs(listToTest2);
            _parser.TracksChanged += Raise.EventWith(this, args2);

            Assert.That(_eventEntered.Track.Tag, Is.EqualTo(newTrack.Tag));
        }

        [Test]
        public void Initial_TracksUpdatedThreeTracks_TracksCountIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            listToTest.Add(new Track() { Tag = "NIC111", X = 25000, Y = 65000, Altitude = 15000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "GJO222", X = 35000, Y = 55000, Altitude = 5000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "ASD333", X = 45000, Y = 45000, Altitude = 7000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(this, args);
    
            List<Track> listToTest2 = new List<Track>();
            listToTest2.Add(new Track() { Tag = "NIC111", X = 25010, Y = 64987, Altitude = 14998, TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest2.Add(new Track() { Tag = "GJO222", X = 35005, Y = 55015, Altitude = 5010, TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest2.Add(new Track() { Tag = "ASD333", X = 45008, Y = 44994, Altitude = 7002, TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            TracksChangedEventArgs args2 = new TracksChangedEventArgs(listToTest2);
            _parser.TracksChanged += Raise.EventWith(this, args2);
         
            Assert.That(_updatedTrackList.Count, Is.EqualTo(3));
        }

        [Test]
        public void Initial_TrackStartCalcOneTrack_TracksIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            Track track = new Track()
            {
                Tag = "NIC222",
                X = 25000,
                Y = 25000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest.Add(track);
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(this, args);

            List<Track> listToTest2 = new List<Track>();
            Track newTrack = new Track()
            {
                Tag = "NIC222",
                X = 25010,
                Y = 25002,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest2.Add(newTrack);
            TracksChangedEventArgs args2 = new TracksChangedEventArgs(listToTest2);
            _parser.TracksChanged += Raise.EventWith(this, args2);

            Assert.That(_prevTrackStartCal, Is.EqualTo(track));
            Assert.That(_newTrackStartCal, Is.EqualTo(newTrack));
        }

        [Test]
        public void Initial_TrackLeftedOneTrack_TrackIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            Track track = new Track()
            {
                Tag = "NIC333",
                X = 25000,
                Y = 10000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest.Add(track);
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(this, args);

            List<Track> listToTest2 = new List<Track>();
            Track newTrack = new Track()
            {
                Tag = "NIC333",
                X = 25010,
                Y = 9990,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest2.Add(newTrack);
            TracksChangedEventArgs args2 = new TracksChangedEventArgs(listToTest2);
            _parser.TracksChanged += Raise.EventWith(this, args2);

            Assert.That(_eventLefted.Track.Tag, Is.EqualTo(newTrack.Tag));
        }

        [Test]
        public void Initial_SeperationCheckerTwoTracks_TracksIsCorrect()
        {

            List<Track> listToTest = new List<Track>();
            Track track = new Track()
            {
                Tag = "NIC444",
                X = 25000,
                Y = 20000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest.Add(track);
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(this, args);

            List<Track> listToTest2 = new List<Track>();
            Track newTrack1 = new Track()
            {
                Tag = "NIC444",
                X = 25010,
                Y = 20020,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            Track newTrack2 = new Track()
            {
                Tag = "GOG123",
                X = 34010,
                Y = 15000,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest2.Add(newTrack1);
            listToTest2.Add(newTrack2);
            TracksChangedEventArgs args2 = new TracksChangedEventArgs(listToTest2);
            _parser.TracksChanged += Raise.EventWith(this, args2);

            Assert.That(_sepTrack.Tag, Is.EqualTo(newTrack1.Tag) );
            Assert.That(_tracksUpdatedeInSeperationList[1].Tag, Is.EqualTo(newTrack2.Tag));
        }
    }
}
