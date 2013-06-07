using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controls.Interface
{
    public interface IContentHost<T>
    {
        void Add(IEnumerable<T> items);
        void Remove(IEnumerable<T> items);
        void Update();
    }
}
