using API.Dtos;
using API.Dtos.Basket;
using AutoMapper;
using Core.Entities;

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

            CreateMap<Basket, GetBasketDto>()
                .ReverseMap();

            CreateMap<BasketProduct, GetBasketProductDto>()
                .ForMember(destiny => destiny.ProductName, origin => origin.MapFrom(s => s.Product.Name))
                .ForMember(destiny => destiny.ProductImage, origin => origin.MapFrom<BasketProductUrlResolver>())
                .ForMember(destiny => destiny.DoughName, origin => origin.MapFrom(s => s.Dough.Name))
                .ForMember(destiny => destiny.SizeName, origin => origin.MapFrom(s => s.Size.Name))
                .ForMember(destiny => destiny.Price, origin => origin.MapFrom<PriceResolver>());

            CreateMap<BasketIngredient, GetBasketIngredientDto>()
                .ForMember(destiny => destiny.IngredientName, origin => origin.MapFrom(s => s.Ingredient.Name))
                .ForMember(destiny => destiny.IngredientPrice, origin => origin.MapFrom(s => s.Ingredient.Price));

            CreateMap<SaveBasketDto, Basket>().ReverseMap();
            CreateMap<SaveBasketProductDto, BasketProduct>().ReverseMap();
            CreateMap<SaveBasketIngredientDto, BasketIngredient>().ReverseMap();

            CreateMap<Basket, Order>().ReverseMap();
            CreateMap<BasketProduct, OrderProduct>().ReverseMap();
            CreateMap<BasketIngredient, OrderIngredient>().ReverseMap();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderProduct, OrderProductDto>();
            CreateMap<OrderIngredient, OrderIngredientDto>();
        }
    }
}
