using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS.BO
{
    public class GlobalManagerBO : IBusiness<GlobalManager>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(GlobalManager globalManager)
        {
            Context.GlobalManagers.Add(globalManager);
            Context.SaveChanges();
        }

        public virtual void Remove(GlobalManager globalManager)
        {
            Context.GlobalManagers.Remove(globalManager);
            Context.SaveChanges();
        }

        public virtual void Update(GlobalManager globalManager)
        {
            var entity = Context.GlobalManagers.SingleOrDefault(c => c.Email == globalManager.Email);

            if (entity == null)
                return;

            globalManager.Id = entity.Id;
            Context.Entry(entity).CurrentValues.SetValues(globalManager);
            entity.Address = globalManager.Address;

            Context.SaveChanges();
        }

        public virtual GlobalManager Find(params object[] keys)
        {
            return Context.GlobalManagers.Find(keys);
        }

        public virtual IList<GlobalManager> FindAll()
        {
            return Context.GlobalManagers.ToList();
        }
    }
}
