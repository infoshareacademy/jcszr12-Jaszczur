namespace TutorLizard.Web.Interfaces.Services;

public interface IUiMessagesService
{
    void ShowFailureMessage(string message);
    void ShowMessage(string message, string messageType);
    void ShowSuccessMessage(string message);
}