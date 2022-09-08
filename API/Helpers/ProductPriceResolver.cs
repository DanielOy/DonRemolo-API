using API.Dtos.Basket;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class ProductPriceResolver : IValueResolver<BasketProduct, GetBasketProductDto, decimal>
    {
        public decimal Resolve(BasketProduct source, GetBasketProductDto destination, decimal destMember, ResolutionContext context)
        {
            decimal total = source.Product.Price;
            total += source.Dough?.Price ?? 0;
            total += source.Size?.Price ?? 0;
            return total;
        }
    }
}
