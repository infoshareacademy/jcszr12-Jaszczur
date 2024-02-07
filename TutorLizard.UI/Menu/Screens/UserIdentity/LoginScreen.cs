using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.UserIdentity;
public class LoginScreen : UserIdentityMenuScreenBase
{
    public LoginScreen(IMenuService menuService, IUserIdentityService userIdentityService) : base(menuService, userIdentityService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
