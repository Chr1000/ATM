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
    public class AirSpaceUnitTest
    {
        private IFilter _filter;
        private IAirspace _airspace;
        private Track _track;
        private bool _trackIsInAirspace;

        private int _nEventsReceived;

        [SetUp]
        public void Setup()
        {
            _filter = Substitute.For<IFilter>();
            _airspace = new Airspace(_filter);

            _filter.IsTrackInAirspace += (o, args) =>
            {
                _track = args.Track;
                _trackIsInAirspace = args.IsInAirspace;
                _nEventsReceived++;
            };
        }

        [TestCase(10000, 10000, 20000)]
        [TestCase(40000, 40000, 20000)]
        [TestCase(50000, 50000, 500)]
        [TestCase(90000, 90000, 500)]
        public void Initial_IsTrackInAirspaceOneTrack_TracksIsInAirspaceIsTrue(int x, int y, int alt)
        {
            Track track = new Track() { Tag = "NIC111", X = x, Y = y, Altitude = alt, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) };
            IsTrackInAirspaceEventArgs args = new IsTrackInAirspaceEventArgs(track);
          
            _filter.IsTrackInAirspace += Raise.EventWith(args);

            Assert.That(_trackIsInAirspace, Is.EqualTo(true));
        }

        //X too low
        [TestCase(9999, 90000, 20000)]
        //Y too low
        [TestCase(10000, 9999, 500)]
        //X and Y too low
        [TestCase(9999, 9999, 500)]
        //Y too high
        [TestCase(10000, 90001, 10000)]
        //X too high
        [TestCase(90001, 10000, 500)]
        //X and Y too high
        [TestCase(90001, 100000, 500)]
        //Alt too low
        [TestCase(90000, 10000, 499)]
        //Alt too high
        [TestCase(10000, 10000, 20001)]
        public void Initial_IsTrackInAirspaceOneTrack_TracksIsInAirspaceIsFalse(int x, int y, int alt)
        {
            Track track = new Track() { Tag = "NIC111", X = x, Y = y, Altitude = alt, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) };
            IsTrackInAirspaceEventArgs args = new IsTrackInAirspaceEventArgs(track);

            _filter.IsTrackInAirspace += Raise.EventWith(args);

            Assert.That(_trackIsInAirspace, Is.EqualTo(false));
        }

        //Testcases all within our airspace boundaries.
        //[TestCase(10000, 10000, 20000)]
        //[TestCase(40000, 40000, 20000)]
        //[TestCase(50000, 50000, 500)]
        //[TestCase(90000, 90000, 500)]
        //public void TrackInsideAirSpace_ReturnTrue(int x, int y, int alt)
        //{
        //    _t = new Track() { X = x, Y = y, Altitude = alt };

        //    NUnit.Framework.Assert.That(_uut.IsTrackInAirspace(_t), Is.EqualTo(true));

        //}

        //X too low
        //[TestCase(9999, 90000, 20000)]
        //Y too low
        //[TestCase(10000, 9999, 500)]
        //X and Y too low
        //[TestCase(9999, 9999, 500)]
        //Y too high
        //[TestCase(10000, 90001, 10000)]
        //X too high
        //[TestCase(90001, 10000, 500)]
        //X and Y too high
        //[TestCase(90001, 100000, 500)]
        //Alt too low
        //[TestCase(90000, 10000, 499)]
        //Alt too high
        //[TestCase(10000, 10000, 20001)]
        //public void TrackOutsideAirSpace_ReturnFalse(int x, int y, int alt)
        //{
        //    _t = new Track() { X = x, Y = y, Altitude = alt };

        //    NUnit.Framework.Assert.That(_uut.IsTrackInAirspace(_t), Is.EqualTo(false));
        //}
    }
}
