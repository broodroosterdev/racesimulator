using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DataPoints
{
    public interface IDataPoint
    {
        public String Name { get; set; }
        void Add(List<IDataPoint> list);
    }
}
