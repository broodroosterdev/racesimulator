using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; } = new List<IParticipant>();
        public Queue<Track> Tracks { get; set; } = new Queue<Track>();
        public Track NextTrack()
        {
            if (Tracks.Count > 0)
                return Tracks.Dequeue();
            else
                return null;
        }
    }
}
