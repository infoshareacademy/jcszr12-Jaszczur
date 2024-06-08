using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace TutorLizard.Web.Models;

public class LoginModel 
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    // Third-Party Login Providers
    public IEnumerable<AuthenticationScheme> AuthenticationSchemes { get; set; }
}
