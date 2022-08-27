using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PromotionDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal PromotionalPrice { get; set; }
        public decimal OriginalPrice { get; set; }
    }
}
