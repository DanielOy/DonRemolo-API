using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork, StoreContext context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Basket> GetOrderByUserId(string userId)
        {
            var spec = new UserOrderWithProducts(userId);

            return await _unitOfWork.Baskets.GetBySpecification(spec);
        }

        public async Task<Basket> GetOrderById(Guid id)
        {
            var spec = new BasketWithProductsSpec(id);

            return await _unitOfWork.Baskets.GetBySpecification(spec);
        }

        public async Task DeleteOrder(Guid id)
        {
            _unitOfWork.Baskets.Delete(id);

            await _unitOfWork.Save();
        }

        public async Task<Basket> UpdateOrder(Basket basket)
        {
            bool orderExists = await _unitOfWork.Baskets.Exists(o => o.Id == basket.Id);

            await DeleteOldData(basket);

            basket.Products.ForEach(p => p.BasketId = basket.Id);
            basket.Products.ForEach(p => p.Ingredients.ForEach(i => i.BasketId = basket.Id));

            if (orderExists)
            {
                _unitOfWork.Baskets.Update(basket);
            }
            else
            {
                basket.CreationDate = DateTime.Now;
                _unitOfWork.Baskets.Insert(basket);
            }

            await _unitOfWork.Save();

            var spec = new BasketWithProductsSpec(basket.Id);

            return await _unitOfWork.Baskets.GetBySpecification(spec);
        }

        private async Task DeleteOldData(Basket basket)
        {
            var products = await _unitOfWork.BasketProducts.GetAllByExpression(x => x.BasketId == basket.Id);
            foreach (var product in products)
                _unitOfWork.BasketProducts.Delete(product);

            var ingredients = await _unitOfWork.BasketIngredients.GetAllByExpression(x => x.BasketId == basket.Id);
            foreach (var ingredient in ingredients)
                _unitOfWork.BasketIngredients.Delete(ingredient);
        }
    }
}
