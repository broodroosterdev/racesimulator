using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public Track(string name, LinkedList<Section> sections)
        {
            Name = name;
            Sections = sections;
        }

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = _sectionsToList(sections);
        }

        private LinkedList<Section> _sectionsToList(SectionTypes[] sections)
        {
            LinkedList<Section> sectionList = new LinkedList<Section>();
            foreach (SectionTypes type in sections)
            {
                sectionList.AddLast(new Section{SectionType = type});
            }

            return sectionList;
        }
    }
}
