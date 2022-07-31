using System;

namespace Core.Entities
{
    public class BasketIngredient
    {
        public int Id { get; set; }
        public Guid BasketId { get; set; }
        public int BasketProductId { get; set; }
        public int? IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
