using Core.Entities;
using System;
using System.Linq;

namespace Core.Specifications
{
    public class ProductCountSpecification : BaseSpecification<Product>
    {
        public ProductCountSpecification(ProductSpecParams postSpecParams) : base(x =>
        (string.IsNullOrEmpty(postSpecParams.Search) || x.Description.ToLower().Contains(postSpecParams.Search))
        && (string.IsNullOrEmpty(postSpecParams.Category) || x.Category.Name.ToLower().Equals(postSpecParams.Category)))
        { }
    }
}
