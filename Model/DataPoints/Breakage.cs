using System;
using System.Collections.Generic;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class Breakage : DataPoint
    {
        public String Name;
        public Section Section;

        public Breakage(String name, Section section)
        {
            Name = name;
            Section = section;
        }
    }
}
