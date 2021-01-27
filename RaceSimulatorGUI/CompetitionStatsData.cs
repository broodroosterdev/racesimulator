using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller;
using Model;
using System.Linq;
using System.Timers;

namespace RaceSimulatorGUI
{
    public class CompetitionStatsData : StatsData
    {
        public List<DriverPoints> Points => Data.Competition.Points.GetData()
            .Select(points => points as DriverPoints).ToList();

        public List<RaceTime> RaceTimes => Data.Competition.RaceTimes.GetData()
            .Select(time => time as RaceTime).ToList();
    }
}