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

    public class TrackParserUnitTest
    {
        private RawTransponderDataEventArgs _incorrectTransponderData;
        private List<Track> receivedTrackData;

        [SetUp]
        public void Setup()
        {
            _incorrectTransponderData = new RawTransponderDataEventArgs(new List<string>()
            { "SKK444;12345;67890;10000;20181004123456789" }
            
            );
        }
        public void TrackParserTestOfTag_TagIsCorrect()
        {
            NUnit.Framework.Assert.That(receivedTrackData[0].Tag, Is.EqualTo("SKK444"));
        }
    }
}
