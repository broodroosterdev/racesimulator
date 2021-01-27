using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Model_Track_sectionsToListShould
    {
        private Track _track;

        [SetUp]
        public void Setup()
        {
            _track = new Track("test", new LinkedList<Section>());
        }

        [Test]
        public void EmptyArray_ReturnEmptyList()
        {
            var track = new Track("test", new SectionTypes[0]);
            Assert.IsNotNull(track.Sections);
            Assert.AreEqual(0, track.Sections.Count);
        }

        [Test]
        public void OneItemArray_ReturnOneItemList()
        {
            var track = new Track("test", new SectionTypes[1]
            {
                SectionTypes.StartGrid
            });
            Assert.AreEqual(1, track.Sections.Count);
        }
    }
}