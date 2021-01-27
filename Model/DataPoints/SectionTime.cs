using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class SectionTime : IDataPoint
    {
        public Section Section { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }

        public void Add(List<IDataPoint> list)
        {
            bool found = false;
            foreach (var item in list)
            {
                if (item.Name != Name) continue;
                if (item.GetType() == typeof(SectionTime))
                {
                    var time = item as SectionTime;
                    if (time.Section == Section)
                    {
                        time.Time = Time;
                        found = true;
                    }
                }
            }

            if (!found)
                list.Add(this);
        }

        public string BestParticipant(List<IDataPoint> list)
        {
            //Gets the average of all the timespans
            Func<List<IDataPoint>, double> getAverageTime =
                l => l.Select(e => (e as SectionTime).Time.Milliseconds).Average();
            //Groups the sectionTimes by participant
            var sectionTimesByParticipant = list.GroupBy(e => e.Name);
            var bestParticipant = sectionTimesByParticipant
                //Gets the lowest average
                .Aggregate((p1, p2) =>
                    getAverageTime(p1.ToList()) < getAverageTime(p2.ToList()) ? p1 : p2);
            return bestParticipant.Key;
        }
    }
}