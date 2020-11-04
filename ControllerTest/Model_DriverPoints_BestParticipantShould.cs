using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Model_DriverPoints_BestParticipantShould
    {
        private DataRepository<DriverPoints> _repo;
        [SetUp]
        public void Setup()
        {
            _repo = new DataRepository<DriverPoints>();
            _repo.AddValue(new DriverPoints(){Name = "A", Points = 1});
            _repo.AddValue(new DriverPoints(){Name = "B", Points = 2});
        }

        [Test]
        public void GiveParticipantWithMostPoints()
        {
            var naam = _repo.BestParticipant();
            Assert.AreEqual("B", naam);
        }

    }
}
