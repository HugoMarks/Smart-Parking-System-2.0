using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class PriceBO : IBusiness<Price, int>
    {
        public virtual void Add(Price price)
        {
            using (var context = new SPSDb())
            {
                context.Prices.Add(price);
                context.SaveChanges();
            }
        }

        public virtual Price Find(params object[] keys)
        {
            using (var context = new SPSDb())
            {
                return context.Prices.Find(keys);
            }
        }

        public virtual IList<Price> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Prices.ToList();
            }
        }

        public virtual void Remove(Price price)
        {
            using (var context = new SPSDb())
            {
                context.Prices.Remove(price);
                context.SaveChanges();
            }
        }

        public virtual void Update(Price price)
        {
            using (var context = new SPSDb())
            {
                var entity = context.Prices.Find(price.Id);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(price);
                entity.Parking = price.Parking;

                context.SaveChanges();
            }
        }
    }
}

