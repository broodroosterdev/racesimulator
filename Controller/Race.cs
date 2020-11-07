using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Timers;
using Model.DataPoints;

namespace Controller
{
    public class Race
    {
        private const int AMOUNT_OF_ROUNDS = 2;

        public delegate void DriversChangedEvent(object model, DriversChangedEventArgs e);

        public event DriversChangedEvent DriversChanged;

        public delegate void RaceEndedEvent(object model);

        public event RaceEndedEvent RaceEnded;

        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DataRepository<SectionTime> SectionTimes = new DataRepository<SectionTime>();
        private Dictionary<IParticipant, DateTime> _timeInSection = new Dictionary<IParticipant, DateTime>();
        private DataRepository<Breakage> _breakages = new DataRepository<Breakage>();
        private DataRepository<LaneSwitch> _laneSwitches = new DataRepository<LaneSwitch>();
        private DateTime _startTime;

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Dictionary<IParticipant, int> _rondjes;
        private Timer _timer = new Timer(500);
        private int _finished;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            _rondjes = new Dictionary<IParticipant, int>();
            RandomizeEquipment();
            GiveStartPositions();
            _timer.Elapsed += OnTimedEvent;
        }

        public void Start()
        {
            _timer.Enabled = true;
            _startTime = DateTime.Now;
        }


        public void CleanUp()
        {
            _timer.Enabled = false;
            DriversChanged = null;
        }

        public SectionData GetSectionData(Section section)
        {
            if (!_positions.ContainsKey(section))
            {
                _positions.Add(section, new SectionData());
            }
            return _positions[section];
        }

        private void IncreaseRondjes(IParticipant participant, Action<IParticipant> setter)
        {
            if (!_rondjes.ContainsKey(participant))
            {
                _rondjes.Add(participant, -1);
            }

            var rondjes = _rondjes[participant];
            rondjes++;
            _rondjes[participant] = rondjes;
            if (rondjes >= AMOUNT_OF_ROUNDS)
            {
                Data.Competition.RaceTimes.AddValue(new RaceTime()
                {
                    Name = participant.Name,
                    Time = DateTime.Now.Subtract(_startTime)
                });
                setter(null);
                _finished++;
            }
        }
        private TimeSpan GetSectionTime(IParticipant participant)
        {
            if (_timeInSection.TryGetValue(participant, out DateTime enteredTime))
            {
                return DateTime.Now.Subtract(enteredTime);
            }
            else
            {
                return DateTime.Now.Subtract(_startTime);
            }
        }

        private void SetNewSectionTime(IParticipant participant)
        {
            if (_timeInSection.ContainsKey(participant))
                _timeInSection[participant] = DateTime.Now;
            else
                _timeInSection.Add(participant, DateTime.Now);
        }

        public void RecordSectionTime(Section section, IParticipant participant)
        {
            var timeSpan = GetSectionTime(participant);
            SectionTimes.AddValue(new SectionTime()
            {
                Name = participant.Name,
                Section = section,
                Time = timeSpan
            });
            SetNewSectionTime(participant);
        }

        public bool HasFinished(IParticipant participant)
        {
            return participant == null;
        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (_finished == Participants.Count)
            {
                //Start new race using event
                CleanUp();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Race ended" + new string(' ', Console.WindowWidth));
                RaceEnded?.Invoke(this);
                RaceEnded = null;
            }
            else
            {
                var driverChanged = MoveDrivers();
                if (driverChanged)
                {
                    _timer.Enabled = false;
                    DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });
                    _timer.Enabled = true;
                }
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

