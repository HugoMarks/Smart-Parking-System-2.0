using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class PriceBO : IBusiness<Price>
    {
        private static SPSContext Context = SPSContext.Instance;

        public virtual void Add(Price price)
        {
            Context.Prices.Add(price);
            Context.SaveChanges();
        }

        public virtual Price Find(int id)
        {
            return Context.Prices.Find(id);
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
            var savedPrice = Context.Prices.SingleOrDefault(p => p.Id == price.Id);

            if (savedPrice != null)
            {
                savedPrice = price;
                Context.SaveChanges();
            }
        }
    }
}

