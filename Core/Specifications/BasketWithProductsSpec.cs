using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Specifications
{
    public class BasketWithProductsSpec : BaseSpecification<Basket>
    {
        public BasketWithProductsSpec(Guid orderId) : base(x => x.Id == orderId)
        {
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Product));
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Dough));
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Size));
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Ingredients).ThenInclude(i => i.Ingredient));
            AddInclude(q => q.Include(o => o.Promotions).ThenInclude(p => p.Promotion));
            AddInclude(q => q.Include(o => o.Promotions).ThenInclude(p => p.Items).ThenInclude(i=>i.Product));
        }
    }
}
