﻿using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SPS.BO
{
    public class CollaboratorBO : IBusiness<Collaborator, string>
    {
        public virtual void Add(Collaborator collaborator)
        {
            using (var context = new SPSDb())
            {
                if (this.Find(collaborator.CPF) != null)
                {
                    throw new UniqueKeyViolationException(string.Format("There is already a collaborator with CPF {0}.", collaborator.CPF));
                }

                if (collaborator.Address != null)
                {
                    var address = context.Addresses.SingleOrDefault(a => a.PostalCode == collaborator.Address.PostalCode);

                    if (address != null)
                    {
                        collaborator.Address = address;
                    }
                }

                if (collaborator.Parking != null)
                {
                    collaborator.Parking = context.Parkings.SingleOrDefault(p => p.CNPJ == collaborator.Parking.CNPJ);
                }

                context.Collaborators.Add(collaborator);
                context.SaveChanges();
            }
        }

        public virtual void Remove(Collaborator collaborator)
        {
            using (var context = new SPSDb())
            {
                context.Collaborators.Remove(collaborator);
                context.SaveChanges();
            }
        }

        public virtual void Update(Collaborator collaborator)
        {
            using (var context = new SPSDb())
            {
                var entity = context.Collaborators.SingleOrDefault(c => c.Email == collaborator.Email);

                if (entity == null)
                    return;

                collaborator.Id = entity.Id;
                context.Entry(entity).CurrentValues.SetValues(collaborator);

                if (collaborator.Address != null)
                {
                    var address = context.Addresses.SingleOrDefault(a => a.PostalCode == collaborator.Address.PostalCode);

                    if (address == null)
                    {
                        address = collaborator.Address;
                    }

                    entity.Address = address;
                }

                if (collaborator.Parking != null)
                {
                    entity.Parking = context.Parkings.SingleOrDefault(p => p.CNPJ == collaborator.Parking.CNPJ);
                }

                context.SaveChanges();
            }
        }

        public virtual Collaborator Find(string cpf)
        {
            Collaborator collaborator = null;

            using (var context = new SPSDb())
            {
                collaborator = context.Collaborators
                    .Include(c => c.Parking)
                    .Include(c => c.Parking.Spaces)
                    .Include(c => c.Parking.Prices)
                    .Include(c => c.Parking.LocalManager)
                    .Include(c => c.Parking.Collaborators)
                    .Include(c => c.Parking.Clients)
                    .Include(c => c.Address)
                    .SingleOrDefault(c => c.CPF == cpf);
            }

            return collaborator;
        }

        public virtual IList<Collaborator> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Collaborators
                    .Include(c => c.Parking)
                    .Include(c => c.Parking.Spaces)
                    .Include(c => c.Parking.Prices)
                    .Include(c => c.Parking.LocalManager)
                    .Include(c => c.Parking.Collaborators)
                    .Include(c => c.Parking.Clients)
                    .Include(c => c.Address)
                    .ToList();
            }
        }
    }
}

