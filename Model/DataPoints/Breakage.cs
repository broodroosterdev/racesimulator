using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Breakage
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
