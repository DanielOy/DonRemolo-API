using Core.Entities;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Ingredient> Ingredients { get; }
        public IGenericRepository<Dough> Doughs { get; }
        public IGenericRepository<Size> Sizes { get; }
        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<Promotion> Promotions { get; }
        public IGenericRepository<PromotionItem> PromotionItems { get; }

        public IGenericRepository<Basket> Baskets { get; }
        public IGenericRepository<BasketProduct> BasketProducts { get; }
        public IGenericRepository<BasketIngredient> BasketIngredients { get; }
        public IGenericRepository<BasketPromotion> BasketPromotions { get; }
        public IGenericRepository<BasketPromotionItem> BasketPromotionItems { get; }

        public IGenericRepository<Order> Orders { get; }
        public IGenericRepository<OrderProduct> OrderProducts { get; }
        public IGenericRepository<OrderIngredient> OrderIngredients { get; }
        public IGenericRepository<OrderPromotion> OrderPromotions { get; }
        public IGenericRepository<OrderPromotionItem> OrderPromotionItems { get; }

        public IGenericRepository<Comment> Comments { get; }

        Task Save();
    }
}
