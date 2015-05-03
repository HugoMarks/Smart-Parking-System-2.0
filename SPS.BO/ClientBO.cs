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

        public virtual MonthlyClient Find(int id)
        {
            return Context.Clients.Find(id);
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
            var savedClient = Context.Clients.SingleOrDefault(c => c.Id == client.Id);

            if (savedClient != null)
            {
                savedClient = client;
                Context.SaveChanges();
            }
        }
    }
}

