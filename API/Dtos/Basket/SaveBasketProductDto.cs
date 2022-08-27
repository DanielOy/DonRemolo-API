using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Basket
{
    public class SaveBasketProductDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public int? DoughId { get; set; }
        public int? SizeId { get; set; }
        public List<SaveBasketIngredientDto> Ingredients { get; set; }
    }
}
