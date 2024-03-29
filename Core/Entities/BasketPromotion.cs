﻿using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class BasketPromotion
    {
        public int Id { get; set; }
        public Guid BasketId { get; set; }
        public Basket Basket { get; set; }
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int Quantity { get; set; }

        public List<BasketPromotionItem> Items { get; set; }
    }
}
