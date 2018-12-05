using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM;
using ATM.Core;
using ATM.Interfaces;
using NUnit.Framework;
using TransponderReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace ATM.Test.Unit
{
    [TestFixture]
    class SeperationCheckerUnitTest
    {
        private SeperationChecker UUT;
        private Track track1;
        private Track track2;

        [SetUp]

        public void SetUp()
        {
            UUT = new SeperationChecker(new FakeUpdater());
        }

        [Test]
        public void SeperationCheckerTest_Of_CheckForSeperation_SeperationDetected()
        {
            track1 = new Track()
            {
                Tag = "JHG654",
                X = 30000,
                Y = 30000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            track2 = new Track()
            {
                Tag = "JKL654",
                X = 30000,
                Y = 30000,
                Altitude = 4800,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };

            NUnit.Framework.Assert.That(UUT.CheckForSeparation(track1, track2), Is.EqualTo(true));
        }

        [Test]
        public void SeperationCheckerTest_Of_CheckForSeperation_NoSeperationDetected_altDiff()
        {
            track1 = new Track()
            {
                Tag = "JHG654",
                X = 30000,
                Y = 30000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            track2 = new Track()
            {
                Tag = "JKL654",
                X = 30000,
                Y = 30000,
                Altitude = 4500,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };

            NUnit.Framework.Assert.That(UUT.CheckForSeparation(track1, track2), Is.EqualTo(false));
        }

        [Test]
        public void SeperationCheckerTest_Of_CheckForSeperation_NoSeperationDetected_horizontalDiff()
        {
            track1 = new Track()
            {
                Tag = "JHG654",
                X = 30000,
                Y = 20000,
                Altitude = 5000,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };
            track2 = new Track()
            {
                Tag = "JKL654",
                X = 30000,
                Y = 30000,
                Altitude = 4800,
                TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)
            };

            NUnit.Framework.Assert.That(UUT.CheckForSeparation(track1, track2), Is.EqualTo(false));
        }



    }
}
