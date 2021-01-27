using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Model_RaceTime_BestParticipantShould
    {
        private DataRepository<RaceTime> _repo;

        [SetUp]
        public void Setup()
        {
            _repo = new DataRepository<RaceTime>();
            _repo.AddValue(new RaceTime() {Name = "A", Time = TimeSpan.FromSeconds(2)});
            _repo.AddValue(new RaceTime() {Name = "B", Time = TimeSpan.FromSeconds(3)});
            _repo.AddValue(new RaceTime() {Name = "C", Time = TimeSpan.FromSeconds(1)});
        }

        [Test]
        public void ReturnNameOfParticipantWithLowestTimeSpan()
        {
            var naam = _repo.BestParticipant();
            Assert.AreEqual("C", naam);
        }
    }
}