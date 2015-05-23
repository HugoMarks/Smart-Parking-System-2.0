using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class LocalManagerBO : IBusiness<LocalManager, string>
    {
        public virtual void Add(LocalManager localManager)
        {
            using (var context = new SPSDb())
            {
                if (this.Find(localManager.CPF) != null)
                {
                    throw new UniqueKeyViolationException(string.Format("There is already a local manager with CPF {0}.", localManager.CPF));
                }

                if (localManager.Address != null)
                {
                    localManager.Address = context.Addresses.SingleOrDefault(a => a.PostalCode == localManager.Address.PostalCode);
                }

                context.LocalManagers.Add(localManager);
                context.SaveChanges();
            }
        }

        public virtual void Remove(LocalManager localManager)
        {
            using (var context = new SPSDb())
            {
                context.LocalManagers.Remove(localManager);
                context.SaveChanges();
            }
        }

        public virtual void Update(LocalManager localManager)
        {
            using (var context = new SPSDb())
            {
                var entity = context.LocalManagers.SingleOrDefault(l => l.Email == localManager.Email);

                if (entity == null)
                    return;

                localManager.Id = entity.Id;
                context.Entry(entity).CurrentValues.SetValues(localManager);
                entity.Address = localManager.Address;

                context.SaveChanges();
            }
        }

        public virtual LocalManager Find(string cpf)
        {
            LocalManager localManager = null;

            using (var context = new SPSDb())
            {
                localManager = context.LocalManagers
                    .Include("Address")
                    .SingleOrDefault(c => c.CPF == cpf);
            }

            return localManager;
        }

        public virtual IList<LocalManager> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.LocalManagers.ToList();
            }
        }
    }
}
