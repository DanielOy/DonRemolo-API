using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Basket
{
    public class SaveBasketDto
    {
        [Required]
        public string Id { get; set; }
        public string UserId { get; set; }
        public bool AtHome { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }

        public bool DelayDelivery { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public bool IsCashPayment { get; set; }
        public decimal? Cash { get; set; }

        public List<SaveBasketProductDto> Products { get; set; }
    }
}
