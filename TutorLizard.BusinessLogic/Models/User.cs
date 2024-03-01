using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserType UserType { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
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
}
public enum UserType { Tutor, Student, Admin } 
