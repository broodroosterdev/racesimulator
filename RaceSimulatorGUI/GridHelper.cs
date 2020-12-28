using System.Collections.Generic;
using System.Linq;
using Model;

namespace RaceSimulatorGUI
{
    public static class GridHelper
    {
       public static (Bounds, List<List<SectionInfo>>) GenerateGrid(LinkedList<Section> sections)
        {
            int x = 101;
            int y = 100;
            int lowestX = x;
            int highestX = x;
            int lowestY = y;
            int highestY = y;
            int direction = 1;
            List<List<SectionInfo>> sectionList = new List<List<SectionInfo>>();
            //Fill grid with nulls
            for (int i = 0; i < 200; i++)
            {
                sectionList.Add(Enumerable.Repeat<SectionInfo>(null, 200).ToList());
            }
            LinkedListNode<Section> iterator = sections.First;
            for (int i = 0; i < sections.Count; i++)
            {
                sectionList[y][x] = new SectionInfo()
                {
                    Direction = direction,
                    Type = iterator.Value.SectionType,
                    Section = iterator.Value
                };

                //Change direction
                switch (iterator.Value.SectionType)
                {
                    case SectionTypes.LeftCorner:
                        {
                            if (direction == 0)
                            {
                                direction = 3;
                            }
                            else
                            {
                                direction--;
                            }

                            break;
                        }
                    case SectionTypes.RightCorner:
                        {
                            if (direction == 3)
                            {
                                direction = 0;
                            }
                            else
                            {
                                direction++;
                            }
                            break;
                        }
                }

                //Move to next tile
                switch (direction)
                {
                    case 0:
                        {
                            y++;
                            break;
                        }
                    case 1:
                        {
                            x++;
                            break;
                        }
                    case 2:
                        {
                            y--;
                            break;
                        }
                    case 3:
                        {
                            x--;
                            break;
                        }
                }

                //Get bounds of track
                if (x < lowestX)
                    lowestX = x;
                if (x > highestX)
                    highestX = x;
                if (y < lowestY)
                    lowestY = y;
                if (y > highestY)
                    highestY = y;

                iterator = iterator.Next;
            }
            
            Bounds bounds = new Bounds(lowestX, lowestY, highestX, highestY);
            //Trim vertical empty tiles
            sectionList = sectionList.GetRange(lowestY, (highestY - lowestY) + 1);
            //Trim horizontal empty tiles
            sectionList = sectionList.Select(tiles => tiles.GetRange(lowestX, highestX - lowestX + 1)).ToList();
            //Reverse to get top of circuit at the top
            sectionList.Reverse();
            return (bounds, sectionList);
        } 
    }
}