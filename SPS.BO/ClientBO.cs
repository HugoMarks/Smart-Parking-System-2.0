using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SPS.BO
{
    public class ClientBO : IBusiness<Client, string>
    {
        public virtual void Add(Client client)
        {
            using (var context = new SPSDb())
            {
                if (client.CPF != null && this.Find(client.CPF) != null)
                {
                    throw new UniqueKeyViolationException(string.Format("There is already a client with CPF {0}.", client.CPF));
                }

                if (client.Address != null)
                {
                    client.Address = context.Addresses.SingleOrDefault(a => a.PostalCode == client.Address.PostalCode);
                }

                context.Clients.Add(client);
                context.SaveChanges();
            }
        }

        public virtual Client Find(string cpf)
        {
            Client client = null;

            using (var context = new SPSDb())
            {
                client = context.Clients
                    .Include(c => c.Tags)
                    .Include(c => c.Records)
                    .Include(c => c.Parkings)
                    .Include(c => c.Address)
                    .SingleOrDefault(c => c.CPF == cpf);
            }

            return client;
        }

        public virtual IList<Client> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Clients
                    .Include(c => c.Tags)
                    .Include(c => c.Records)
                    .Include(c => c.Parkings)
                    .Include(c => c.Address)
                    .ToList();
            }
        }

        public virtual void Remove(Client client)
        {
            using (var context = new SPSDb())
            {
                context.Clients.Remove(client);
                context.SaveChanges();
            }
        }

        public virtual void Update(Client client)
        {
            using (var context = new SPSDb())
            {
                Client entity;

                if (client.Id > 0)
                {
                    entity = context.Clients.Find(client.Id);
                }
                else
                {
                    entity = context.Clients.SingleOrDefault(c => c.Email == client.Email);
                }

                if (entity == null)
                    return;

                client.Id = entity.Id;
                context.Entry(entity).CurrentValues.SetValues(client);

                if (client.Address != null)
                {
                    entity.Address = context.Addresses.SingleOrDefault(a => a.PostalCode == client.Address.PostalCode);
                }

                if (client.Tags != null)
                {
                    entity.Tags = client.Tags;
                }

                context.SaveChanges();
            }
        }

        public void AttachToParking(Client client, string parkingCNPJ)
        {
            using (var context = new SPSDb())
            {
                Client dbClient;
                Parking dbParking = context.Parkings.SingleOrDefault(p => p.CNPJ == parkingCNPJ);

                if (!string.IsNullOrEmpty(client.CPF))
                {
                    dbClient = context.Clients.Include(c => c.Parkings).SingleOrDefault(c => c.CPF == client.CPF);
                }
                else
                {
                    dbClient = context.Clients.Include(c => c.Parkings).SingleOrDefault(c => c.Email == client.Email);
                }

                if (dbClient.Parkings.Count > 5)
                {
                    throw new MaximumLimitReachedException("Número máximo de estacionamentos para esse cliente atingido");
                }

                dbClient.Parkings.Add(dbParking);
                context.SaveChanges();
            }
        }
    }
}

