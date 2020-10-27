using System;
using System.Collections.Generic;
using System.Text;
using Model.DataPoints;

namespace Model
{
    public class Store<T> where T : DataPoint
    {
        private List<T> _list = new List<T>();
    }
}
