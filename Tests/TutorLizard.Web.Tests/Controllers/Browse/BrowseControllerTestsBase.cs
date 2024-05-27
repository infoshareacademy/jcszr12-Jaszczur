using AutoFixture;
using Moq;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.Web.Controllers;

namespace TutorLizard.Web.Tests.Controllers.Browse;

public abstract class BrowseControllerTestsBase
{
    protected BrowseController BrowseController;
    protected Mock<IBrowseService> MockBrowseService = new();
    protected Mock<IUserAuthenticationService> MockUserAuthenticationService = new();
    protected Fixture Fixture = new();

    public BrowseControllerTestsBase()
    {
        BrowseController = new(MockBrowseService.Object, MockUserAuthenticationService.Object);
    }
    protected GetBrowseAdsPageResponse CreateGetBrowseAdsPageResponse(bool success)
    {
        return Fixture
                    .Build<GetBrowseAdsPageResponse>()
                        .With(r => r.Success, success)
                    .Create();
    }

    protected void SetupMockGetLoggedInUserId(int? userId)
    {
        MockUserAuthenticationService
           .Setup(x => x.GetLoggedInUserId())
           .Returns(userId);
    }
}