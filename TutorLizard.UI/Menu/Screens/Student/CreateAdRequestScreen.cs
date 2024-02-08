using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Student;
public class CreateAdRequestScreen : StudentMenuScreenBase
{
    public CreateAdRequestScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
