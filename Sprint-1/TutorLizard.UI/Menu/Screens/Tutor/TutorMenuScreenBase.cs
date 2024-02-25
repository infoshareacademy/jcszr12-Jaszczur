using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public abstract class TutorMenuScreenBase : IMenuScreen
{
    protected readonly IMenuService _menuService;
    protected readonly ITutorService _tutorService;

    public abstract MenuNavigation Display();
    public TutorMenuScreenBase(IMenuService menuService, ITutorService tutorService)
    {
        _menuService = menuService;
        _tutorService = tutorService;
    }
}
