using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class CategoryUrlResolver : IValueResolver<Category, CategoryDto, string>
    {
        private readonly IConfiguration _configuration;

        public CategoryUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Category source, CategoryDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Picture))
            {
                return $"{_configuration["ApiUrl"]}images/{source.Picture}";
            }

            return null;
        }
    }
}
