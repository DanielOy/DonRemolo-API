using API.Dtos;
using API.Dtos.Basket;
using AutoMapper;
using Core.Constants;
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
                .ForMember(destiny => destiny.ProductPrice, origin => origin.MapFrom<ProductPriceResolver>())
                .ForMember(destiny => destiny.IngredientsPrice, origin => origin.MapFrom<IngredientsPriceResolver>())
                .ForMember(destiny => destiny.IsDrink, origin => origin.MapFrom(s => s.Product.CategoryId == Categories.DrinkId));

            CreateMap<BasketIngredient, GetBasketIngredientDto>()
                .ForMember(destiny => destiny.IngredientName, origin => origin.MapFrom(s => s.Ingredient.Name))
                .ForMember(destiny => destiny.IngredientPrice, origin => origin.MapFrom(s => s.Ingredient.Price));

            CreateMap<BasketPromotion, GetBasketPromotionDto>()
                .ForMember(destiny => destiny.PromotionName, origin => origin.MapFrom(s => s.Promotion.Title))
                .ForMember(destiny => destiny.Price, origin => origin.MapFrom(s => s.Promotion.PromotionalPrice));

            CreateMap<BasketPromotionItem, GetBasketPromotionItemDto>()
                .ForMember(destiny => destiny.ProductName, origin => origin.MapFrom(s => s.Product.Name));

            CreateMap<SaveBasketDto, Basket>().ReverseMap();
            CreateMap<SaveBasketProductDto, BasketProduct>().ReverseMap();
            CreateMap<SaveBasketIngredientDto, BasketIngredient>().ReverseMap();
            CreateMap<SaveBasketPromotionDto, BasketPromotion>().ReverseMap();
            CreateMap<SaveBasketPromotionItemDto, BasketPromotionItem>().ReverseMap();

            CreateMap<BasketProduct, OrderProduct>()
                .ForMember(destiny => destiny.Price, origin => origin.MapFrom<BasketProductPriceResolver>());

            CreateMap<BasketPromotion, OrderPromotion>()
                .ForMember(destiny => destiny.Price, origin => origin.MapFrom(s => s.Promotion.PromotionalPrice));

            CreateMap<Order, Basket>().ReverseMap();
            CreateMap<OrderProduct, BasketProduct>();
            CreateMap<OrderIngredient, BasketIngredient>().ReverseMap();
            CreateMap<OrderPromotion, BasketPromotion>().ReverseMap();
            CreateMap<OrderPromotionItem, BasketPromotionItem>().ReverseMap();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderProduct, OrderProductDto>()
                .ForMember(destiny => destiny.ProductName, origin => origin.MapFrom(s => s.Product.Name));

            CreateMap<OrderIngredient, OrderIngredientDto>()
                .ForMember(destiny => destiny.IngredientName, origin => origin.MapFrom(s => s.Ingredient.Name));

            CreateMap<OrderPromotion, OrderPromotionDto>()
                .ForMember(destiny => destiny.PromotionName, origin => origin.MapFrom(s => s.Promotion.Title));

            CreateMap<OrderPromotionItem, OrderPromotionItemDto>()
                .ForMember(destiny => destiny.ProductName, origin => origin.MapFrom(s => s.Product.Name));
        }
    }
}
