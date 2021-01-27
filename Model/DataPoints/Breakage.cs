using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class Breakage : IDataPoint
    {
        public String Name { get; set; }
        public Section Section { get; set; }

        public Breakage(String name, Section section)
        {
            Name = name;
            Section = section;
        }

        public void Add(List<IDataPoint> list)
        {
            list.Add(this);
        }

        public string BestParticipant(List<IDataPoint> list)
        {
            //Group Breakages by participant
            var breakagesByParticipant = list.ToLookup(e => e.Name);
            //Get group with the least breakages
            var lowestParticipant = breakagesByParticipant.Aggregate((p1, p2) => p1.Count() < p2.Count() ? p1 : p2);
            return lowestParticipant.Key;
        }
    }
}