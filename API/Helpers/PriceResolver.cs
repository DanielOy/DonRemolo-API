using API.Dtos.Basket;
using AutoMapper;
using Core.Entities;
using System.Linq;

namespace API.Helpers
{
    public class PriceResolver : IValueResolver<BasketProduct, GetBasketProductDto, decimal>
    {
        public decimal Resolve(BasketProduct source, GetBasketProductDto destination, decimal destMember, ResolutionContext context)
        {
            decimal baseTotal = source.Product?.Price ?? 0.0m;
            decimal additionalsTotal = 0;
            additionalsTotal += source.Dough?.Price ?? 0;
            additionalsTotal += source.Size?.Price ?? 0;
            additionalsTotal += source.Ingredients?.Sum(x => x.Ingredient?.Price ?? 0) ?? 0;

            return baseTotal + additionalsTotal;
        }
    }
}
