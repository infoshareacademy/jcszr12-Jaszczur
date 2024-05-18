using AutoFixture;
using Microsoft.AspNetCore.Mvc;
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

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(100, 100)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public async Task Ads_WhenProvidedValidPageNumber_ShouldAskForCorrectPage(int pageNumber, int expectedUsedPageNumber)
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

    [Fact]
    public async Task Ads_WhenResponseIsUnsuccessful_ShouldReturnRedirectToAction()
    {
        // Arrange
        int pageNumber = 1;
        GetBrowseAdsPageResponse response = _fixture
            .Build<GetBrowseAdsPageResponse>()
                .With(r => r.Success, false)
            .Create();
        _mockBrowseService
           .Setup(x => x.GetBrowseAdsPage(It.IsAny<GetBrowseAdsPageRequest>()))
           .Returns(Task.FromResult(response));

        // Act
        var result = await _browseController.Ads(pageNumber);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Ads_WhenResponseIsSuccessful_ShouldReturnView()
    {
        // Arrange
        int pageNumber = 1;
        GetBrowseAdsPageResponse response = _fixture
            .Build<GetBrowseAdsPageResponse>()
                .With(r => r.Success, true)
            .Create();
        _mockBrowseService
           .Setup(x => x.GetBrowseAdsPage(It.IsAny<GetBrowseAdsPageRequest>()))
           .Returns(Task.FromResult(response));

        // Act
        var result = await _browseController.Ads(pageNumber);

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task AdDetails_WhenUserIdIsNull_ShouldReturnRedirectToAction()
    {
        // Arrage
        int id = 1;
        _mockUserAuthenticationService
            .Setup(x => x.GetLoggedInUserId())
            .Returns((int?)null);

        // Act
        var result = await _browseController.AdDetails(id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task AdDetails_WhenUserIdIsNotNull_ShouldSendCorrectRequest()
    {
        // Arrange
        int id = 1;
        int userId = 19;

        GetAdDetailsRequest expected = new()
        {
            AdId = id,
            UserId = userId
        };

        _mockUserAuthenticationService
            .Setup(x => x.GetLoggedInUserId())
            .Returns(userId);
        
        List<GetAdDetailsRequest> requests = [];
        GetAdDetailsResponse response = _fixture.Create<GetAdDetailsResponse>();
        _mockBrowseService
            .Setup(x => x.GetAdDetails(Capture.In(requests)))
            .Returns(Task.FromResult<GetAdDetailsResponse?>(response));

        // Act
        await _browseController.AdDetails(id);

        // Assert
        Assert.Equivalent(expected, requests.Single());
    }

    [Fact]
    public async Task AdDetails_WhenResponseIsNull_ShouldReturnRedirectToAction()
    {
        // Arrange
        int id = 1;
        int userId = 19;

        _mockUserAuthenticationService
            .Setup(x => x.GetLoggedInUserId())
            .Returns(userId);

        _mockBrowseService
            .Setup(x => x.GetAdDetails(It.IsAny<GetAdDetailsRequest>()))
            .Returns(Task.FromResult<GetAdDetailsResponse?>(null));

        // Act
        var result = await _browseController.AdDetails(id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task AdDetails_WhenResponseIsNull_ShouldReturnView()
    {
        // Arrange
        int id = 1;
        int userId = 19;

        _mockUserAuthenticationService
            .Setup(x => x.GetLoggedInUserId())
            .Returns(userId);

        GetAdDetailsResponse response = _fixture.Create<GetAdDetailsResponse>();
        _mockBrowseService
            .Setup(x => x.GetAdDetails(It.IsAny<GetAdDetailsRequest>()))
            .Returns(Task.FromResult<GetAdDetailsResponse?>(response));

        // Act
        var result = await _browseController.AdDetails(id);

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Schedule_WhenUserIdIsNull_ShouldReturnRedirectToAction()
    {
        // Arrange
        _mockUserAuthenticationService
            .Setup(x => x.GetLoggedInUserId())
            .Returns((int?)null);

        // Act
        var result = await _browseController.Schedule();

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Schedule_WhenUserIdIsNotNull_ShouldReturnRedirectView()
    {
        // Arrange
        int userId = 19;
        _mockUserAuthenticationService
            .Setup(x => x.GetLoggedInUserId())
            .Returns(userId);

        GetUsersScheduleResponse response = _fixture.Create<GetUsersScheduleResponse>();
        _mockBrowseService
            .Setup(x => x.GetUsersSchedule(It.IsAny<GetUsersScheduleRequest>()))
            .Returns(Task.FromResult(response));

        // Act
        var result = await _browseController.Schedule();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(19)]
    [InlineData(1000)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public async Task Schedule_WhenUserIdIsNotNull_ShouldSendCorrectRequest(int userId)
    {
        // Arrange
        _mockUserAuthenticationService
            .Setup(x => x.GetLoggedInUserId())
            .Returns(userId);

        List<GetUsersScheduleRequest> requests = [];
        GetUsersScheduleResponse response = _fixture.Create<GetUsersScheduleResponse>();
        _mockBrowseService
            .Setup(x => x.GetUsersSchedule(Capture.In(requests)))
            .Returns(Task.FromResult(response));

        GetUsersScheduleRequest expected = new()
        {
            UserId = userId
        };

        // Act
        await _browseController.Schedule();

        // Assert
        Assert.Equivalent(expected, requests.Single());
    }
}
