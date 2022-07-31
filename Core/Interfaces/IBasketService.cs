using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetOrderByUserId(string userId);
        Task<Basket> GetOrderById(Guid id);
        Task<Basket> UpdateOrder(Basket order);
        Task DeleteOrder(Guid id);
    }
}
