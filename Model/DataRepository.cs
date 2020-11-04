using System;
using System.Collections.Generic;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class DataRepository<T> where T : IDataPoint
    {
        private List<IDataPoint> _list = new List<IDataPoint>();

        public int Count => _list.Count;

        public void AddValue(T value)
        {
            value.Add(_list);
        }

        public string BestParticipant()
        {
            return _list.Count == 0 ? "" : _list[0].BestParticipant(_list);
        }

        public List<IDataPoint> GetData()
        {
            return _list;
        }
    }
}
