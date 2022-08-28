using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class OrderPromotion
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public List<OrderPromotionItem> Items { get; set; }
    }
}
