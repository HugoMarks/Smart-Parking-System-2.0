using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SPS.BO
{
    public class ParkingBO : IBusiness<Parking, string>
    {
        public virtual void Add(Parking parking)
        {
            using (var context = new SPSDb())
            {
                if (context.Parkings.Any(p => p.LocalManager.CPF == parking.LocalManager.CPF))
                {
                    throw new UniqueKeyViolationException("Esse admistrador já está associado a um estacionamento.");
                }

                if (parking.LocalManager != null)
                {
                    parking.LocalManager = context.LocalManagers.SingleOrDefault(lm => lm.CPF == parking.LocalManager.CPF);
                }

                if (parking.Address != null)
                {
                    parking.Address = context.Addresses.SingleOrDefault(a => a.PostalCode == parking.Address.PostalCode);
                }

                context.Parkings.Add(parking);
                context.SaveChanges();
            }
        }

        public virtual void Remove(Parking parking)
        {
            using (var context = new SPSDb())
            {
                context.Parkings.Remove(parking);
                context.SaveChanges();
            }
        }

        public virtual void Update(Parking parking)
        {
            using (var context = new SPSDb())
            {
                var entity = context.Parkings.Find(parking.CNPJ);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(parking);

                if (parking.Address != null)
                {
                    entity.Address = context.Addresses.SingleOrDefault(a => a.PostalCode == parking.Address.PostalCode);
                }

                if (parking.LocalManager != null)
                {
                    entity.LocalManager = context.LocalManagers.SingleOrDefault(l => l.CPF == parking.LocalManager.CPF);
                }

                entity.Prices = parking.Prices;
                context.SaveChanges();
            }
        }

        public virtual Parking Find(string key)
        {
            Parking parking = null;

            using (var context = new SPSDb())
            {
                parking = context.Parkings
                    .Include(p => p.Clients)
                    .Include(p => p.Collaborators)
                    .Include(p => p.LocalManager)
                    .Include(p => p.Address)
                    .Include(p => p.Spaces)
                    .Include(p => p.Prices)
                    .SingleOrDefault(p => p.CNPJ == key);
            }

            return parking;
        }

        public virtual IList<Parking> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Parkings
                    .Include(p => p.Clients)
                    .Include(p => p.Collaborators)
                    .Include(p => p.LocalManager)
                    .Include(p => p.Address)
                    .Include(p => p.Spaces)
                    .Include(p => p.Prices)
                    .ToList();
            }
        }
    }
}

