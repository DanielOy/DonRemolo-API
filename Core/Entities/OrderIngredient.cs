using System;

namespace Core.Entities
{
    public class OrderIngredient
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int OrderProductId { get; set; }
        public int? IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
