using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class DriverPoints : IDataPoint
    {
        public String Name { get; set; }
        public int Points { get; set; }

        public void Add(List<IDataPoint> list)
        {
            bool found = false;
            foreach (var item in list)
            {
                if (item.Name != Name) continue;
                if (item.GetType() == typeof(DriverPoints))
                {
                    var points = item as DriverPoints;
                    points.Points += Points;
                    found = true;
                }
            }

            if (!found)
                list.Add(this);
        }

        public string BestParticipant(List<IDataPoint> list)
        {
            var sectionTimes = list.Select(p => (DriverPoints) p).ToList();
            var bestParticipant = sectionTimes.Aggregate((p1, p2) => p1.Points > p2.Points ? p1 : p2);
            return bestParticipant.Name;
        }
    }
}