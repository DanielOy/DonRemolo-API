using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destiny => destiny.CategoryName, origin => origin.MapFrom(s => s.Category.Name));
        }
    }
}
