using TutorLizard.Shared.Enums;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    public Task<LogInResult> LogIn(string username, string password);
    public Task<bool> RegisterUser(string userName, UserType type, string email, string password, string activationCode);
    Task<ActivationResultDto> ActivateUserAsync(string activationCode);
    public Task<RegisterUserWithGoogleResponse> RegisterUserWithGoogle(RegisterUserWithGoogleRequest request);
    public Task<bool> IsTheGoogleUserRegistered(string googleId);
    public Task<LogInResult> LogInWithGoogle(string email, string googleId);
}
