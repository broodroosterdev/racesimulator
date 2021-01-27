using System.Collections.Generic;
using System.Linq;
using Model.DataPoints;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; } = new List<IParticipant>();
        public Queue<Track> Tracks { get; set; } = new Queue<Track>();
        public DataRepository<DriverPoints> Points { get; set; } = new DataRepository<DriverPoints>();
        public DataRepository<RaceTime> RaceTimes { get; set; } = new DataRepository<RaceTime>();


        public Track NextTrack()
        {
            return Tracks.Count > 0 ? Tracks.Dequeue() : null;
        }

        public void GivePoints()
        {
            var points = 20;
            var sortedTimes = RaceTimes.GetData().Select(e => (RaceTime) e).OrderBy(raceTime => raceTime.Time).ToList();
            foreach (var laptime in sortedTimes)
            {
                Points.AddValue(new DriverPoints()
                {
                    Name = laptime.Name,
                    Points = points
                });
                if (points > 1)
                    points--;
            }
        }
    }
}