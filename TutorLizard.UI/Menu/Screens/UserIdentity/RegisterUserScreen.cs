using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.UserIdentity;
public class RegisterUserScreen : UserIdentityMenuScreenBase
{
    public RegisterUserScreen(IMenuService menuService, IUserIdentityService userIdentityService) : base(menuService, userIdentityService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
