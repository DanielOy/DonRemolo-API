using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ExternalAuthDto
    {
        [Required]
        public string Token { get; set; }
    }
}
