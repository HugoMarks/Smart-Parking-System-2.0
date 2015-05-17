using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class PriceBO : IBusiness<Price>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(Price price)
        {
            Context.Prices.Add(price);
            Context.SaveChanges();
        }

        public virtual Price Find(params object[] keys)
        {
            return Context.Prices.Find(keys);
        }

        public virtual IList<Price> FindAll()
        {
            return Context.Prices.ToList();
        }

        public virtual void Remove(Price price)
        {
            Context.Prices.Remove(price);
            Context.SaveChanges();
        }

        public virtual void Update(Price price)
        {
            var entity = Context.Prices.Find(price.Id);

            if (entity == null)
                return;

            Context.Entry(entity).CurrentValues.SetValues(price);
            entity.Parking = price.Parking;

            Context.SaveChanges();
        }
    }
}

