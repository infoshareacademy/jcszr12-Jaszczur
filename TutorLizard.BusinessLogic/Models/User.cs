using System.ComponentModel.DataAnnotations;
using TutorLizard.BusinessLogic.Enums;

namespace TutorLizard.BusinessLogic.Models;

public class User
{
    public int Id { get; set; }
    public bool? IsActive { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(40)]
    public string Name { get; set; }

    public UserType UserType { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [MaxLength(100)]
    public string PasswordHash { get; set; }

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
    public string ActivationCode { get; set; }
}
