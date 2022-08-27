using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class UserOrderWithProducts : BaseSpecification<Basket>
    {
        public UserOrderWithProducts(string userId) : base(x => x.UserId == userId)
        {
            AddInclude(q => q.Include(o => o.Products).ThenInclude(p => p.Product));

            AddOrderByDescending(x => x.CreationDate);
        }
    }
}
