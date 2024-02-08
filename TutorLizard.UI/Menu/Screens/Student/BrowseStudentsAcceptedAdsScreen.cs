using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Student;
public class BrowseStudentsAcceptedAdsScreen : StudentMenuScreenBase
{
    public BrowseStudentsAcceptedAdsScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
