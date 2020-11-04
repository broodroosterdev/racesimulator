using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Model_Competition_GivePointsShould
    {
        private Competition _competition;

        [SetUp]
        public void Setup()
        {
            _competition = new Competition();
            _competition.RaceTimes.AddValue(new RaceTime(){Name = "A", Time = TimeSpan.FromSeconds(4)});
            _competition.RaceTimes.AddValue(new RaceTime() { Name = "B", Time = TimeSpan.FromSeconds(3) });
            _competition.RaceTimes.AddValue(new RaceTime() { Name = "C", Time = TimeSpan.FromSeconds(2) });
            _competition.RaceTimes.AddValue(new RaceTime() { Name = "D", Time = TimeSpan.FromSeconds(1) });
        }

        [Test]
        public void GiveFastestParticipant20Points()
        {
            _competition.GivePoints();
            DriverPoints fastestPoints = _competition.Points.GetData().Where((e) => e.Name == "D").ToList()[0] as DriverPoints;
            Assert.AreEqual(20, fastestPoints.Points);
            
        }

        [Test]
        public void DecreasePointsBy1ForEachParticipant()
        {
            _competition.GivePoints();
            var points = _competition.Points.GetData().Select(e => (DriverPoints)e);
            Func<string, int> getPoints = name => points.Where(e => e.Name == name).ToList()[0].Points;
            Assert.AreEqual(20, getPoints("D"));
            Assert.AreEqual(19, getPoints("C"));
            Assert.AreEqual(18, getPoints("B"));
            Assert.AreEqual(17, getPoints("A"));
        }

    }
}
