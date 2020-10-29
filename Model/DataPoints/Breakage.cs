using System;
using System.Collections.Generic;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class Breakage : IDataPoint
    {
        public String Name { get; set; }
        public Section Section;

        public Breakage(String name, Section section)
        {
            Name = name;
            Section = section;
        }

        public void Add(List<IDataPoint> list)
        {
            list.Add(this);
        }
    }
}
