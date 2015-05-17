using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class ParkingBO : IBusiness<Parking>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(Parking parking)
        {
            Context.Parkings.Add(parking);
            Context.SaveChanges();
        }

        public virtual void Remove(Parking parking)
        {
            Context.Parkings.Remove(parking);
            Context.SaveChanges();
        }

        public virtual void Update(Parking parking)
        {
            var entity = Context.Parkings.Find(parking.CNPJ);

            if (entity == null)
                return;

            Context.Entry(entity).CurrentValues.SetValues(parking);
            entity.Address = parking.Address;
            entity.LocalManager = parking.LocalManager;

            Context.SaveChanges();
        }

        public virtual Parking Find(params object[] keys)
        {
            return Context.Parkings.Find(keys);
        }

        public virtual IList<Parking> FindAll()
        {
            return Context.Parkings.ToList();
        }
    }
}

