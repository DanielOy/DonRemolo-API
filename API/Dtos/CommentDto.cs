using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CommentDto
    {
        [Required]
        [Range(1, 5)]
        public int Calification { get; set; }
        
        public List<string> Categories { get; set; }
        
        [Required]
        public string Comments { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
