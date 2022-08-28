namespace Core.Entities
{
    public class PromotionItem
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Quantity { get; set; }

        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
    }
}
