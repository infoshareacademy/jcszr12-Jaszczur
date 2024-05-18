using AutoFixture;
using Moq;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.Web.Controllers;

namespace TutorLizard.Web.Tests.Controllers;
public class BrowseControllerTests
{
    private readonly BrowseController _browseController;
    private readonly Mock<IBrowseService> _mockBrowseService = new();
    private readonly Mock<IUserAuthenticationService> _mockUserAuthenticationService = new();
    private readonly Fixture _fixture = new();
    public BrowseControllerTests()
    {
        _browseController = new(_mockBrowseService.Object, _mockUserAuthenticationService.Object);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(int.MinValue, 1)]
    public async Task Ads_WhenProvidedInvalidPageNumber_ShouldAskForPageOne(int pageNumber, int expectedUsedPageNumber)
    {
        // Arrange
        List<GetBrowseAdsPageRequest> requests = [];
        GetBrowseAdsPageResponse response = _fixture
            .Build<GetBrowseAdsPageResponse>()
                .With(r => r.Success, true)
            .Create();

        _mockBrowseService
            .Setup(x => x.GetBrowseAdsPage(Capture.In(requests)))
            .Returns(Task.FromResult(response));

        // Act
        await _browseController.Ads(pageNumber);

        // Assert
        Assert.Equal(expectedUsedPageNumber, requests.Single().PageNumber);
    }
}
