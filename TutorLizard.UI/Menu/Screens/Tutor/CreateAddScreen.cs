﻿using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class CreateAddScreen : TutorMenuScreenBase
{
    public CreateAddScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    public override MenuNavigation Display()
    {
        throw new NotImplementedException();
    }
}
