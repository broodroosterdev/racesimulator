using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DataPoints
{
    public class LaneSwitch : DataPoint
    {
        public String Name;
        public Section Section;
        public Lane ToLane;

        public LaneSwitch(String name, Section section, Lane toLane)
        {
            Name = name;
            Section = section;
            ToLane = toLane;
        }
    }

    public enum Lane
    {
        Left,
        Right
    }
}
