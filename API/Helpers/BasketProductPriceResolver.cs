using AutoMapper;
using Core.Entities;
using System.Linq;

namespace API.Helpers
{
    public class BasketProductPriceResolver : IValueResolver<BasketProduct, OrderProduct, decimal>
    {
        public decimal Resolve(BasketProduct source, OrderProduct destination, decimal destMember, ResolutionContext context)
        {
            decimal additionalsTotal = 0;
            additionalsTotal += source.Size?.Price ?? 0;
            additionalsTotal += source.Ingredients?.Sum(x => x.Ingredient?.Price ?? 0) ?? 0;

            return additionalsTotal;
        }
    }
}
