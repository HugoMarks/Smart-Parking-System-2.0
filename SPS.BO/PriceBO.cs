using SPS.BO.Exceptions;
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
                if (price.Parking.Prices.Any(p => ContainsPrice(p, price)))
                {
                    throw new UniqueKeyViolationException("Já existe um preço nesse intervalo de tempo");
                }

                if (price.Parking != null)
                {
                    price.Parking = context.Parkings.Find(price.Parking.CNPJ);
                }

                context.Prices.Add(price);
                context.SaveChanges();
            }
        }

        public virtual Price Find(int id)
        {
            using (var context = new SPSDb())
            {
                return context.Prices
                    .Include("Parking")
                    .SingleOrDefault(p => p.Id == id);
            }
        }

        public virtual IList<Price> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Prices
                    .Include("Parking")
                    .ToList();
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

                if(price.IsDefault)
                {
                    var oldDefault = context.Prices.SingleOrDefault(p => p.IsDefault);

                    if (oldDefault != null)
                    {
                        oldDefault.IsDefault = false;
                    }
                }

                price.Id = entity.Id;
                context.Entry(entity).CurrentValues.SetValues(price);

                if (price.Parking != null)
                {
                    entity.Parking = context.Parkings.Find(entity.Parking.CNPJ);
                }

                context.SaveChanges();
            }
        }

        private bool ContainsPrice(Price price1, Price price2)
        {
            if (price1.IsDefault || price2.IsDefault)
            {
                return false;
            }

            return (price1.StartTime == price2.StartTime && price1.EndTime == price2.EndTime) || 
                (price2.StartTime >= price1.StartTime && price2.EndTime <= price1.EndTime);
        }
    }
}

