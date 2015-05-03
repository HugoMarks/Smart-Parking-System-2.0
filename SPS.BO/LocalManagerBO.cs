using SPS.Model;
using SPS.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class LocalManagerBO : IBusiness<LocalManager>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(LocalManager collaborator)
        {
            Context.LocalManagers.Add(collaborator);
            Context.SaveChanges();
        }

        public virtual void Remove(LocalManager collaborator)
        {
            Context.LocalManagers.Remove(collaborator);
            Context.SaveChanges();
        }

        public virtual void Update(LocalManager collaborator)
        {
            var savedCollaborator = Context.LocalManagers.SingleOrDefault(c => c.Id == collaborator.Id);

            if (savedCollaborator != null)
            {
                savedCollaborator = collaborator;
                Context.SaveChanges();
            }
        }

        public virtual LocalManager Find(int id)
        {
            return Context.LocalManagers.Find(id);
        }

        public virtual IList<LocalManager> FindAll()
        {
            return Context.LocalManagers.ToList();
        }
    }
}
