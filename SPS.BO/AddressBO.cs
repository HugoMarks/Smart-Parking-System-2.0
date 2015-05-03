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

        public virtual Address Find(int id)
        {
            return Context.Addresses.Find(id);
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
            var savedAddress = Context.Addresses.SingleOrDefault(a => a.PostalCode == address.PostalCode);

            if (savedAddress != null)
            {
                savedAddress = address;
                Context.SaveChanges();
            }
        }
    }
}

