using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PromotionRuleProduct
    {
        public int Id { get; set; }
        public int PromotionRuleItemId { get; set; }
        public virtual PromotionRuleItem PromotionRuleItem { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
