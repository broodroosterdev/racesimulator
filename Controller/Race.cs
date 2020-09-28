using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Timers;

namespace Controller
{
    public class Race
    {
        public delegate void DriversChangedEvent(object model, DriversChangedEventArgs e);

        public event DriversChangedEvent DriversChanged;
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
           var driverChanged = MoveDrivers();
           if (driverChanged)
           {
               DriversChanged?.Invoke(this, new DriversChangedEventArgs(){Track = this.Track});
           }
        }

        public bool MoveDrivers()
        {
            bool driverChanged = false;
            var iterator = Track.Sections.Last;
            while (iterator != null)
            {
                SectionData data = GetSectionData(iterator.Value);
                //Check left driver
                if (data.Left != null)
                {
                    var equipment = data.Left.Equipment;
                    data.DistanceLeft += equipment.Performance * equipment.Speed;
                    if (data.DistanceLeft > 50)
                    {
                        driverChanged = true;
                        var newDistance = data.DistanceLeft - 50;
                        SectionData nextData;
                        if (iterator.Next != null)
                        {
                            nextData = GetSectionData(iterator.Next.Value);
                            
                        }
                        else
                        {
                            nextData = GetSectionData(Track.Sections.First.Value);
                        }
                        //Check if it can go straight without crashing
                        if (nextData.Left == null)
                        {
                            nextData.Left = data.Left;
                            nextData.DistanceLeft = newDistance;
                            data.Left = null;
                            data.DistanceLeft = 0;
                        //Check if it can overtake on the right instead
                        } else if (nextData.Right == null)
                        {
                            nextData.Right = data.Left;
                            nextData.DistanceRight = newDistance;
                            data.Left = null;
                            data.DistanceLeft = 0;
                        }
                        //If it cant overtake, set distance to 50 to check it next timer event
                        else
                        {
                            data.DistanceLeft = 50;
                        }
                    }
                }
                //Check right driver
                if (data.Right != null)
                {
                    var equipment = data.Right.Equipment;
                    data.DistanceRight += equipment.Performance * equipment.Speed;
                    if (data.DistanceRight > 50)
                    {
                        driverChanged = true;
                        var newDistance = data.DistanceRight - 50;
                        SectionData nextData;
                        if (iterator.Next != null)
                        {
                            nextData = GetSectionData(iterator.Next.Value);
                        }
                        else
                        {
                            nextData = GetSectionData(Track.Sections.First.Value);
                        }
                        //Check if it can go straight without crashing
                        if (nextData.Right == null)
                        {
                            nextData.Right = data.Right;
                            nextData.DistanceRight = newDistance;
                            data.Right = null;
                            data.DistanceRight = 0;
                            //Check if it can overtake on the right instead
                        } else if (nextData.Left == null)
                        {
                            nextData.Left = data.Right;
                            nextData.DistanceLeft = newDistance;
                            data.Right = null;
                            data.DistanceRight = 0;
                        }
                        //If it cant overtake, set distance to 50 to check it next timer event
                        else
                        {
                            data.DistanceRight = 50;
                        }
                       
                    }
                }

                iterator = iterator.Previous;
            }

            return driverChanged;
        }



        public void RandomizeEquipment()
        {
            foreach(IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(1, 100);
                participant.Equipment.Performance = _random.Next(1, 3);
                participant.Equipment.Speed = _random.Next(1, 15);
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
                    data.DistanceLeft = 0;
                } else if (data.Right == null)
                {
                    data.Right = participant;
                    data.DistanceRight = 0;
                    iterator = iterator.Previous;
                }
                else if(iterator.Previous != null)
                {

                    iterator = iterator.Previous;
                    data = GetSectionData(iterator.Value);
                    data.Left = participant;
                    data.DistanceLeft = 0;
                }
            }
        }
    }
}
