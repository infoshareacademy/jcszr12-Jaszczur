using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
public enum UserType { Tutor, Student } 
