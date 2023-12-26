using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUnitOfWork
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        List<TEntity> GetAll();

        TEntity GetByName(string name);

        void Insert(TEntity entity);

        void Delete(string name);

        void Update(TEntity entity);

        void Save();
    }
}
