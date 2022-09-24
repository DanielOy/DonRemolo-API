using System.Collections.Generic;

namespace Core.Entities
{
    public class PromotionRuleItem
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }

        public int? CategoryId { get; set; }
        
        public string GroupName { get; set; }
        public int Quantity { get; set; }
        public int? SizeId { get; set; }
        public virtual Size Size { get; set; }

        public List<PromotionRuleProduct> Products { get; set; }
    }
}
