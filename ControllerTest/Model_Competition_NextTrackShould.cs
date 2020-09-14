using System.Collections.Generic;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void Setup()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var track = new Track("Test", new LinkedList<Section>());
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            var track = new Track("Test", new LinkedList<Section>());
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            var spa = new Track("Spa", new LinkedList<Section>());
            var assen = new Track("Assen", new LinkedList<Section>()); 
            _competition.Tracks.Enqueue(spa);
            _competition.Tracks.Enqueue(assen);
            Assert.AreEqual(spa, _competition.NextTrack());
            Assert.AreEqual(assen, _competition.NextTrack());
        }
    }
}
