using Core.Entities;

namespace Core.Specifications
{
    public class ProductMostPopularSpecification : BaseSpecification<Product>
    {
        public ProductMostPopularSpecification() : base(p => p.MostPopular == true)
        {
            AddInclude(x => x.Category);
        }
    }
}
