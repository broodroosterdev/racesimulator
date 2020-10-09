using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DataRepository<T>
    {
        private List<T> _list = new List<T>();

        public void AddValue(T value)
        {
            _list.Add(value);
        }
    }
}
