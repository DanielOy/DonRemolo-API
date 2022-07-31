using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Dtos
{
    public class BasketDto
    {
        public BasketDto()
        { }
        public BasketDto(string id)
        {
            Id = id;
        }

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

        public decimal Total => Products?.Sum(x => x.SubTotal) ?? 0.0m;

        public List<BasketProductDto> Products { get; set; }
    }
}
