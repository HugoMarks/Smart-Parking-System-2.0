﻿using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS.BO
{
    public class GlobalManagerBO : IBusiness<GlobalManager, string>
    {
        public virtual void Add(GlobalManager globalManager)
        {
            using (var context = new SPSDb())
            {
                if (this.Find(globalManager.CPF) != null)
                {
                    throw new UniqueKeyViolationException(string.Format("There is already a global manager with CPF {0}.", globalManager.CPF));
                }

                context.GlobalManagers.Add(globalManager);
                context.SaveChanges();
            }
        }

        public virtual void Remove(GlobalManager globalManager)
        {
            using (var context = new SPSDb())
            {
                context.GlobalManagers.Remove(globalManager);
                context.SaveChanges();
            }
        }

        public virtual void Update(GlobalManager globalManager)
        {
            using (var context = new SPSDb())
            {
                var entity = context.GlobalManagers.SingleOrDefault(c => c.Email == globalManager.Email);

                if (entity == null)
                    return;

                globalManager.Id = entity.Id;
                context.Entry(entity).CurrentValues.SetValues(globalManager);
                entity.Address = globalManager.Address;

                context.SaveChanges();
            }
        }

        public virtual GlobalManager Find(string cpf)
        {
            GlobalManager globalManager = null;

            using (var context = new SPSDb())
            {
                globalManager = context.GlobalManagers
                    .Include("Address")
                    .SingleOrDefault(c => c.CPF == cpf);
            }

            return globalManager;
        }

        public virtual IList<GlobalManager> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.GlobalManagers.ToList();
            }
        }
    }
}
