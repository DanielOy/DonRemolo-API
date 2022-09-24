using Core.Entities;

namespace Core.Specifications
{
    public class ProductByCategoryIdSpec : BaseSpecification<Product>
    {
        public ProductByCategoryIdSpec(int categoryId) : base(x => 
        x.Category.Id == categoryId || x.Category.ParentId == categoryId)
        {

        }
    }
}
