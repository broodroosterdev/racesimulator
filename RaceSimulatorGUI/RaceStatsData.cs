using System.Collections.Generic;
using Controller;
using Model;
using System.Linq;
using Model.DataPoints;

namespace RaceSimulatorGUI
{
    public class RaceStatsData : StatsData
    {
        public List<Breakage> Breakages => Data.CurrentRace.Breakages.GetData()
            .Select(item => item as Breakage).ToList();

        public List<LaneSwitch> LaneSwitches =>
            Data.CurrentRace.LaneSwitches.GetData()
                .Select(item => item as LaneSwitch).ToList();

        public List<SectionTime> SectionTimes => Data.CurrentRace.SectionTimes.GetData()
            .Select(item => item as SectionTime).ToList();
    }
}