using Core.Entities;
using Core.Interfaces;
using FluentValidation;

namespace Core.Validators
{
    public class BasketValidator : AbstractValidator<Basket>
    {
        private readonly IUnitOfWork _unitOfWork;
        public BasketValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BasketValidator()
        {
            RuleForEach(x => x.Products)
                .ChildRules(product => product.RuleFor(x => x.ProductId).MustAsync(
                    async (productBasket, productId, token) =>
                    {
                        if (productId is null || productId == 0)
                        {
                            return productBasket.SizeId == null && productBasket.DoughId == null;
                        }
                        else
                        {
                            var product = await _unitOfWork.Products.GetByID(productId);
                            var size = await _unitOfWork.Sizes.GetByID(productBasket.SizeId);
                            var dough = await _unitOfWork.Doughs.GetByID(productBasket.DoughId);

                            return product.CategoryId == size.CategoryId && product.CategoryId == dough.CategoryId;
                        }
                    }).WithMessage("Size Or Dough can't be used"));
        }
    }
}
