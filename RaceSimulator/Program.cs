using Controller;
using System;
using System.Threading;
using Model;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRace();
            //Data.NextRace();
            Visualizer.Initialize();
            Visualizer.DrawTrack(Data.CurrentRace.Track);
            Data.CurrentRace.Start();
            for(; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
