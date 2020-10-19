using System.Collections.Generic;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; } = new List<IParticipant>();
        public Queue<Track> Tracks { get; set; } = new Queue<Track>();

        public DataRepository<List<DriverPoints>> Points { get; set; } = new DataRepository<List<DriverPoints>>();
        public Track NextTrack()
        {
            if (Tracks.Count > 0)
                return Tracks.Dequeue();
            return null;
        }

        public void GivePoints(List<RaceTime> raceResult)
        {
            var points = 20;
            var result = new List<DriverPoints>();
            foreach (var laptime in raceResult)
            {
                result.Add(new DriverPoints()
                {
                    Name = laptime.Name,
                    Points = points
                });
                if (points > 1)
                    points--;
            }
            Points.AddValue(result);
        }
    }
}
