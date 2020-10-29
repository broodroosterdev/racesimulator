using System;
using System.Collections.Generic;
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
    }
}
