namespace RaceSimulatorGUI
{
    public class Bounds
    {
        public int LowestX { get; set; }
        public int LowestY { get; set; }
        public int HighestX { get; set; }
        public int HightestY { get; set; }

        public Bounds(int lowestX, int lowestY, int highestX, int hightestY)
        {
            LowestX = lowestX;
            LowestY = lowestY;
            HighestX = highestX;
            HightestY = hightestY;
        }
    }
}