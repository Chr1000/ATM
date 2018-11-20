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
        //private ICalculator _calculator;
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
        public void TestMethod1()
        {
        }
    }
}
