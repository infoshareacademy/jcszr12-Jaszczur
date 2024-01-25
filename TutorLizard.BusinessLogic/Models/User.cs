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
    public static int GetID()
    {
        var filePath = $@"{Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName}\User.json";

        var jsonData = System.IO.File.ReadAllText(filePath);
        var userList = JsonConvert.DeserializeObject<List<User>>(jsonData)
                ?? new List<User>();

        if (userList.Count > 0)
            return userList.Max(x => x.Id) + 1;
        else
            return 0;
    }
}
public enum UserType { Tutor, Student } 
