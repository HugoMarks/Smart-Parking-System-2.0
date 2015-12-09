using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
namespace SPS.BO
{
    public class PlateBO : IBusiness<Plate, string>
    {
        public virtual void Add(Plate plate)
        {
            using (var context = new SPSDb())
            {
                if (context.Plates.Any(t => t.Id == plate.Id))
                {
                    throw new UniqueKeyViolationException("Placa já vinculada");
                }

                if (plate.Client != null)
                {
                    plate.Client = context.Clients.Include(c => c.Plates).SingleOrDefault(c => c.CPF == plate.Client.CPF);

                    if (plate.Client.Plates.Count > 5)
                    {
                        throw new MaximumLimitReachedException("Número máximo de placas atingido");
                    }
                }

                context.Plates.Add(plate);
                context.SaveChanges();
            }
        }

        public virtual void Remove(Plate plate)
        {
            using (var context = new SPSDb())
            {
                context.Plates.Remove(plate);
                context.SaveChanges();
            }
        }

        public virtual void Update(Plate plate)
        {
            using (var context = new SPSDb())
            {
                var entity = context.Plates.Find(plate.Id);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(plate);
                entity.Client = plate.Client;

                context.SaveChanges();
            }
        }

        public virtual Plate Find(string id)
        {
            Plate plate = null;

            using (var context = new SPSDb())
            {
                plate = context.Plates
                    .Include(t => t.Client.Parkings.Select(p => p.Clients.Select(c => c.Plates)))
                    .SingleOrDefault(c => c.Id == id);
            }

            return plate;
        }

        public virtual IList<Plate> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Plates
                    .Include(t => t.Client.Parkings.Select(p => p.Clients.Select(c => c.Plates)))
                    .ToList();
            }
        }
    }
}
