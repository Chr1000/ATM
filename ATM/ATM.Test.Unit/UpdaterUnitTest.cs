using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM;
using ATM.Core.Interfaces;
using ATM.Interfaces;
using NUnit.Framework;
using TransponderReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Assert = NUnit.Framework.Assert;

namespace ATM.Test.Unit
{
    [TestFixture]
    public class UpdaterUnitTest
    {
        private IFilter _filter;
        private IUpdater _updater;

        private Track _sepTrack;

        private Track _newTrackStartCal;
        private Track _prevTrackStartCal;

        private IEvent _eventEntered;
        private IEvent _eventLefted;
        private List<Track> _tracksUpdatedeInSeperationList;
        private List<Track> _updatedTrackList;

        private int _nEventsReceived;

        [SetUp]
        public void Setup()
        {
            _filter = Substitute.For<IFilter>();
            _updater = new Updater(_filter);


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
        public void Initial_TracksUpdatedOneTracks_TrackIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            listToTest.Add(new Track() { Tag = "NIC111", X = 25000, Y = 65000, Altitude = 15000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            TracksFilteredEventArgs args = new TracksFilteredEventArgs(listToTest);
            _filter.TracksFiltered += Raise.EventWith(this, args);

            List<Track> listToTest2 = new List<Track>();
            listToTest2.Add(new Track() { Tag = "NIC111", X = 25010, Y = 64987, Altitude = 14998, TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            TracksFilteredEventArgs args2 = new TracksFilteredEventArgs(listToTest2);
            _filter.TracksFiltered += Raise.EventWith(this, args2);

            Assert.That(_updatedTrackList[0].Tag, Is.EqualTo("NIC111"));
        }

        [Test]
        public void Initial_TracksUpdatedThreeTracks_TracksCountIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            listToTest.Add(new Track() { Tag = "NIC111", X = 25000, Y = 65000, Altitude = 15000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "GJO222", X = 35000, Y = 55000, Altitude = 5000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest.Add(new Track() { Tag = "ASD333", X = 45000, Y = 45000, Altitude = 7000, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            TracksFilteredEventArgs args = new TracksFilteredEventArgs(listToTest);
            _filter.TracksFiltered += Raise.EventWith(this, args);

            List<Track> listToTest2 = new List<Track>();
            listToTest2.Add(new Track() { Tag = "NIC111", X = 25010, Y = 64987, Altitude = 14998, TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest2.Add(new Track() { Tag = "GJO222", X = 35005, Y = 55015, Altitude = 5010, TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            listToTest2.Add(new Track() { Tag = "ASD333", X = 45008, Y = 44994, Altitude = 7002, TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });
            TracksFilteredEventArgs args2 = new TracksFilteredEventArgs(listToTest2);
            _filter.TracksFiltered += Raise.EventWith(this, args2);

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
            TracksFilteredEventArgs args = new TracksFilteredEventArgs(listToTest);
            _filter.TracksFiltered += Raise.EventWith(this, args);

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
            TracksFilteredEventArgs args2 = new TracksFilteredEventArgs(listToTest2);
            _filter.TracksFiltered += Raise.EventWith(this, args2);

            Assert.That(_prevTrackStartCal, Is.EqualTo(track));
            Assert.That(_newTrackStartCal, Is.EqualTo(newTrack));
        }

        [Test]
        public void Initial_TrackLeftedOneTrack_TrackInEventIsCorrect()
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
            TracksFilteredEventArgs args = new TracksFilteredEventArgs(listToTest);
            _filter.TracksFiltered += Raise.EventWith(this, args);

            List<Track> listToTest2 = new List<Track>();
            Track newTrack = new Track()
            {
                Tag = "NIC333",
                X = 25010,
                Y = 10000,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest2.Add(newTrack);
            TracksFilteredEventArgs args2 = new TracksFilteredEventArgs(listToTest2);
            _filter.TracksFiltered += Raise.EventWith(this, args2);
            Track trackOutOfSpace = new Track()
            {
                Tag = "NIC333",
                X = 25010,
                Y = 9980,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            TrackLeftAirspaceEventArgs args3 = new TrackLeftAirspaceEventArgs(trackOutOfSpace);
            _filter.TrackLeft += Raise.EventWith(args3);

            Assert.That(_eventLefted.Track.Tag, Is.EqualTo(track.Tag));
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
            TracksFilteredEventArgs args = new TracksFilteredEventArgs(listToTest);
            _filter.TracksFiltered += Raise.EventWith(this, args);

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
            TracksFilteredEventArgs args2 = new TracksFilteredEventArgs(listToTest2);
            _filter.TracksFiltered += Raise.EventWith(this, args2);

            Assert.That(_sepTrack.Tag, Is.EqualTo(newTrack1.Tag));
            Assert.That(_tracksUpdatedeInSeperationList[1].Tag, Is.EqualTo(newTrack2.Tag));
        }
    }
}
