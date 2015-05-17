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
            var entity = Context.LocalManagers.SingleOrDefault(l => l.Email == localManager.Email);

            if (entity == null)
                return;

            localManager.Id = entity.Id;
            Context.Entry(entity).CurrentValues.SetValues(localManager);
            entity.Address = localManager.Address;

            Context.SaveChanges();
        }

        public virtual LocalManager Find(params object[] keys)
        {
            return Context.LocalManagers.Find(keys);
        }

        public virtual IList<LocalManager> FindAll()
        {
            return Context.LocalManagers.ToList();
        }
    }
}
