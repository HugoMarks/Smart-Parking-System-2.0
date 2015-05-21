using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

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
                entity.Address = collaborator.Address;
                entity.Parking = collaborator.Parking;

                context.SaveChanges();
            }
        }

        public virtual Collaborator Find(string cpf)
        {
            Collaborator collaborator = null;

            using (var context = new SPSDb())
            {
                collaborator = context.Collaborators
                    .Include("Parking")
                    .Include("Address")
                    .SingleOrDefault(c => c.CPF == cpf);
            }

            return collaborator;
        }

        public virtual IList<Collaborator> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Collaborators.ToList();
            }
        }
    }
}

