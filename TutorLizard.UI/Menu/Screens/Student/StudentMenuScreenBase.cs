using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Student;
public abstract class StudentMenuScreenBase : IMenuScreen
{
    protected readonly IMenuService _menuService;
    protected readonly IStudentService _studentService;

    public abstract MenuNavigation Display();
    public StudentMenuScreenBase(IMenuService menuService, IStudentService studentService)
    {
        _menuService = menuService;
        _studentService = studentService;
    }
}
