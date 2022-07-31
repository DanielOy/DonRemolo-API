using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class ProductPaginationSpecification : BaseSpecification<Product>
    {
        public ProductPaginationSpecification(ProductSpecParams postSpecParams) : base(x =>
        (string.IsNullOrEmpty(postSpecParams.Search) || x.Description.ToLower().Contains(postSpecParams.Search))
        && (string.IsNullOrEmpty(postSpecParams.Category) || x.Category.Name.ToLower().Equals(postSpecParams.Category)))
        {
            ApplyPagging(postSpecParams.PageSize * (postSpecParams.PageIndex - 1), postSpecParams.PageSize);

            if (!string.IsNullOrWhiteSpace(postSpecParams.Sort))
            {
                switch (postSpecParams.Sort)
                {
                    case "NameAsc":
                        AddOrderBy(x => x.Name);
                        break;
                    case "NameDesc":
                        AddOrderByDescending(x => x.Name);
                        break;
                    case "PriceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Id);
                        break;
                }
            }

            AddInclude(x => x.Include(x => x.Category));
        }
    }
}
