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

namespace ATM.Test.Unit
{
    [TestFixture]

    public class TrackParserUnitTest
    {
        private Parser UUT;
        private string unparsedStringForTest;
        private Track parsedTrack;

        [SetUp]
        public void Setup()
        {
         UUT = new Parser(new FakeTransponderReciever());
         unparsedStringForTest = "SKK444;12345;67890;10000;20181004123456789";
         parsedTrack = UUT.ReadTrackData(unparsedStringForTest);
        }

        [Test]
        public void TrackParserTestOfTag_TagIsCorrect()
        {
            string correctTag = "SKK444";
            NUnit.Framework.Assert.That(parsedTrack.Tag, Is.EqualTo(correctTag));
        }

        [Test]
        public void TrackParserTestOfX_XIsCorrect()
        {
            int correctX = 12345;
            NUnit.Framework.Assert.That(parsedTrack.X, Is.EqualTo(correctX));
        }

        [Test]
        public void TrackParserTestOfY_YIsCorrect()
        {
            int correctY = 67890;
            NUnit.Framework.Assert.That(parsedTrack.Y, Is.EqualTo(correctY));
        }

        [Test]
        public void TrackParserTestOfAtlitude_AltitudeIsCorrect()
        {
            int correctAltitude = 10000;
            NUnit.Framework.Assert.That(parsedTrack.Altitude, Is.EqualTo(correctAltitude));
        }

        [Test]
        public void TrackParserTestOfTimeStamp_TimeStampIsCorrect()
        {
            DateTime correctTimeStamp = DateTime.ParseExact("20181004123456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            NUnit.Framework.Assert.That(parsedTrack.TimeStamp, Is.EqualTo(correctTimeStamp));
        }


    }
}
