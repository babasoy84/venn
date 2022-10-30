using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Models.Models.Abstracts;

namespace Venn.Data.Repos
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private VennDbContext _context;

        private DbSet<T> _dbSet;

        public Repository(VennDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            if (entity != null)
            {
                _dbSet.Add(entity);
            }
        }

        public void Delete(T entity)
        {
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public T Get(int id)
        {
            if (id > 0)
            {
                return _dbSet.FirstOrDefault(e => e.Id == id);
            }
            return null;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                _dbSet.Update(entity);
            }
        }
    }
}
