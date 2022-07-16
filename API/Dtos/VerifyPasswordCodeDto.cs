using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class VerifyPasswordCodeDto
    {
        [Required]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
           ErrorMessage = "The email hasn't a valid format")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{4}", ErrorMessage = "The reset password code hasn't a valid format")]
        public string Code { get; set; }
    }
}
