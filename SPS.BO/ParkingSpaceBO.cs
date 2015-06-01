using SPS.BO.Exceptions;
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
                if (parkingSpace.Parking.Spaces.Any(ps => ps.Number == parkingSpace.Number))
                {
                    throw new UniqueKeyViolationException("O estacionamento da vaga já contém uma vaga com o número especificado");
                }

                if (parkingSpace.Parking != null)
                {
                    parkingSpace.Parking = context.Parkings.Find(parkingSpace.Parking.CNPJ);
                }

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
                var entity = context.ParkingSpaces.Find(parkingSpace.Id);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(parkingSpace);
                context.SaveChanges();
            }
        }

        public virtual ParkingSpace Find(int id)
        {
            using (var context = new SPSDb())
            {
                return context.ParkingSpaces
                    .Include("Parking")
                    .SingleOrDefault(ps => ps.Id == id);
            }
        }

        public virtual IList<ParkingSpace> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.ParkingSpaces
                    .Include("Parking")
                    .ToList();
            }
        }
    }
}

