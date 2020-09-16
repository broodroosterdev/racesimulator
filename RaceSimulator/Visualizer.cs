using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Model;

namespace RaceSimulator
{
    public static class Visualizer
    {
        #region graphics
        private static string[] _straightHorizontal =
        {
            "--------", 
            "        ",
            "        ",
            "--------",
        };
        private static string[] _straightVertical =
        {
            "|     |", 
            "|     |", 
            "|     |", 
            "|     |"
        };
        private static string[] _rightHorizontal =
        {
            "----\\  ", 
            "     \\ ", 
            "      \\", 
            "\\     |"
        };
        private static string[] _rightVertical =
        {
            "  /-----",
            " /      ",
            "/       ",
            "|     /-"
        };
        private static string[] _leftHorizontal =
        {
            "/     |",
            "      /",
            "     / ",
            "----/  "
        };
        private static string[] _leftVertical =
        {
            "|     \\-",
            "\\      ",
            " \\     ",
            "  \\-----",
        };
        private static string[] _finishHorizontal =
        {
            "--------",
            "      # ",
            "      # ",
            "--------",
        };
        private static string[] _finishVertical =
        {
            "|     |", 
            "| # # |", 
            "|     |", 
            "|     |"
        };
        private static string[] _startHorizontal =
        {
            "--------",
            "     |  ", 
            "     |  ",
            "--------",
        };
        private static string[] _startVertical =
        {
            "|     |", 
            "| - - |", 
            "|     |", 
            "|     |"
        };
        #endregion

        public static void Initialize()
        {

        }

        public static void DrawTrack(Track track)
        {
            var grid = GenerateGrid(track.Sections);
            Console.WriteLine(track.Name);
            for (int y = 0; y < grid.Count; y++)
            {
                var tiles = grid[y];
                for (int x = 0; x < tiles.Count; x++)
                {
                    if(tiles[x] != null)
                        //Add 1 to the y to make space for name of Track
                        DrawSection(tiles[x].Type, tiles[x].Direction, x, y + 1);
                }
            }
            
        }

        private static void DrawSection(SectionTypes type, int direction, int x, int y)
        {
            var ascii = GetAscii(type, direction);
            x = x * 8;
            y = y * 4;
            foreach (string line in ascii)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(line);
                y++;
            }
        }

        private static string[] GetAscii(SectionTypes type, int direction)
        {
            string[] ascii = { };
            switch (type)
            {
                case SectionTypes.Straight:
                    {
                        if (direction % 2 != 0)
                            ascii = _straightHorizontal;
                        else
                            ascii = _straightVertical;
                        break;
                    }
                case SectionTypes.LeftCorner:
                    {
                        switch (direction)
                        {
                            case 0:
                                {
                                    ascii = _rightHorizontal;
                                    break;
                                }
                            case 1:
                                {
                                    ascii = _leftHorizontal;
                                    break;
                                }
                            case 2:
                                {
                                    ascii = _leftVertical;
                                    break;
                                }
                            case 3:
                                {
                                    ascii = _rightVertical;
                                    break;
                                }
                        }

                        break;
                    }
                case SectionTypes.RightCorner:
                    {
                        switch (direction)
                        {
                            case 0:
                                {
                                    ascii = _rightVertical;
                                    break;
                                }
                            case 1:
                                {
                                    ascii = _rightHorizontal;
                                    break;
                                }
                            case 2:
                                {
                                    ascii = _leftHorizontal;
                                    break;
                                }
                            case 3:
                                {
                                    ascii = _leftVertical;
                                    break;
                                }
                        }

                        break;
                    }
                case SectionTypes.StartGrid:
                    {
                        if (direction % 2 != 0)
                            ascii = _startHorizontal;
                        else
                            ascii = _startVertical;
                        break;
                    }
                case SectionTypes.Finish:
                    {
                        if (direction % 2 != 0)
                            ascii = _finishHorizontal;
                        else
                            ascii = _finishVertical;
                        break;
                    }
            }
            return ascii;
        }

        private static List<List<SectionInfo>> GenerateGrid(LinkedList<Section> sections)
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
            //Trim vertical empty tiles
            sectionList = sectionList.GetRange(lowestY, (highestY - lowestY) + 1);
            //Trim horizontal empty tiles
            sectionList = sectionList.Select(tiles => tiles.GetRange(lowestX, highestX - lowestX + 1)).ToList();
            //Reverse to get top of circuit at the top
            sectionList.Reverse();
            return sectionList;
        }
    }

    class SectionInfo
    {
        public SectionTypes Type { get; set; }
        public int Direction { get; set; }

        public static Direction IntToDirection(int direction)
        {
            switch (direction)
            {
                case 0:
                {
                    return RaceSimulator.Direction.Up;
                }
                case 1:
                {
                    return RaceSimulator.Direction.Right;
                }
                case 2:
                {
                    return RaceSimulator.Direction.Down;
                }
                case 3:
                {
                    return RaceSimulator.Direction.Left;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
