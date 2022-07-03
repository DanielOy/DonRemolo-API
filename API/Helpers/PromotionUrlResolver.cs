using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class PromotionUrlResolver : IValueResolver<Promotion, PromotionViewDto, string>
    {
        private readonly IConfiguration _configuration;

        public PromotionUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Promotion source, PromotionViewDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Picture))
            {
                return $"{_configuration["ApiUrl"]}images/{source.Picture}";
            }

            return null;
        }
    }
}
