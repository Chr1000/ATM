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

        [TestCase("NIC007", 27272, 67676, 14141)]
        [TestCase("CBN007", 25252, 66666, 11111)]
        [TestCase("BON007", 26262, 68545, 12458)]
        public void Initial_TrackUpdatedOneTrack_StringIsCorrect(string tag, int x, int y, int alt)
        {       
            List<Track> updatedTracklistToTest = new List<Track>();
            List<IEvent> evnetList = new List<IEvent>();
            updatedTracklistToTest.Add(new Track() { Tag = tag, X = x, Y = y, Altitude = alt, Velocity = 10, Course = 45, TimeStamp = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) });

            TracksUpdatedEventArgs args = new TracksUpdatedEventArgs(updatedTracklistToTest, evnetList);
            _updater.TracksUpdated += Raise.EventWith(args);

            bool isInList = false;
            for (int i = 0; i < _outputList.Count; i++)
            {
                if (_outputList[i].Contains(tag))
                {
                    isInList = true;
                }
            }

            Assert.That(isInList, Is.EqualTo(true));
        }
    }
}
