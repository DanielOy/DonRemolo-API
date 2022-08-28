using System.Collections.Generic;

namespace API.Dtos.Basket
{
    public class SaveBasketPromotionDto
    {
        public int PromotionId { get; set; }
        public int Quantity { get; set; }

        public List<SaveBasketPromotionItemDto> Items { get; set; }
    }
}
