using System.ComponentModel.DataAnnotations;

namespace Api_Sport_Business_Logic.Models.Dtos
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
