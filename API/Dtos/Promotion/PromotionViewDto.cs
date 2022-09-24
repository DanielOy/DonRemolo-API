namespace API.Dtos.Promotion
{
    public class PromotionViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PromotionalPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Picture { get; set; }
    }
}
