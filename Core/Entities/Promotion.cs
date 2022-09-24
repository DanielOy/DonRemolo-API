using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Promotion
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal PromotionalPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Picture { get; set; }

        public List<PromotionRuleItem> RuleItems { get; set; }
    }
}
