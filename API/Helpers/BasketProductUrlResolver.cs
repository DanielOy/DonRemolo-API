using API.Dtos.Basket;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class BasketProductUrlResolver : IValueResolver<BasketProduct, GetBasketProductDto, string>
    {

        private readonly IConfiguration _configuration;

        public BasketProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(BasketProduct source, GetBasketProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source?.Product?.Picture))
            {
                return $"{_configuration["ApiUrl"]}images/{source.Product.Picture}";
            }

            return null;
        }
    }
}
