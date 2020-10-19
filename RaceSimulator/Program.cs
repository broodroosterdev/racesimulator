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
            Visualizer.Initialize();
            
            for(; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}