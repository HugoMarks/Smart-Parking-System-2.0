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
            var savedCollaborator = Context.Collaborators.SingleOrDefault(c => c.Id == collaborator.Id);

            if (savedCollaborator != null)
            {
                savedCollaborator = collaborator;
                Context.SaveChanges();
            }
        }

        public virtual Collaborator Find(int id)
        {
            return Context.Collaborators.Find(id);
        }

        public virtual IList<Collaborator> FindAll()
        {
            return Context.Collaborators.ToList();
        }
    }
}

