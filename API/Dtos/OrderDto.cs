using System;
using System.Collections.Generic;
using static Core.Entities.Order;

namespace API.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public bool AtHome { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }

        public bool DelayDelivery { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsCashPayment { get; set; }
        public decimal Cash { get; set; }

        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderProductDto> Products { get; set; }
        public List<OrderPromotionDto> Promotions { get; set; }
    }
}
