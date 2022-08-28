using System.Collections.Generic;

namespace API.Dtos.Basket
{
    public class GetBasketPromotionDto
    {
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Price * Quantity;

        public List<GetBasketPromotionItemDto> Items { get; set; }
    }
}
