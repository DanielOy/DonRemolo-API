using Core.Entities;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Category> Categories { get; }
        Task Save();
    }
}
