using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class AddressBO : IBusiness<Address>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(Address address)
        {
            Context.Addresses.Add(address);
            Context.SaveChanges();
        }

        public virtual Address Find(params object[] keys)
        {
            return Context.Addresses.Find(keys);
        }

        public virtual IList<Address> FindAll()
        {
            return Context.Addresses.ToList();
        }

        public virtual void Remove(Address address)
        {
            Context.Addresses.Remove(address);
            Context.SaveChanges();
        }

        public virtual void Update(Address address)
        {
            var entity = Context.Addresses.Find(address.PostalCode);

            if (entity == null)
                return;

            Context.Entry(entity).CurrentValues.SetValues(address);

            Context.SaveChanges();
        }
    }
}

