using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models.DTOs;
public class UserDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj nazwę użytkownika.")]
    [Display(Name = "Nazwa użytkownika")]
    [MinLength(5, ErrorMessage = "Minimalna długość: 5 znaków")]
    [MaxLength(20, ErrorMessage = "Maksymalna długość: 20 znaków")]
    public string Name { get; set; }

    [Display(Name = "Typ użytkownika")]
    public UserType UserType { get; set; }

    [Required(ErrorMessage = "Podaj adres email.")]
    [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email.")]
    [Display(Name = "Adres email")]
    public string Email { get; set; }

    [Display(Name = "Data rejestracji")]
    public DateTime DateCreated { get; set; } = DateTime.Now;

    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        UserType = user.UserType;
        Email = user.Email;
        DateCreated = user.DateCreated;
    }
}
