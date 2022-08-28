using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Specifications
{
    public class OrderWithProductsSpec : BaseSpecification<Order>
    {
        public OrderWithProductsSpec() : base()
        {
            IncludeDetails();
        }

        public OrderWithProductsSpec(Guid orderId) : base(x => x.Id == orderId)
        {
            IncludeDetails();
        }

        private void IncludeDetails()
        {
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Product));
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Dough));
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Size));
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Ingredients).ThenInclude(i => i.Ingredient));
            AddInclude(q => q.Include(o => o.Promotions).ThenInclude(p => p.Promotion));
            AddInclude(q => q.Include(o => o.Promotions).ThenInclude(p => p.Items).ThenInclude(i => i.Product));
        }
    }
}
