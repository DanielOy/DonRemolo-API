using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllByExpression(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        Task<IEnumerable<T>> GetAllBySpecification(ISpecification<T> spec);
        Task<T> GetByID(object id);
        Task<T> GetBySpecification(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Insert(T entity);
        void Update(T entityToUpdate);
        void Delete(T entityToDelete);
        void Delete(object Id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Exists(Expression<Func<T, bool>> predicate = null);
    }
}
