using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Driver : IParticipant
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }
        private int _points;
        public int Points { get => _points; set => _points = value; }
        private IEquipment _equipment;
        public IEquipment Equipment { get => _equipment; set => _equipment = value; }
        private TeamColors _teamcolor;
        public TeamColors TeamColor { get => _teamcolor; set => _teamcolor = value; }
    }
}
