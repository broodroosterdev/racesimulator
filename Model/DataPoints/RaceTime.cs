﻿using System;
using System.Collections.Generic;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class RaceTime : DataPoint
    {
        public String Name { get; set; }
        public TimeSpan Time { get; set; }
    }
}
