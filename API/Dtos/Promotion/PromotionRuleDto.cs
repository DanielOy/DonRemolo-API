using System.Collections.Generic;

namespace API.Dtos.Promotion
{
    public class PromotionRuleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<PromotionRuleItemDto> RuleItems { get; set; }
    }
}
