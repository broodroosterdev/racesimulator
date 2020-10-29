using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DataPoints
{
    public class LaneSwitch : IDataPoint
    {
        public String Name { get; set; }
        public Section Section;
        public Lane ToLane;

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
    }

    public enum Lane
    {
        Left,
        Right
    }
}
