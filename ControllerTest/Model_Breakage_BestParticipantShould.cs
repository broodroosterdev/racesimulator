using Model;
using Model.DataPoints;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest
{
    class Model_Breakage_BestParticipantShould
    {
        private DataRepository<Breakage> _repo;

        [SetUp]
        public void Setup()
        {
            _repo = new DataRepository<Breakage>();
            _repo.AddValue(new Breakage("A", new Section()));
            _repo.AddValue(new Breakage("A", new Section()));
            _repo.AddValue(new Breakage("B", new Section()));
        }

        [Test]
        public void ReturnNameWithLeastBreakages()
        {
            var name = _repo.BestParticipant();
            Assert.AreEqual(name, "B");
        }
    }
}