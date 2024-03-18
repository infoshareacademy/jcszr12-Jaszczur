using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json;
public class UserJsonRepository : JsonRepositoryBase<User>, IUserRepository
{
    protected override string FilePath => "Data/users.json";
    public UserJsonRepository()
    {
        LoadFromJson();
    }

    public User CreateUser(string name, UserType type, string email, string passwordHash)
    {
        User newUser = new(GetNewId(), name, type, email, passwordHash);
        _data.Add(newUser);
        SaveToJson();

        return newUser;
    }

    public List<User> GetAllUsers()
    {
        return _data;
    }

    public User? GetUserById(int id)
    {
        return _data.Find(x => x.Id == id);
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
        _data.Remove(toDelete);

        SaveToJson();
    }

    private int GetNewId()
    {
        if (_data.Any())
            return _data.Max(x => x.Id) + 1;

        return 1;
    }
}
