using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Controller_Race_MoveDriversShould
    {
        private Race _race;

        [SetUp]
        public void Setup()
        {
            var track = new Track("test", new SectionTypes[] {
                SectionTypes.StartGrid, 
                SectionTypes.StartGrid,
                SectionTypes.Finish, 
                SectionTypes.RightCorner, 
                SectionTypes.RightCorner,
                SectionTypes.Straight, 
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            });
            _race = new Race(track, new List<IParticipant>()
            {
                new Driver(){Name = "test", Equipment = new Car()},
                new Driver(){Name = "2test", Equipment = new Car()},
                new Driver(){Name = "3test", Equipment = new Car()}
            });
        }

        [Test]
        public void MoveDrivers_MoveToNextSectionWhenDistanceTravelled()
        {
            _race.Participants.RemoveAt(1);
            _race.Participants[0].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = 30;
            Assert.AreEqual(false, _race.MoveDrivers());
            Assert.AreEqual(true, _race.MoveDrivers());
        }

        [Test]
        public void MoveDrivers_DistanceTravelledEqualsSpeedTimesPerformance()
        {
            _race.Participants[0].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = 30;
            _race.Participants[1].Equipment.Performance = 2;
            _race.Participants[1].Equipment.Speed = 15;
            var data = _race.GetSectionData(_race.Track.Sections.First.Next.Value);
            Assert.AreEqual(0, data.DistanceLeft);
            Assert.AreEqual(0, data.DistanceRight);
            _race.MoveDrivers();
            data = _race.GetSectionData(_race.Track.Sections.First.Next.Value);
            Assert.AreEqual(30, data.DistanceLeft);
            Assert.AreEqual(30, data.DistanceRight);
        }

        [Test]
        public void MoveDrivers_ShouldTryToOvertake()
        {
            _race.Participants[0].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = 0;
            _race.Participants[1].Equipment.Performance = 1;
            _race.Participants[1].Equipment.Speed = 30;
            _race.Participants[2].Equipment.Performance = 1;
            _race.Participants[2].Equipment.Speed = 30;
            //Verify that participant is on the left side of the track
            var data = _race.GetSectionData(_race.Track.Sections.First.Value);
            Assert.AreEqual(_race.Participants[2], data.Left);
            _race.MoveDrivers();
            //Verify that participant is still on left side.
            data = _race.GetSectionData(_race.Track.Sections.First.Value);
            Assert.AreEqual(_race.Participants[2], data.Left);
            _race.MoveDrivers();
            //Once right driver in next section has moved, the participant is able to overtake
            data = _race.GetSectionData(_race.Track.Sections.First.Next.Value);
            Assert.AreEqual(_race.Participants[2], data.Right);
        }

        [Test]
        public void MoveDrivers_ShouldCapDistanceWhenUnableToOvertake()
        {
            _race.Participants[0].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = 0;
            _race.Participants[1].Equipment.Performance = 1;
            _race.Participants[1].Equipment.Speed = 0;
            _race.Participants[2].Equipment.Performance = 1;
            _race.Participants[2].Equipment.Speed = 30;
            //Verify that participant is on the left side of the track
            var data = _race.GetSectionData(_race.Track.Sections.First.Value);
            Assert.AreEqual(_race.Participants[2], data.Left);
            _race.MoveDrivers();
            //Verify that participant is still on left side.
            data = _race.GetSectionData(_race.Track.Sections.First.Value);
            Assert.AreEqual(_race.Participants[2], data.Left);
            _race.MoveDrivers();
            //Check if distance has been set to cap since participant couldnt overtake
            data = _race.GetSectionData(_race.Track.Sections.First.Value);
            Assert.AreEqual(50, data.DistanceLeft);
        }

        [Test]
        public void MoveDrivers_ShouldWrapAround()
        {
            _race.Participants[0].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = 30;
            _race.Participants[1].Equipment.Performance = 1;
            _race.Participants[1].Equipment.Speed = 30;
            _race.Participants[2].Equipment.Performance = 1;
            _race.Participants[2].Equipment.Speed = 30;
            var data = _race.GetSectionData(_race.Track.Sections.Last.Value);
            data.Left = _race.Participants[0];
            _race.MoveDrivers();
            Assert.AreEqual(30, data.DistanceLeft);
            _race.MoveDrivers();
            data = _race.GetSectionData(_race.Track.Sections.First.Value);
            Assert.AreEqual(_race.Participants[0], data.Right);
        }
    }
}
