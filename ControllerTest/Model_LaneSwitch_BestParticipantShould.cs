using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.DataPoints;
using NUnit.Framework;

namespace ControllerTest
{
    class Model_LaneSwitch_BestParticipantShould
    {
        private DataRepository<LaneSwitch> _repo;
        [SetUp]
        public void Setup()
        {
            _repo = new DataRepository<LaneSwitch>();
            _repo.AddValue(new LaneSwitch("A", new Section(), Lane.Left));
            _repo.AddValue(new LaneSwitch("B", new Section(), Lane.Left));
            _repo.AddValue(new LaneSwitch("B", new Section(), Lane.Left));
        }

        [Test]
        public void ReturnNameOfParticipantWithMostSwitches()
        {
            var naam = _repo.BestParticipant();
            Assert.AreEqual("B", naam);
        }
    }
}
