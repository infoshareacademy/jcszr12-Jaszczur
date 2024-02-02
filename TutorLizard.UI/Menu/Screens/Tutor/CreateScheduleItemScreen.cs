using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class CreateScheduleItemScreen : TutorMenuScreenBase
{
    public CreateScheduleItemScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
