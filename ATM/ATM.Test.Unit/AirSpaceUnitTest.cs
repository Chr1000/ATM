using System;
using System.Collections.Generic;
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

    public class AirSpaceUnitTest
    {
        private ATM.AirSpace _uut;
        private ATM.Track _t;

        [SetUp]
        public void Setup()
        {
            _uut = new ATM.AirSpace();
        }

        //Testcases all within our airspace boundaries. 
        [TestCase(10000, 10000, 20000)]
        [TestCase(40000, 40000, 20000)]
        [TestCase(50000, 50000, 500)]
        [TestCase(90000, 90000, 500)]
        public void TrackInsideAirSpace_ReturnTrue(int x, int y, int alt)
        {
            _t = new Track() { X = x, Y = y, Altitude = alt };

            NUnit.Framework.Assert.That(_uut.WithinAirSpace(_t), Is.EqualTo(true));

        }

        //X too low
        [TestCase(9999, 90000, 20000)]
        //Y too low
        [TestCase(10000, 9999, 500)]
        //Y too high
        [TestCase(10000, 90001, 10000)]
        //X too high
        [TestCase(90001, 10000, 500)]
        //Alt too low
        [TestCase(90000, 10000, 499)]
        //Alt too high
        [TestCase(10000, 10000, 20001)]
        public void TrackOutsideAirSpace_ReturnFalse(int x, int y, int alt)
        {
            _t = new Track() { X = x, Y = y, Altitude = alt };

            NUnit.Framework.Assert.That(_uut.WithinAirSpace(_t), Is.EqualTo(false));
        }
    }
}
