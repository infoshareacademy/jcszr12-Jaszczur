using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TutorLizard.Web.Interfaces.Services;
using TutorLizard.Web.Strings;

namespace TutorLizard.Web.Services;

public class UiMessagesService : IUiMessagesService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataFactory;
    public UiMessagesService(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataFactory = tempDataFactory;
    }

    public void ShowSuccessMessage(string message)
    {
        ShowMessage(message, MessageType.Success);
    }

    public void ShowFailureMessage(string message)
    {
        ShowMessage(message, MessageType.Failure);
    }

    public void ShowMessage(string message, string notificationType)
    {
        if (_httpContextAccessor.HttpContext is null)
            return;

        _tempDataFactory.GetTempData(_httpContextAccessor.HttpContext)[notificationType] = message;
    }
}
