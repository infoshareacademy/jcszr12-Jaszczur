using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.UserIdentity;
public abstract class UserIdentityMenuScreenBase : IMenuScreen
{
    protected readonly IMenuService _menuService;
    protected readonly IUserIdentityService _userIdentityService;

    public abstract MenuNavigation Display();
    public UserIdentityMenuScreenBase(IMenuService menuService, IUserIdentityService userIdentityService)
    {
        _menuService = menuService;
        _userIdentityService = userIdentityService;
    }
}
