using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu;
public class MenuService : IMenuService
{
    private readonly IUserIdentityService _userIdentityService;
    private readonly ITutorService _tutorService;
    private readonly IStudentService _studentService;

    public MenuService(IUserIdentityService userIdentityService, ITutorService tutorService, IStudentService studentService)
    {
        _userIdentityService = userIdentityService;
        _tutorService = tutorService;
        _studentService = studentService;
    }

    public void Start()
    {
        Console.WriteLine("Tutor Lizard - Korepetycje z Jaszczurem");
        Console.ReadLine();
    }
}
