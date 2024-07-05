
namespace TutorLizard.Shared.Models.DTOs.Requests;
public class RegisterUserWithGoogleRequest
{
    public RegisterUserWithGoogleRequest(string username, string email, string googleId)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        GoogleId = googleId ?? throw new ArgumentNullException(nameof(googleId));
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string GoogleId { get; set; }
}
