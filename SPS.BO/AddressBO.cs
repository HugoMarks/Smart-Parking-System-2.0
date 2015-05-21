using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class AddressBO : IBusiness<Address, string>
    {
        public virtual void Add(Address address)
        {
            using (var context = new SPSDb())
            {
                context.Addresses.Add(address);
                context.SaveChanges();
            }
        }

        public virtual Address Find(string cep)
        {
            Address address = null;

            using (var context = new SPSDb())
            {
                address = context.Addresses.SingleOrDefault(a => a.PostalCode == cep);
            }

            return address;
        }
        public virtual IList<Address> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Addresses.ToList();
            }
        }

        public virtual void Remove(Address address)
        {
            using (var context = new SPSDb())
            {
                context.Addresses.Remove(address);
                context.SaveChanges();
            }
        }

        public virtual void Update(Address address)
        {
            using (var context = new SPSDb())
            {
                var entity = context.Addresses.Find(address.PostalCode);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(address);

                context.SaveChanges();
            }
        }
    }
}

