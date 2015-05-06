using SPS.Model;
using SPS.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class LocalManagerBO : IBusiness<LocalManager>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(LocalManager localManager)
        {
            Context.LocalManagers.Add(localManager);
            Context.SaveChanges();
        }

        public virtual void Remove(LocalManager localManager)
        {
            Context.LocalManagers.Remove(localManager);
            Context.SaveChanges();
        }

        public virtual void Update(LocalManager localManager)
        {
            var savedCollaborator = Context.LocalManagers.SingleOrDefault(c => c.Id == localManager.Id);

            if (savedCollaborator != null)
            {
                savedCollaborator = localManager;
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