                    TryToBreak(data.Left, iterator.Value, ref driverChanged);
                    if (!equipment.IsBroken)
                        data.DistanceLeft += equipment.Performance * equipment.Speed;
                    if (!equipment.IsBroken && data.DistanceLeft > 50)
                    {
                        if (!HasFinished(data.Left))
                        {

                            driverChanged = true;
                            var newDistance = data.DistanceLeft - 50;
                            SectionData nextData = GetSectionData((iterator.Next ?? iterator.List.First).Value);

                            //Check if it can go straight without crashing
                            if (nextData.Left == null)
                            {

                                nextData.Left = data.Left;
                                nextData.DistanceLeft = newDistance;
                                data.Left = null;
                                data.DistanceLeft = 0;
                                RecordSectionTime(iterator.Value, nextData.Left);
                                if (iterator.Value.SectionType == SectionTypes.Finish)
                                {
                                    IncreaseRondjes(nextData.Left, p => nextData.Left = p);
                                }
                                
                                //Check if it can overtake on the right instead
                            }
                            else if (nextData.Right == null)
                            {
                                nextData.Right = data.Left;
                                nextData.DistanceRight = newDistance;
                                data.Left = null;
                                data.DistanceLeft = 0;
                                RecordSectionTime(iterator.Value, nextData.Right);
                                _laneSwitches.AddValue(new LaneSwitch(nextData.Right.Name, (iterator.Next ?? iterator.List.First).Value, Lane.Right));
                                if (iterator.Value.SectionType == SectionTypes.Finish)
                                {
                                    IncreaseRondjes(nextData.Right, p => nextData.Right = p);
                                }
                               
                            }
                            //If it cant overtake, set distance to 50 to check it next timer event
                            else
                            {
                                data.DistanceLeft = 50;
                            }
                        }
                    }
                }

                //Check right driver
                if (data.Right != null)
                {
                    var equipment = data.Right.Equipment;
                    TryToBreak(data.Right, iterator.Value, ref driverChanged);
                    if (!equipment.IsBroken)
                        data.DistanceRight += equipment.Performance * equipment.Speed;
                    if (!equipment.IsBroken && data.DistanceRight > 50)
                    {
                        if (!HasFinished(data.Right))
                        {

                            driverChanged = true;
                            var newDistance = data.DistanceRight - 50;
                            SectionData nextData = GetSectionData((iterator.Next ?? iterator.List.First).Value);

                            //Check if it can go straight without crashing
                            if (nextData.Right == null)
                            {
                                nextData.Right = data.Right;
                                nextData.DistanceRight = newDistance;
                                data.Right = null;
                                data.DistanceRight = 0;
                                RecordSectionTime(iterator.Value, nextData.Right);
                                if (iterator.Value.SectionType == SectionTypes.Finish)
                                {
                                    IncreaseRondjes(nextData.Right, p => nextData.Right = p);
                                }
                               
                                //Check if it can overtake on the right instead
                            }
                            else if (nextData.Left == null)
                            {
                                nextData.Left = data.Right;
                                nextData.DistanceLeft = newDistance;
                                data.Right = null;
                                data.DistanceRight = 0;
                                RecordSectionTime(iterator.Value, nextData.Left);
                                _laneSwitches.AddValue(new LaneSwitch(nextData.Left.Name, (iterator.Next ?? iterator.List.First).Value, Lane.Left));
                                if (iterator.Value.SectionType == SectionTypes.Finish)
                                {
                                    IncreaseRondjes(nextData.Left, p => nextData.Left = p);
                                }
                               
                            }
                            //If it cant overtake, set distance to 50 to check it next timer event
                            else
                            {
                                data.DistanceRight = 50;
                            }
                        }

                    }
                }
                iterator = iterator.Previous;
            }
            return driverChanged;
        }



        public void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(20, 100);
                participant.Equipment.Performance = _random.Next(1, 3);
                participant.Equipment.Speed = _random.Next(5, 15);
            }
        }

        public void TryToBreak(IParticipant participant, Section section, ref bool changed)
        {
            //If the equipment is broken, make the chance it will get repaired higher
            int chance = participant.Equipment.IsBroken ? 5 : 1;
            IEquipment equipment = participant.Equipment;
            int random = _random.Next(0, equipment.Quality * 10);
            if (random <= chance)
            {
                if (!equipment.IsBroken)
                {
                    if(equipment.Speed > 20)
                        equipment.Speed -= 10;
                    equipment.Performance = 1;
                }
                else
                { 
                    _breakages.AddValue(new Breakage(participant.Name, section));
                }

                equipment.IsBroken = !equipment.IsBroken;
                changed = true;
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
            foreach (IParticipant participant in sortedParticipants)
            {
                if (iterator.Value.SectionType != SectionTypes.StartGrid)
                    break;
                var data = GetSectionData(iterator.Value);
                if (data.Left == null)
                {
                    data.Left = participant;
                    data.DistanceLeft = 0;
                }
                else if (data.Right == null)
                {
                    data.Right = participant;
                    data.DistanceRight = 0;
                    iterator = iterator.Previous;
                }
                else if (iterator.Previous != null)
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
