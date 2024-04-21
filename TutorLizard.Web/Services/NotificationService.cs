using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TutorLizard.Web.Interfaces.Services;
using TutorLizard.Web.Strings;

namespace TutorLizard.Web.Services;

public class NotificationService : INotificationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataFactory;
    public NotificationService(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataFactory = tempDataFactory;

    }

    public void ShowSuccessNotification(string message)
    {
        ShowNotification(message, NotificationType.Success);
    }

    public void ShowFailureNotification(string message)
    {
        ShowNotification(message, NotificationType.Failure);
    }

    public void ShowNotification(string message, string notificationType)
    {
        if (_httpContextAccessor.HttpContext is null)
            return;

        _tempDataFactory.GetTempData(_httpContextAccessor.HttpContext)[notificationType] = message;
    }
}
