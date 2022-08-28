using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Dtos.Basket
{
    public class GetBasketDto
    {
        public GetBasketDto()
        { }
        public GetBasketDto(string id)
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

        public decimal Total => (Products?.Sum(x => x.SubTotal) ?? 0.0m) + (Promotions?.Sum(x => x.SubTotal) ?? 0.0m);

        public List<GetBasketProductDto> Products { get; set; }
        public List<GetBasketPromotionDto> Promotions { get; set; }
    }
}
