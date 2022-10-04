﻿using Core.Entities;

namespace Core.Specifications
{
    public class ProductCountSpecification : BaseSpecification<Product>
    {
        public ProductCountSpecification(ProductSpecParams postSpecParams) : base(x =>
        (string.IsNullOrEmpty(postSpecParams.Search)
        || x.Description.ToLower().Contains(postSpecParams.Search))
        && (string.IsNullOrEmpty(postSpecParams.Category)
            || x.Category.Name.ToLower().Equals(postSpecParams.Category))
        || (x.Category.ParentId != null
            && x.Category.ParentCategory.Name.ToLower().Equals(postSpecParams.Category)))
        { }
    }
}
