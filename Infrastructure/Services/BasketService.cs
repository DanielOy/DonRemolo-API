using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Core.Validators;
using Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BasketService(IUnitOfWork unitOfWork, StoreContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Basket> GetBasketByUserId(string userId)
        {
            var spec = new UserOrderWithProducts(userId);

            return await _unitOfWork.Baskets.GetBySpecification(spec);
        }

        public async Task<Basket> GetBasketById(Guid id)
        {
            var spec = new BasketWithProductsSpec(id);

            return await _unitOfWork.Baskets.GetBySpecification(spec);
        }

        public async Task DeleteBasket(Guid id)
        {
            _unitOfWork.Baskets.Delete(id);

            await _unitOfWork.Save();
        }

        public async Task<Basket> UpdateBasket(Basket basket)
        {
            bool orderExists = await _unitOfWork.Baskets.Exists(o => o.Id == basket.Id);

            await DeleteOldData(basket);

            basket.Products?.ForEach(p => p.BasketId = basket.Id);
            basket.Products?.ForEach(p => p.Ingredients?.ForEach(i => i.BasketId = basket.Id));
            basket.Promotions?.ForEach(p => p.BasketId = basket.Id);
            basket.Promotions?.ForEach(p => p.Items?.ForEach(i => i.BasketId = basket.Id));

            if (orderExists)
            {
                _unitOfWork.Baskets.Update(basket);
            }
            else
            {
                basket.CreationDate = DateTime.Now;

                var validator = new BasketValidator(_unitOfWork);
                var validationResult = await validator.ValidateAsync(basket);
                if (validationResult.IsValid)
                    _unitOfWork.Baskets.Insert(basket);
                else
                    throw new Exception(string.Join('\n', validationResult.Errors.Select(x => x.ErrorMessage)));
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

            var promotions = await _unitOfWork.BasketPromotions.GetAllByExpression(x => x.BasketId == basket.Id);
            foreach (var promotion in promotions)
                _unitOfWork.BasketPromotions.Delete(promotion);

            var promotionItems = await _unitOfWork.BasketPromotionItems.GetAllByExpression(x => x.BasketId == basket.Id);
            foreach (var promotionItem in promotionItems)
                _unitOfWork.BasketPromotionItems.Delete(promotionItem);
        }

        public async Task<string> ConfirmBasket(Guid basketId)
        {
            var basket = await GetBasketById(basketId);
            var orderId = Guid.NewGuid();

            if (basket is null)
                return String.Empty;

            var order = _mapper.Map<Order>(basket);
            order.Status = Order.OrderStatus.Ordered;
            order.Id = orderId;
            order.Total = (order.Products?.Sum(x => x.Price * x.Quantity) ?? 0)
                + (order.Promotions?.Sum(x => x.Price * x.Quantity) ?? 0);

            _unitOfWork.Orders.Insert(order);
            await DeleteBasket(basketId);

            return orderId.ToString();
        }
    }
}
