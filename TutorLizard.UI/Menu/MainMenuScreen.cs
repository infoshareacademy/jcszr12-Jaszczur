using TutorLizard.UI.Menu.Screens.Simple;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu;
public class MainMenuScreen : SimpleMenuScreenBase
{
    public MainMenuScreen(IMenuService menuService) : base(menuService)
    {
    }

    public override MenuNavigation Display()
    {
        Console.WriteLine("Tutor Lizard - Korepetycje z Jaszczurem");
        Console.ReadLine();

        return MenuNavigation.QuitProgram;
    }
}
