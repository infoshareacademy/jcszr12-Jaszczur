namespace TutorLizard.Web.Interfaces.Services;

public interface INotificationService
{
    void ShowFailureNotification(string message);
    void ShowNotification(string message, string notificationType);
    void ShowSuccessNotification(string message);
}