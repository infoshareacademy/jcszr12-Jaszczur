using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Tests.Controllers.Browse;
public class BrowseControllerAdDetailsTests : BrowseControllerTestsBase
{
    [Fact]
    public async Task AdDetails_WhenUserIdIsNull_ShouldReturnRedirectToAction()
    {
        // Arrage
        int id = 1;
        SetupMockGetLoggedInUserId(null);

        // Act
        var result = await BrowseController.AdDetails(id);

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

        SetupMockGetLoggedInUserId(userId);

        List<GetAdDetailsRequest> requests = [];
        GetAdDetailsResponse response = Fixture.Create<GetAdDetailsResponse>();
        MockBrowseService
            .Setup(x => x.GetAdDetails(Capture.In(requests)))
            .Returns(Task.FromResult<GetAdDetailsResponse?>(response));

        // Act
        await BrowseController.AdDetails(id);

        // Assert
        Assert.Equivalent(expected, requests.Single());
    }

    [Fact]
    public async Task AdDetails_WhenResponseIsNull_ShouldReturnRedirectToAction()
    {
        // Arrange
        int id = 1;
        int userId = 19;

        SetupMockGetLoggedInUserId(userId);

        MockBrowseService
            .Setup(x => x.GetAdDetails(It.IsAny<GetAdDetailsRequest>()))
            .Returns(Task.FromResult<GetAdDetailsResponse?>(null));

        // Act
        var result = await BrowseController.AdDetails(id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task AdDetails_WhenResponseIsNull_ShouldReturnView()
    {
        // Arrange
        int id = 1;
        int userId = 19;

        SetupMockGetLoggedInUserId(userId);

        GetAdDetailsResponse response = Fixture.Create<GetAdDetailsResponse>();
        MockBrowseService
            .Setup(x => x.GetAdDetails(It.IsAny<GetAdDetailsRequest>()))
            .Returns(Task.FromResult<GetAdDetailsResponse?>(response));

        // Act
        var result = await BrowseController.AdDetails(id);

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}
