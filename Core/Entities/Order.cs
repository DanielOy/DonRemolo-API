﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool AtHome { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public bool DelayDelivery { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsCashPayment { get; set; }
        public decimal Cash { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderProduct> Products { get; set; }

        public enum OrderStatus
        {
            Ordered,
            Elaboration,
            Ready,
            OnWay,
            Delivered
        }
    }
}
