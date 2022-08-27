namespace Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool MostPopular { get; set; }
        public Category Category { get; set; }
    }
}
