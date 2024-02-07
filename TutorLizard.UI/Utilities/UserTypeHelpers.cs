using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.UI.Utilities;
public static class UserTypeHelpers
{
    public static string UiName(this UserType? userType)
    {
        switch (userType)
        {
            case UserType.Tutor:
                return "Nauczyciel";
            case UserType.Student:
                return "Uczeń";
            case null:
            default:
                return "";
        }
    }
}
