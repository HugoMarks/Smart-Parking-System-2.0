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
            var savedParkingSpace = Context.ParkingSpaces.SingleOrDefault(p => p.Number == parkingSpace.Number);

            if (savedParkingSpace != null)
            {
                savedParkingSpace = parkingSpace;
                Context.SaveChanges();
            }
        }

        public virtual ParkingSpace Find(int id)
        {
            return Context.ParkingSpaces.Find(id);
        }

        public virtual IList<ParkingSpace> FindAll()
        {
            return Context.ParkingSpaces.ToList();
        }
    }
}

