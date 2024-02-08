using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Student;
public class BrowseAdsScreen : StudentMenuScreenBase
{
    public BrowseAdsScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
