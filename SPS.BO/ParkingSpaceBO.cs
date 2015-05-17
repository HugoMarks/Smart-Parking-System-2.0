using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class ParkingSpaceBO : IBusiness<ParkingSpace>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(ParkingSpace parkingSpace)
        {
            Context.ParkingSpaces.Add(parkingSpace);
            Context.SaveChanges();
        }

        public virtual void Remove(ParkingSpace parkingSpace)
        {
            Context.ParkingSpaces.Remove(parkingSpace);
            Context.SaveChanges();
        }

        public virtual void Update(ParkingSpace parkingSpace)
        {
            var entity = Context.ParkingSpaces.Find(parkingSpace.Number);

            if (entity == null)
                return;

            Context.Entry(entity).CurrentValues.SetValues(parkingSpace);
            entity.Parking = parkingSpace.Parking;
            Context.SaveChanges();
        }

        public virtual ParkingSpace Find(params object[] keys)
        {
            return Context.ParkingSpaces.Find(keys);
        }

        public virtual IList<ParkingSpace> FindAll()
        {
            return Context.ParkingSpaces.ToList();
        }
    }
}

