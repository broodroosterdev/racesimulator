using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Timers;

namespace Controller
{
    public class Race
    {
        public delegate void DriversChanged(object model, DriversChangedEventArgs e);

        public event DriversChanged driversChanged;
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer = new Timer(500);


        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            RandomizeEquipment();
            GiveStartPositions();
            _timer.Elapsed += OnTimedEvent;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }
        
        public SectionData GetSectionData(Section section)
        {
            if (!_positions.ContainsKey(section))
            {
                _positions.Add(section, new SectionData());
                
            }
            return _positions[section];
        }

        public void OnTimedEvent(Object source, EventArgs e)
        {
            //Console.WriteLine("Timer fired event");
        }



        public void RandomizeEquipment()
        {
            foreach(IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Performance = _random.Next();
            }
        }

        public void GiveStartPositions()
        {
            //Create copy to avoid sorting Participants itself
            var sortedParticipants = Participants;
            //Sort based on performance
            sortedParticipants.Sort((participant1, participant2) => participant1.Equipment.Performance.CompareTo(participant2.Equipment.Performance)); 
            //Find finish section
            var iterator = Track.Sections.First;
            while (iterator.Value.SectionType != SectionTypes.Finish)
            {
                iterator = iterator.Next;
            }
            //Move to startgrid before finish
            iterator = iterator.Previous;
            foreach(IParticipant participant in sortedParticipants)
            {
                if (iterator.Value.SectionType != SectionTypes.StartGrid)
                    break;
                var data = GetSectionData(iterator.Value);
                if (data.Left == null)
                {
                    data.Left = participant;
                } else if (data.Right == null)
                {
                    data.Right = participant;
                }
                else if(iterator.Previous != null)
                {

                    iterator = iterator.Previous;
                    data = GetSectionData(iterator.Value);
                    data.Left = participant;
                }
            }
        }
    }
}
