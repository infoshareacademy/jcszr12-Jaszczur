namespace TutorLizard.BusinessLogic.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserType UserType { get; set; }

    public User(int id, string name, UserType type)
    {
        this.Id = id;
        this.Name = name;
        this.UserType = type;
    }

    public User()
    {
    }
}
public enum UserType { Tutor, Student } 
