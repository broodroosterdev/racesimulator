using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Timers;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Controller_Race_StartShould
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
        public void EnableTimer()
        {
            _race.Start();
            var timer = Helper.GetPrivate<Timer>(_race, "_timer"); 
            Assert.IsTrue(timer.Enabled);
        }

        [Test]
        public void SetStartTime()
        {
            _race.Start();
            var time = Helper.GetPrivate<DateTime>(_race, "_startTime");
            Assert.IsNotNull(time);
        }
    }
}
