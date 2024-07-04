using System.ComponentModel.DataAnnotations;
using TutorLizard.Shared.Enums;

namespace TutorLizard.Shared.Models.DTOs;
public class UserDto
{
    public int Id { get; set; }
    public bool? IsActive { get; set; }
    public string Name { get; set; }
    public UserType UserType { get; set; }
    public string Email { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public string? GoogleId { get; set; }

    public UserDto(int id, string name, UserType userType, string email, DateTime dateCreated, string? googleId)
    {
        Id = id;
        Name = name;
        UserType = userType;
        Email = email;
        DateCreated = dateCreated;
        GoogleId = googleId;
    }
}
