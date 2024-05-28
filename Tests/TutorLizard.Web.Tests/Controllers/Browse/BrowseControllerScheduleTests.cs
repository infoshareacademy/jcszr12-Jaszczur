using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Tests.Controllers.Browse;
public class BrowseControllerScheduleTests : BrowseControllerTestsBase
{
    [Fact]
    public async Task Schedule_WhenUserIdIsNull_ShouldReturnRedirectToAction()
    {
        // Arrange
        SetupMockGetLoggedInUserId(null);

        // Act
        var result = await BrowseController.Schedule();

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Schedule_WhenUserIdIsNotNull_ShouldReturnRedirectView()
    {
        // Arrange
        int userId = 19;
        SetupMockGetLoggedInUserId(userId);

        GetUsersScheduleResponse response = Fixture.Create<GetUsersScheduleResponse>();
        MockBrowseService
            .Setup(x => x.GetUsersSchedule(It.IsAny<GetUsersScheduleRequest>()))
            .Returns(Task.FromResult(response));

        // Act
        var result = await BrowseController.Schedule();

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
        SetupMockGetLoggedInUserId(userId);

        List<GetUsersScheduleRequest> requests = [];
        GetUsersScheduleResponse response = Fixture.Create<GetUsersScheduleResponse>();
        MockBrowseService
            .Setup(x => x.GetUsersSchedule(Capture.In(requests)))
            .Returns(Task.FromResult(response));

        GetUsersScheduleRequest expected = new()
        {
            UserId = userId
        };

        // Act
        await BrowseController.Schedule();

        // Assert
        Assert.Equivalent(expected, requests.Single());
    }
}
