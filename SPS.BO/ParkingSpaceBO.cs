using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class ParkingSpaceBO : IBusiness<ParkingSpace, int>
    {
        public virtual void Add(ParkingSpace parkingSpace)
        {
            using (var context = new SPSDb())
            {
                context.ParkingSpaces.Add(parkingSpace);
                context.SaveChanges();
            }
        }

        public virtual void Remove(ParkingSpace parkingSpace)
        {
            using (var context = new SPSDb())
            {
                context.ParkingSpaces.Remove(parkingSpace);
                context.SaveChanges();
            }
        }

        public virtual void Update(ParkingSpace parkingSpace)
        {
            using (var context = new SPSDb())
            {
                var entity = context.ParkingSpaces.Find(parkingSpace.Number);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(parkingSpace);
                entity.Parking = parkingSpace.Parking;
                context.SaveChanges();
            }
        }

        public virtual ParkingSpace Find(int number)
        {
            throw new NotImplementedException();
        }

        public virtual IList<ParkingSpace> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}

