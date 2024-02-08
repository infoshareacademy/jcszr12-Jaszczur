using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Student;
public class BrowseStudentsAvailableScheduleScreen : StudentMenuScreenBase
{
    public BrowseStudentsAvailableScheduleScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
