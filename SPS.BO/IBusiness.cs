using System.Collections.Generic;

namespace SPS.BO
{
    public interface IBusiness<TModel, TKey>
    {
        void Add(TModel obj);

        TModel Find(TKey key);

        IList<TModel> FindAll();

        void Remove(TModel obj);

        void Update(TModel obj);
    }
}

