using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityDataAccess _dataAccess;

    private User? _activeUser;
    
    private readonly IUserRepository _userRepository;

    public UserIdentityService(IUserIdentityDataAccess dataAccess, IUserRepository userRepository)
    {
        _dataAccess = dataAccess;
        _userRepository = userRepository;
    }

    public UserType? GetUserType()
    {
        return _activeUser?.UserType;
    }

    public string? GetUserName()
    {
        return _activeUser?.Name;
    }

    public int? GetUserId()
    {
        return _activeUser?.Id;
    }

    public bool IsUserNameTaken(string userName)
    {
        return _dataAccess.DoesUserWithThisNameExist(userName);
    }

    public string GetUserNameById(int userId)
    {
        return _dataAccess.GetUserNameById(userId);
    }
}
