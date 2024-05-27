using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Browse;
public class BrowseServiceGetUsersScheduleTests : BrowseServiceTestsBase
{
    [Fact]
    public async Task GetUsersSchedule_WhenUserHasNoAds_ShouldReturnEmptyTutorsSchedule()
    {
        // Arrange
        int scheduleItemCount = 100;
        SetupMockGetAllScheduleItems(scheduleItemCount);

        int usersAdCount = 0;
        int usersScheduleItemRequestCount = 10;
        User userWithoutAds = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);

        int userId = userWithoutAds.Id;
        UsersScheduleRequest request = new()
        {
            UserId = userId
        };

        // Act
        var response = await BrowseService.GetUsersSchedule(request);

        // Assert
        Assert.Empty(response.TutorsSchedule);
    }

    [Fact]
    public async Task GetUsersSchedule_WhenUserHasNoScheduleItemRequests_ShouldReturnEmptyStudentsSchedule()
    {
        // Arrange
        int scheduleItemCount = 100;
        SetupMockGetAllScheduleItems(scheduleItemCount);

        int usersAdCount = 10;
        int usersScheduleItemRequestCount = 0;
        User userWithoutAdRequests = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);

        int userId = userWithoutAdRequests.Id;
        UsersScheduleRequest request = new()
        {
            UserId = userId,
        };

        // Act
        var response = await BrowseService.GetUsersSchedule(request);

        // Assert
        Assert.Empty(response.StudentsSchedule);
    }

    [Fact]
    public async Task GetUsersSchedule_WhenUserHasAdsWithScheduleItems_ShouldReturnCorrectTutorsSchedule()
    {
        // Arrange
        int scheduleItemCount = 50;
        int adCount = 5;
        int scheduleItemRequestCount = 100;
        SetupMockScheduleData(scheduleItemCount, adCount, scheduleItemRequestCount);

        int usersFinalAdCount = 2;
        User user = CreateUserAndGiveHimExistingAds(usersFinalAdCount);

        UsersScheduleRequest request = new()
        {
            UserId = user.Id
        };

        int expectedAdCount = user.Ads.Sum(ad => ad.ScheduleItems.Count);

        // Act
        var response = await BrowseService.GetUsersSchedule(request);
        int actualAdCount = response.TutorsSchedule.Count;

        // Assert
        Assert.Equal(expectedAdCount, actualAdCount);

        foreach (var actual in response.TutorsSchedule)
        {
            var expected = user.Ads
                .SelectMany(ad => ad.ScheduleItems)
                .FirstOrDefault(item => item.Id == actual.Id);

            Assert.NotNull(expected);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.AdId, actual.AdId);
            Assert.Equal(expected.Ad.Title, actual.AdTitle);
            Assert.Equal(expected.DateTime, actual.DateTime);
            Assert.Equal(expected.ScheduleItemRequests.Count, actual.RequestCount);
        }
    }



    [Fact]
    public async Task GetUsersSchedule_WhenUserHasScheduleItemRequests_ShouldReturnCorrectStudentsSchedule()
    {
        // Arrange
        int scheduleItemCount = 50;
        int adCount = 5;
        int scheduleItemRequestCount = 100;
        SetupMockScheduleData(scheduleItemCount, adCount, scheduleItemRequestCount);

        int usersFinalScheduleItemRequestCount = 20;
        User user = CreateUserAndGiveHimExistingScheduleItemRequests(usersFinalScheduleItemRequestCount);

        UsersScheduleRequest request = new()
        {
            UserId = user.Id
        };

        // Act
        var response = await BrowseService.GetUsersSchedule(request);

        // Assert
        Assert.Equal(user.ScheduleItemRequests.Count, response.StudentsSchedule.Count);

        foreach (var actual in response.StudentsSchedule)
        {
            var expected = user.ScheduleItemRequests
                .Select(request => request.ScheduleItem)
                .FirstOrDefault(request => request.Id == actual.Id);

            Assert.NotNull(expected);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.AdId, actual.AdId);
            Assert.Equal(expected.Ad.Id, actual.AdId);
            Assert.Equal(expected.Ad.Title, actual.AdTitle);
            Assert.Equal(expected.Ad.User.Name, actual.TutorName);
            Assert.Equal(expected.DateTime, actual.DateTime);
        }
    }

    [Fact]
    public async Task GetUsersSchedule_WhenNoStudentIsAccepted_ShouldReturnNullAcceptedStudentsName()
    {
        // Arrange
        int scheduleItemCount = 1;
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData(scheduleItemCount, adCount, scheduleItemRequestCount);

        int usersFinalAdCount = 1;
        User user = CreateUserAndGiveHimExistingAds(usersFinalAdCount);

        UsersScheduleRequest request = new()
        {
            UserId = user.Id
        };

        var scheduleItemRequest = user
            .Ads.First()
            .ScheduleItems.First()
            .ScheduleItemRequests.First();
        scheduleItemRequest.IsAccepted = false;
        DbContext.SaveChanges();

        // Act
        var response = await BrowseService.GetUsersSchedule(request);
        var actualName = response.TutorsSchedule.First().AcceptedStudentsName;

        // Assert
        Assert.Null(actualName);
    }

    [Fact]
    public async Task GetUsersSchedule_WhenAStudentIsAccepted_ShouldReturnStudentsName()
    {
        // Arrange
        int scheduleItemCount = 1;
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData(scheduleItemCount, adCount, scheduleItemRequestCount);

        int usersFinalAdCount = 1;
        User user = CreateUserAndGiveHimExistingAds(usersFinalAdCount);

        UsersScheduleRequest request = new()
        {
            UserId = user.Id
        };

        var scheduleItemRequest = user
            .Ads.First()
            .ScheduleItems.First()
            .ScheduleItemRequests.First();
        scheduleItemRequest.IsAccepted = true;
        DbContext.SaveChanges();

        string expectedName = scheduleItemRequest.User.Name;

        // Act
        var response = await BrowseService.GetUsersSchedule(request);
        var actualName = response.TutorsSchedule.First().AcceptedStudentsName;

        // Assert
        Assert.Equal(expectedName, actualName);
    }

    [Fact]
    public async Task GetUsersSchedule_WhenRequestIsAccepted_ShouldReturnCorrectStatus()
    {
        // Arrange
        int scheduleItemCount = 1;
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData(scheduleItemCount, adCount, scheduleItemRequestCount);

        int usersFinalScheduleItemRequestCount = 1;
        User user = CreateUserAndGiveHimExistingScheduleItemRequests(usersFinalScheduleItemRequestCount);

        UsersScheduleRequest request = new()
        {
            UserId = user.Id
        };

        var scheduleItemRequest = user
            .ScheduleItemRequests.First();

        scheduleItemRequest.IsAccepted = true;
        DbContext.SaveChanges();

        var expectedStatus = StudentsScheduleItemSummaryDto.RequestStatus.Accepted;

        // Act
        var response = await BrowseService.GetUsersSchedule(request);
        var actualStatus = response.StudentsSchedule.First().Status;

        // Assert
        Assert.Equal(expectedStatus, actualStatus);
    }

    [Fact]
    public async Task GetUsersSchedule_WhenRequestIsPending_ShouldReturnCorrectStatus()
    {
        // Arrange
        int scheduleItemCount = 1;
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData(scheduleItemCount, adCount, scheduleItemRequestCount);

        int usersFinalScheduleItemRequestCount = 1;
        User user = CreateUserAndGiveHimExistingScheduleItemRequests(usersFinalScheduleItemRequestCount);

        UsersScheduleRequest request = new()
        {
            UserId = user.Id
        };

        var scheduleItemRequest = user
            .ScheduleItemRequests.First();

        scheduleItemRequest.IsAccepted = false;
        DbContext.SaveChanges();

        var expectedStatus = StudentsScheduleItemSummaryDto.RequestStatus.Pending;

        // Act
        var response = await BrowseService.GetUsersSchedule(request);
        var actualStatus = response.StudentsSchedule.First().Status;

        // Assert
        Assert.Equal(expectedStatus, actualStatus);
    }
}
