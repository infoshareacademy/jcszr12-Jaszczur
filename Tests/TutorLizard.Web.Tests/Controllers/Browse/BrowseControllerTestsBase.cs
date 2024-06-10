using AutoFixture;
using Moq;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Web.Controllers;
using TutorLizard.Web.Interfaces.Services;

namespace TutorLizard.Web.Tests.Controllers.Browse;

public abstract class BrowseControllerTestsBase
{
    protected BrowseController BrowseController;
    protected Mock<IBrowseService> MockBrowseService = new();
    protected Mock<IUserAuthenticationService> MockUserAuthenticationService = new();
    protected Mock<IUiMessagesService> MockUiMessagesService = new();
    protected Fixture Fixture = new();

    public BrowseControllerTestsBase()
    {
        BrowseController = new(MockBrowseService.Object,
                               MockUserAuthenticationService.Object,
                               MockUiMessagesService.Object);
    }

    protected void SetupMockGetLoggedInUserId(int? userId)
    {
        MockUserAuthenticationService
           .Setup(x => x.GetLoggedInUserId())
           .Returns(userId);
    }
}