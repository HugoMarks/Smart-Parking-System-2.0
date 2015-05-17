using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class MontlyClientBO : IBusiness<MonthlyClient>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(MonthlyClient client)
        {
            Context.Clients.Add(client);
            Context.SaveChanges();
        }

        public virtual MonthlyClient Find(params object[] keys)
        {
            return Context.Clients.Find(keys);
        }

        public virtual IList<MonthlyClient> FindAll()
        {
            return Context.Clients.ToList();
        }

        public virtual void Remove(MonthlyClient client)
        {
            Context.Clients.Remove(client);
            Context.SaveChanges();
        }

        public virtual void Update(MonthlyClient client)
        {
            MonthlyClient entity;

            if (client.Id > 0)
            {
                entity = Context.Clients.Find(client.Id);
            }
            else
            {
                entity = Context.Clients.SingleOrDefault(c => c.Email == client.Email);
            }

            if (entity == null)
                return;

            client.Id = entity.Id;
            Context.Entry(entity).CurrentValues.SetValues(client);
            entity.Address = client.Address;
            entity.Parking = client.Parking;
            entity.Tags = client.Tags;

            Context.SaveChanges();
        }
    }
}

