using System;

namespace Core.Entities
{
    public class BasketPromotionItem
    {
        public int Id { get; set; }
        public Guid BasketId { get; set; }
        public Basket Basket { get; set; }
        public int BasketPromotionId { get; set; }
        public BasketPromotion BasketPromotion { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
