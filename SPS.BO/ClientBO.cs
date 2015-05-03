using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class ClientBO : IBusiness<Client>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(Client client)
        {
            Context.Clients.Add(client);
            Context.SaveChanges();
        }

        public virtual Client Find(int id)
        {
            return Context.Clients.Find(id);
        }

        public virtual IList<Client> FindAll()
        {
            return Context.Clients.ToList();
        }

        public virtual void Remove(Client client)
        {
            Context.Clients.Remove(client);
            Context.SaveChanges();
        }

        public virtual void Update(Client client)
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

