using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Controller_Race_CleanupShould
    {
        private Race _race;

        [SetUp]
        public void Setup()
        {
            var track = new Track("test", new SectionTypes[]
            {
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
                new Driver() {Name = "test", Equipment = new Car()},
                new Driver() {Name = "2test", Equipment = new Car()},
                new Driver() {Name = "3test", Equipment = new Car()}
            });
        }

        [Test]
        public void DisableTimer()
        {
            _race.Start();
            _race.CleanUp();
            var timer = Helper.GetPrivate<Timer>(_race, "_timer");
            Assert.IsFalse(timer.Enabled);
        }

        [Test]
        public void RemoveListeners()
        {
            //Add fake events
            _race.DriversChanged += (i, j) => { };
            _race.RaceEnded += (i) => { };
            _race.CleanUp();
            var driversChangedEvent = Helper.GetPrivate<Race.DriversChangedEvent>(_race, "DriversChanged");
            Assert.IsNull(driversChangedEvent);
        }
    }
}