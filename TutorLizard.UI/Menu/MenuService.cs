using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu;
public class MenuService
{
    private readonly UserIdentityService _userIdentityService;
    private readonly TutorService _tutorService;
    private readonly StudentService _studentService;

    public MenuService(UserIdentityService userIdentityService, TutorService tutorService, StudentService studentService)
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
