using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Simple;
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
