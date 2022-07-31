using Core.Entities;
using Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private bool disposed = false;

        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<Promotion> Promotions { get; }
        public IGenericRepository<Basket> Baskets { get; set; }
        public IGenericRepository<BasketProduct> BasketProducts { get; set; }
        public IGenericRepository<BasketIngredient> BasketIngredients { get; set; }

        public UnitOfWork(StoreContext context)
        {
            _context = context;

            Products = new GenericRepository<Product>(_context);
            Categories = new GenericRepository<Category>(_context);
            Promotions = new GenericRepository<Promotion>(_context);
            Baskets = new GenericRepository<Basket>(_context);
            BasketProducts = new GenericRepository<BasketProduct>(_context);
            BasketIngredients = new GenericRepository<BasketIngredient>(_context);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
