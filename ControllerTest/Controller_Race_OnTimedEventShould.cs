using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Controller_Race_OnTimedEventShould
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
        public void CleanupWhenAllFinished()
        {
            _race.Start();
            _race.Participants = new List<IParticipant>();
            Thread.Sleep(300);
            var timer = Helper.GetPrivate<System.Timers.Timer>(_race, "_timer");
            Assert.IsFalse(timer.Enabled);
        }

        [Test]
        public void MoveDriversWhenNotFinished()
        {
            _race.Start();
            _race.Participants[0].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = 30;
            var data = _race.GetSectionData(_race.Track.Sections.First.Next.Value);
            Assert.AreEqual(0, data.DistanceLeft);
            Thread.Sleep(300);
            data = _race.GetSectionData(_race.Track.Sections.First.Next.Value);
            Assert.AreNotEqual(0, data.DistanceLeft);
        }
    }
}
