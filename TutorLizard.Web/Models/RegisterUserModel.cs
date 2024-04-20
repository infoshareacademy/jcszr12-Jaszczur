using System.ComponentModel.DataAnnotations;
using TutorLizard.BusinessLogic.Enums;

namespace TutorLizard.Web.Models
{
    public class RegisterUserModel
    {
        public string? UserName { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public UserType Type { get; set; }
        public string? Email { get; set; }
    }
}
