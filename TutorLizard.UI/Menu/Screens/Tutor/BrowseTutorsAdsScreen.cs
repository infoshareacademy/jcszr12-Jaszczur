using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsAdsScreen : TutorMenuScreenBase
{
    public BrowseTutorsAdsScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
