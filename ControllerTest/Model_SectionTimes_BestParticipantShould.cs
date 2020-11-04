using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Model_SectionTimes_BestParticipantShould
    {
        private DataRepository<SectionTime> _repo;
        [SetUp]
        public void Setup()
        {
            _repo = new DataRepository<SectionTime>();
            _repo.AddValue(new SectionTime() {Name = "A", Time = TimeSpan.FromSeconds(1)});
            _repo.AddValue(new SectionTime() { Name = "A", Time = TimeSpan.FromSeconds(4) });
            _repo.AddValue(new SectionTime() { Name = "B", Time = TimeSpan.FromSeconds(1) });
            _repo.AddValue(new SectionTime() { Name = "B", Time = TimeSpan.FromSeconds(2) });
        }

        [Test]
        public void ReturnNameOfParticipantWithLowestAverageTimes()
        {
            var naam = _repo.BestParticipant();
            Assert.AreEqual("B", naam);
        }
    }
}
