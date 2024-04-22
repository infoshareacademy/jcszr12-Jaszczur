using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj nazwę użytkownika.")]
    [Display(Name = "Nazwa użytkownika")]
    [MinLength(5, ErrorMessage = "Minimalna długość: 5 znaków")]
    [MaxLength(20, ErrorMessage = "Maksymalna długość: 20 znaków")]
    public string Name { get; set; }

    [Display(Name="Typ użytkownika")]
    public UserType UserType { get; set; }

    [Required(ErrorMessage = "Podaj adres email.")]
    [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email.")]
    [Display(Name = "Adres email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Podaj hasło.")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    [MinLength(5, ErrorMessage = "Minimalna długość: 5 znaków")]
    public string PasswordHash { get; set; }

    [Display(Name = "Data rejestracji")]
    public DateTime DateCreated { get; set; } = DateTime.Now;

    public User(int id, string name, UserType userType, string email, string passwordHash)
    {
        Id = id;
        Name = name;
        UserType = userType;
        Email = email;
        PasswordHash = passwordHash;
    }
    public User()
    {
    }

    // Entity Framework
    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
    public ICollection<AdRequest> AdRequests { get; set; } = new List<AdRequest>();
    public ICollection<ScheduleItemRequest> ScheduleItemRequests { get; set; } = new List<ScheduleItemRequest>();
}
public enum UserType { Tutor, Student, Admin } 
