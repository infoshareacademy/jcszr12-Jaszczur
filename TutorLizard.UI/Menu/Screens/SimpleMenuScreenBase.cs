namespace TutorLizard.UI.Menu.Screens;
public abstract class SimpleMenuScreenBase : IMenuScreen
{
    protected readonly IMenuService _menuService;

    public abstract MenuNavigation Display();
    public SimpleMenuScreenBase(IMenuService menuService)
    {
        _menuService = menuService;
    }
}
