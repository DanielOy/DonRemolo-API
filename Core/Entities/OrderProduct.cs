using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int Quantity { get; set; }
        public int? DoughId { get; set; }
        public Dough Dough { get; set; }
        public int? SizeId { get; set; }
        public Size Size { get; set; }
        public List<OrderIngredient> Ingredients { get; set; }
    }
}
