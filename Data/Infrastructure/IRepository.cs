using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public interface IRepository<T> where T : AppBaseEntity
    {
        // Marks an entity as new
        T Add(T entity);
        // Marks a collection of entities entity as new
        IEnumerable<T> Add(IEnumerable<T> entities);
        // Marks an entity as modified
        T Update(T entity);
        // Marks a collection of entities entity as modified
        IEnumerable<T> Update(IEnumerable<T> entities);
        // Marks an entity to be removed
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Delete(Expression<Func<T, bool>> where);
        // Get an entity by int id
        T GetById(int id);
        // Get an entity using delegate
        T Get(Expression<Func<T, bool>> where);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        // Gets all entities of type T
        IEnumerable<T> GetAll();
        // Gets entities using delegate
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        IEnumerable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where);
        T GetAsNoTracking(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAllAsNoTracking();
    }
}
