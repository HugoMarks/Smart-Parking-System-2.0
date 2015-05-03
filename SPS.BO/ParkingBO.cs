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
            var savedParking = Context.Parkings.SingleOrDefault(p => p.Id == parking.Id);

            if (savedParking != null)
            {
                savedParking = parking;
                Context.SaveChanges();
            }
        }

        public virtual Parking Find(int id)
        {
            return Context.Parkings.Find(id);
        }

        public virtual IList<Parking> FindAll()
        {
            return Context.Parkings.ToList();
        }
    }
}

