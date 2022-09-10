using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CommentDto
    {
        [Required]
        [Range(1, 5)]
        public int Calification { get; set; }
        
        public string Category { get; set; }
        
        [Required]
        public string Comments { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
