using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Model;
namespace Controller
{
    static class Data
    {
        static Competition Competition { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
        }

        public static void AddParticipants()
        {
            Competition.Participants = new List<IParticipant>
            {
                new Driver(),
                new Driver(),
                new Driver()
            };
        }

        public static void AddTracks()
        {
            Competition.Tracks = new Queue<Track>();
            Track SilverSteen = new Track("SilverSteen", new LinkedList<Section>());
            SilverSteen.Sections.AddLast(new Section
            {
                SectionType = SectionTypes.StartGrid
            });
            SilverSteen.Sections.AddLast(new Section
            {
                SectionType = SectionTypes.RightCorner
            });
            SilverSteen.Sections.AddLast(new Section
            {
                SectionType = SectionTypes.RightCorner
            });
            SilverSteen.Sections.AddLast(new Section
            {
                SectionType = SectionTypes.Straight
            });
            SilverSteen.Sections.AddLast(new Section
            {
                SectionType = SectionTypes.RightCorner
            });
            SilverSteen.Sections.AddLast(new Section
            {
                SectionType = SectionTypes.RightCorner
            });
            Competition.Tracks.Enqueue(SilverSteen);
        }
    }
}
