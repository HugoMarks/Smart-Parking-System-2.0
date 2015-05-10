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

        public virtual void Add(GlobalManager collaborator)
        {
            Context.GlobalManagers.Add(collaborator);
            Context.SaveChanges();
        }

        public virtual void Remove(GlobalManager collaborator)
        {
            Context.GlobalManagers.Remove(collaborator);
            Context.SaveChanges();
        }

        public virtual void Update(GlobalManager collaborator)
        {
            var savedGlobalManager = Context.GlobalManagers.SingleOrDefault(c => c.Id == collaborator.Id);

            if (savedGlobalManager != null)
            {
                savedGlobalManager = collaborator;
                Context.SaveChanges();
            }
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
