using System.Collections.Generic;

namespace API.Dtos
{
    public class BasketProductDto
    {
        public int? ProductId { get; set; }
        public int? PromotionId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Price * Quantity;

        public int? DoughId { get; set; }
        public int? SizeId { get; set; }
        public List<BasketIngredientDto> Ingredients { get; set; }
    }
}
