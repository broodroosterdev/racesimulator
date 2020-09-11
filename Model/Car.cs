using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Car : IEquipment
    {
        public int Quality { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Performance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsBroken { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
