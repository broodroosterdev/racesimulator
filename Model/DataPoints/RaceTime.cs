using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class RaceTime : IDataPoint
    {
        public String Name { get; set; }
        public TimeSpan Time { get; set; }

        public void Add(List<IDataPoint> list)
        {
            bool found = false;
            foreach (var item in list)
            {
                if (item.Name != Name) continue;
                if (item.GetType() == typeof(RaceTime))
                {
                    var time = item as RaceTime;
                    time.Time = Time;
                    found = true;
                }
            }
            if (!found)
                list.Add(this);
        }
    }
}
