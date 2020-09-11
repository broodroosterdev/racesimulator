using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Car : IEquipment
    {
        private int _quality;
        public int Quality { get => _quality; set => _quality = value; }
        private int _performance;
        public int Performance { get => _performance; set => _performance = value; }
        private int _speed;
        public int Speed { get => _speed; set => _speed = value; }
        private bool _isBroken;
        public bool IsBroken { get => _isBroken; set => _isBroken = value; }
    }
}
