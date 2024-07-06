
namespace TutorLizard.Shared.Models.DTOs.Requests;
public class RegisterUserWithGoogleRequest
{
    public RegisterUserWithGoogleRequest(string username, string email, string googleId)
    {
        Username = username ?? "";
        Email = email ?? "";
        GoogleId = googleId ?? "";
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string GoogleId { get; set; }
}
