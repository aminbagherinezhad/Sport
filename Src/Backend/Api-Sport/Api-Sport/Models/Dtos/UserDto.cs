using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api_Sport.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("نام کاربری")]
        public string? UserName { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(15)]
        public string? Phone { get; set; }
        [MaxLength(50)]
        public string? Avatar { get; set; }

        public bool? IsBlock { get; set; }
    }
}
