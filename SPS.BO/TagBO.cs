using SPS.BO.Exceptions;
using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SPS.BO
{
    public class TagBO : IBusiness<Tag, string>
    {
        public virtual void Add(Tag tag)
        {
            using (var context = new SPSDb())
            {
                if (context.Tags.Any(t => t.Id == tag.Id))
                {
                    throw new UniqueKeyViolationException("Tag já vinculada");
                }
                
                if (tag.Client != null)
                {
                    tag.Client = context.Clients.Include(c => c.Tags).SingleOrDefault(c => c.CPF == tag.Client.CPF);

                    if (tag.Client.Tags.Count > 5)
                    {
                        throw new MaximumLimitReachedException("Número máximo de tags atingido");
                    }
                }

                context.Tags.Add(tag);
                context.SaveChanges();
            }
        }

        public virtual void Remove(Tag tag)
        {
            using (var context = new SPSDb())
            {
                context.Tags.Remove(tag);
                context.SaveChanges();
            }
        }

        public virtual void Update(Tag tag)
        {
            using (var context = new SPSDb())
            {
                var entity = context.Tags.Find(tag.Id);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(tag);
                entity.Client = tag.Client;

                context.SaveChanges();
            }
        }

        public virtual Tag Find(string id)
        {
            Tag tag = null;

            using (var context = new SPSDb())
            {
                tag = context.Tags
                    .Include(t => t.Client.Parkings.Select(p => p.Clients.Select(c => c.Tags)))
                    .SingleOrDefault(c => c.Id == id);
            }

            return tag;
        }

        public virtual IList<Tag> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Tags
                    .Include(t => t.Client.Parkings.Select(p => p.Clients.Select(c => c.Tags)))
                    .ToList();
            }
        }

    }
}

