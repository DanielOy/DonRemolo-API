using Core.Entities;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketByUserId(string userId);
        Task<Basket> GetBasketById(Guid id);
        Task<Basket> UpdateBasket(Basket order);
        Task DeleteBasket(Guid id);
        Task<string> ConfirmBasket(Guid basketId);
        Task<int> GetProductsCount(Guid basketId);
    }
}
