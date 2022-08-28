using System;

namespace Core.Entities
{
    public class OrderPromotionItem
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int OrderPromotionId { get; set; }
        public OrderPromotion OrderPromotion { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
