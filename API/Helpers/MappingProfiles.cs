using API.Dtos;
using AutoMapper;
using Core.Entities;
using System.Linq;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destiny => destiny.CategoryName, origin => origin.MapFrom(s => s.Category.Name))
                .ForMember(destiny => destiny.Picture, origin => origin.MapFrom<ProductUrlResolver>());

            CreateMap<Ingredient, IngredientDto>()
                .ReverseMap();

            CreateMap<Dough, DoughDto>()
                .ReverseMap();

            CreateMap<Size, SizeDto>()
                .ReverseMap();

            CreateMap<Promotion, PromotionDto>()
                .ReverseMap();

            CreateMap<Promotion, PromotionViewDto>()
                .ForMember(destiny => destiny.Picture, origin => origin.MapFrom<PromotionUrlResolver>());

            CreateMap<Category, CategoryDto>()
                .ForMember(destiny => destiny.Picture, origin => origin.MapFrom<CategoryUrlResolver>());

            CreateMap<Basket, BasketDto>()
                .ReverseMap();

            CreateMap<BasketProductDto, BasketProduct>();

            CreateMap<BasketProduct, BasketProductDto>()
                .ForMember(destiny => destiny.Price, origin => origin.MapFrom<PriceResolver>());

            CreateMap<BasketIngredientDto, BasketIngredient>()
                .ReverseMap();

            CreateMap<Basket, Order>().ReverseMap();
            CreateMap<BasketProduct, OrderProduct>().ReverseMap();
            CreateMap<BasketIngredient, OrderIngredient>().ReverseMap();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderProduct, OrderProductDto>();
            CreateMap<OrderIngredient, OrderIngredientDto>();
        }
    }
}
