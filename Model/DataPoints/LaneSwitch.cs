using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.DataPoints
{
    public class LaneSwitch : IDataPoint
    {
        public String Name { get; set; }
        public Section Section { get; set; }
        public Lane ToLane { get; set; }

        public LaneSwitch(String name, Section section, Lane toLane)
        {
            Name = name;
            Section = section;
            ToLane = toLane;
        }

        public void Add(List<IDataPoint> list)
        {
            list.Add(this);
        }

        public string BestParticipant(List<IDataPoint> list)
        {
            //Group Switches by participant
            var switchesByPerson = list.ToLookup(e => e.Name);
            //Get group with the most switches
            var highestPerson = switchesByPerson.Aggregate((p1, p2) => p1.Count() > p2.Count() ? p1 : p2);
            return highestPerson.Key;
        }
    }

    public enum Lane
    {
        Left,
        Right
    }
}