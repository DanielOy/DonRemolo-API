using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class ProductMostPopularSpecification : BaseSpecification<Product>
    {
        public ProductMostPopularSpecification() : base(p => p.MostPopular == true)
        {
            AddInclude(q => q.Include(x => x.Category));
        }
    }
}
