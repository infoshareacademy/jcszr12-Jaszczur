using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Menu;

namespace TutorLizard.UI;
internal class Program
{
    private static void Main(string[] args)
    {
        DataAccess dataAccess = new();
        UserIdentityService userIdentityService = new(dataAccess);
        TutorService tutorService = new(dataAccess, userIdentityService);
        StudentService studentService = new(dataAccess, userIdentityService);

        MenuService menuService = new(userIdentityService, tutorService, studentService);
        menuService.Start();
    }
}