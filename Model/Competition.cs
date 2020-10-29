using System.Collections.Generic;
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
            if (Tracks.Count > 0)
                return Tracks.Dequeue();
            return null;
        }

        public void GivePoints()
        {
            var points = 20;
            foreach (var laptime in RaceTimes.GetData())
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
