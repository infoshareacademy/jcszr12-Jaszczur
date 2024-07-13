using System.ComponentModel.DataAnnotations;
using TutorLizard.Shared.Enums;

namespace TutorLizard.Web.Models
{
    public class RegisterUserModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(40)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; }

        public UserType Type { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
