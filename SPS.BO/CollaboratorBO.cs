using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class CollaboratorBO : IBusiness<Collaborator>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(Collaborator collaborator)
        {
            Context.Collaborators.Add(collaborator);
            Context.SaveChanges();
        }

        public virtual void Remove(Collaborator collaborator)
        {
            Context.Collaborators.Remove(collaborator);
            Context.SaveChanges();
        }

        public virtual void Update(Collaborator collaborator)
        {
            var entity = Context.Collaborators.SingleOrDefault(c => c.Email == collaborator.Email);

            if (entity == null)
                return;

            collaborator.Id = entity.Id;
            Context.Entry(entity).CurrentValues.SetValues(collaborator);
            entity.Address = collaborator.Address;
            entity.Parking = collaborator.Parking;

            Context.SaveChanges();
        }

        public virtual Collaborator Find(params object[] keys)
        {
            return Context.Collaborators.Find(keys);
        }

        public virtual IList<Collaborator> FindAll()
        {
            return Context.Collaborators.ToList();
        }
    }
}

