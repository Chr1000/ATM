using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM;
using NUnit.Framework;
using TransponderReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ATM.Test.Unit
{
    [TestFixture]

    public class TrackParserUnitTest
    {
        private RawTransponderDataEventArgs _dummyTransponderData;
        private List<Track> receivedTrackData;

        [SetUp]
        public void Setup()
        {
            _dummyTransponderData = new RawTransponderDataEventArgs(new List<string>()
            { "SKK444;12345;67890;10000;20181004123456789" }
            
            );
        }

        [Test]
        public void TrackParserTestOfTag_TagIsCorrect()
        {
            string correctTag = "SKK444";
            NUnit.Framework.Assert.That(receivedTrackData[0].Tag, Is.EqualTo(correctTag));
        }

        [Test]
        public void TrackParserTestOfX_XIsCorrect()
        {
            int correctX = 12345;
            NUnit.Framework.Assert.That(receivedTrackData[0].X, Is.EqualTo(correctX));
        }

        [Test]
        public void TrackParserTestOfY_YIsCorrect()
        {
            int correctY = 67890;
            NUnit.Framework.Assert.That(receivedTrackData[0].Y, Is.EqualTo(correctY));
        }

        [Test]
        public void TrackParserTestOfAtlitude_AltitudeIsCorrect()
        {
            int correctAltitude = 10000;
            NUnit.Framework.Assert.That(receivedTrackData[0].Altitude, Is.EqualTo(correctAltitude));
        }

        [Test]
        public void TrackParserTestOfTimeStamp_TimeStampIsCorrect()
        {
            DateTime correctTimeStamp = DateTime.ParseExact("20181004123456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            NUnit.Framework.Assert.That(receivedTrackData[0].TimeStamp, Is.EqualTo(correctTimeStamp));
        }


    }
}
