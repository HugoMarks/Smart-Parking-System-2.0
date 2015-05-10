using System.Collections.Generic;

namespace SPS.BO
{
    public interface IBusiness<T>
    {
        void Add(T obj);

        T Find(params object[] keys);

        IList<T> FindAll();

        void Remove(T obj);

        void Update(T obj);
    }
}

