using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venn.Data.Repos
{
    public interface IRepository<T>
    {
        T Get(int id);

        IQueryable<T> GetAll();

        void Update(T entity);

        void Add(T entity);

        void Delete(T entity);

        void SaveChanges();
    }
}
