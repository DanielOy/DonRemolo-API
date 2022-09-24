using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class PromotionWithRulesSpec:BaseSpecification<Promotion>
    {
        public PromotionWithRulesSpec(int id):base(x=>x.Id== id)
        {
            AddInclude(q => q.Include(x => x.RuleItems).ThenInclude(x => x.Products).ThenInclude(x => x.Product));
            AddInclude(q => q.Include(x => x.RuleItems).ThenInclude(x => x.Size));
        }
    }
}
