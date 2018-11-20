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
    public class RenderUnitTest
    {
        private IUpdater _updater;
        private IRender _render;

        private List<string> _outputList;

        [SetUp]
        public void Setup()
        {
            _updater = Substitute.For<IUpdater>();
            _render = new Render(_updater);

            _outputList = new List<string>();

            _render.WriteLine += (o, args) => { _outputList.Add(args.Output); };

        }

        [Test]
        public void Initial_TrackUpdatedOneTrack_StringIsCorrect()
        {
            List<Track> updatedTracklistToTest = new List<Track>();
            List<IEvent> evnetList = new List<IEvent>();
            updatedTracklistToTest.Add(new Track() { Tag = "NIC111", X = 25000, Y = 65000, Altitude = 15000, Velocity = 10, Course = 45, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });

            TracksUpdatedEventArgs args = new TracksUpdatedEventArgs(updatedTracklistToTest, evnetList);
            _updater.TracksUpdated += Raise.EventWith(args);

            bool isInList = false;
            for (int i = 0; i < _outputList.Count; i++)
            {
                if (_outputList[i].Contains("NIC111"))
                {
                    isInList = true;
                }
            }

            Assert.That(isInList, Is.EqualTo(true));
        }
    }
}
