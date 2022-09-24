using System.Collections.Generic;

namespace API.Dtos.Promotion
{
    public class PromotionRuleItemDto
    {
        public int? CategoryId { get; set; }
        public string GroupName { get; set; }
        public int Quantity { get; set; }
        public int? SizeId { get; set; }
        public string Size { get; set; }

        public List<PromotionRuleProductDto> Products { get; set; }
    }
}
