namespace Core.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public double? BasePrice { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
