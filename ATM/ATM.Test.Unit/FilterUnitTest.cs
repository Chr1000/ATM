using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM;
using ATM.Interfaces;
using NUnit.Framework;
using TransponderReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Assert = NUnit.Framework.Assert;

namespace ATM.Test.Unit
{
    [TestFixture]
    public class FilterUnitTest
    {
        private IParser _parser;
        private IFilter _filter;
        private IAirspace _airspace;
        private IUpdater _updater;

        private Track _track;
        private Track _trackLeft;
        private List<Track> _tracksFilteredList;
        private int _nEventsReceived;

        [SetUp]
        public void SetUp()
        {
            _parser = Substitute.For<IParser>();
            _filter = new Filter(_parser);
            _airspace = new Airspace(_filter);
            _updater = new Updater(_filter);

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
                _trackLeft = args.Track;
                _nEventsReceived++;
            };
        }

        [Test]
        public void Initial_IsTrackInAirspaceOneTrack_TracksIsCorrect()
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

            Assert.That(_track, Is.EqualTo(track));
        }

        [Test]
        [TestCase(10000, 10000, 20000, true)]
        [TestCase(40000, 40000, 20000, true)]
        [TestCase(50000, 50000, 500, true)]
        [TestCase(90000, 90000, 500, true)]
        //X too low
        [TestCase(9999, 90000, 20000, false)]
        //Y too low
        [TestCase(10000, 9999, 500, false)]
        //X and Y too low
        [TestCase(9999, 9999, 500, false)]
        public void Initial_TracksFilteredOneTrack_TrackIsCorrect(int x, int y, int alt, bool result)
        {
            List<Track> listToTest = new List<Track>();
            Track track = new Track()
            {
                Tag = "NIC111",
                X = x,
                Y = y,
                Altitude = alt,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)

            };
            listToTest.Add(track);
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(args);

            Assert.That(_tracksFilteredList.Contains(track), Is.EqualTo(result));
        }

        [Test]
        public void Initial_TracksFilteredFiveTracks_TracksCountIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            _filter = new Filter(_parser);
            listToTest.Add(new Track()
            {
                Tag = "NIC111", X = 25000, Y = 65000, Altitude = 15000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            });
            listToTest.Add(new Track()
            {
                Tag = "GJO222", X = 35000, Y = 55000, Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            });
            listToTest.Add(new Track()
            {
                Tag = "ASD333", X = 45000, Y = 45000, Altitude = 7000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            });
            listToTest.Add(new Track()
            {
                Tag = "ASD444", X = 55000, Y = 8000, Altitude = 9000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            });
            listToTest.Add(new Track()
            {
                Tag = "NIP111", X = 5000, Y = 25000, Altitude = 3000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            });
            TracksChangedEventArgs args = new TracksChangedEventArgs(listToTest);
            _parser.TracksChanged += Raise.EventWith(args);

            Assert.That(_tracksFilteredList.Count, Is.EqualTo(3));
        }

        [Test]
        public void Initial_TracksLeftOneTrack_TrackLeftIsCorrect()
        {
            List<Track> listToTest = new List<Track>();
            Track track = new Track()
            {
                Tag = "NIC888",
                X = 25000,
                Y = 10005,
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
                Y = 9990,
                Altitude = 4980,
                TimeStamp = DateTime.ParseExact("20151006213456800", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            listToTest2.Add(newTrack);
            TracksChangedEventArgs args2 = new TracksChangedEventArgs(listToTest2);
            _parser.TracksChanged += Raise.EventWith(this, args2);

            Assert.That(_trackLeft.Tag, Is.EqualTo(track.Tag));
        }
    }
}
