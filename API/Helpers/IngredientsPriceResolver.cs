using API.Dtos.Basket;
using AutoMapper;
using Core.Entities;
using System.Linq;

namespace API.Helpers
{
    public class IngredientsPriceResolver : IValueResolver<BasketProduct, GetBasketProductDto, decimal>
    {
        public decimal Resolve(BasketProduct source, GetBasketProductDto destination, decimal destMember, ResolutionContext context)
        {
            return source.Ingredients?.Sum(x => x.Ingredient?.Price ?? 0) ?? 0;
        }
    }
}
