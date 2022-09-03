using System.Collections.Generic;

namespace API.Dtos.Basket
{
    public class GetBasketProductDto
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ProductRelationNumber { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Price * Quantity;

        public int? DoughId { get; set; }
        public string DoughName { get; set; }
        public int? SizeId { get; set; }
        public string SizeName { get; set; }
        public bool IsDrink { get; set; }
        public List<GetBasketIngredientDto> Ingredients { get; set; }
    }
}
