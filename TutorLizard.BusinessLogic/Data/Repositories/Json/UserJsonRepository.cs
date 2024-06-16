using Microsoft.Extensions.Options;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Options;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json;
public class UserJsonRepository : JsonRepositoryBase<User>, IUserRepository
{
    
    

    public UserJsonRepository(IOptions<DataJsonFilePaths> options) : base(options.Value.Users)
    {
        
    }

    public User CreateUser(string name, UserType type, string email, string passwordHash, string googleid)
    {
        User newUser = new(GetNewId(), name, type, email, passwordHash, googleid);
        Data.Add(newUser);
        SaveToJson();

        return newUser;
    }

    public List<User> GetAllUsers()
    {
        return Data;
    }

    public User? GetUserById(int id)
    {
        return Data.Find(x => x.Id == id);
    }

    public void UpdateUser(User user)
    {
        var toUpdate = GetUserById(user.Id);
        if (toUpdate is null)
            return;

        toUpdate.Name = user.Name;
        toUpdate.UserType = user.UserType;
        toUpdate.Email = user.Email;
        toUpdate.PasswordHash = user.PasswordHash;

        SaveToJson();
    }

    public void DeleteUserById(int id)
    {
        var toDelete = GetUserById(id);
        if (toDelete is null)
            return;
        Data.Remove(toDelete);

        SaveToJson();
    }

    private int GetNewId()
    {
        if (Data.Any())
            return Data.Max(x => x.Id) + 1;

        return 1;
    }

    

}
