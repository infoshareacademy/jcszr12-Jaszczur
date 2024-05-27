using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Browse;
public class BrowseServiceTests : BrowseServiceTestsBase, IDisposable
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, -1)]
    public async Task GetBrowseAdsPage_WhenRequestIsInvalid_ShouldReturnUnsuccessfulResponse(int pageSize, int pageNumber)
    {
        // Arrange
        GetBrowseAdsPageRequest request = new(pageNumber, pageSize);
        GetBrowseAdsPageResponse expectedResponse = new()
        {
            Success = false,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = 0
        };

        // Act
        var actualResponse = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Equivalent(expectedResponse, actualResponse);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenPageNumberIsLargerThanAvailable_ShouldReturnLastPage()
    {
        // Arrange
        int adCount = 15;
        int pageNumber = 4;
        int pageSize = 5;

        var ads = CreateTestAds(adCount);
        SetupMockGetAllAds(ads);

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.True(response.Success);
        Assert.Equal(response.TotalPages, response.PageNumber);
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(1, 1, 0, 0)]
    [InlineData(1, 10, 101, 10)]
    [InlineData(10, 10, 101, 10)]
    [InlineData(11, 10, 101, 1)]
    [InlineData(12, 10, 101, 1)]
    public async Task GetBrowseAdsPage_WhenRequestIsValid_ShouldReturnCorrectNumberOfAds(int pageNumber, int pageSize, int adCount, int expectedResponseAdCount)
    {
        // Arrange
        var ads = CreateTestAds(adCount);
        SetupMockGetAllAds(ads);

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);
        int actualResponseAdCount = response.Ads.Count;

        // Assert
        Assert.True(response.Success);
        Assert.Equal(expectedResponseAdCount, actualResponseAdCount);
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(1, 1, 0, 1)]
    [InlineData(1, 10, 101, 11)]
    [InlineData(12, 10, 101, 11)]
    public async Task GetBrowseAdsPage_WhenRequestIsValid_ShouldReturnCorrectTotalPages(int pageNumber, int pageSize, int adCount, int expectedTotalPages)
    {
        // Arrange
        var ads = CreateTestAds(adCount);
        SetupMockGetAllAds(ads);

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);
        int actualTotalPages = response.TotalPages;

        // Assert
        Assert.True(response.Success);
        Assert.Equal(expectedTotalPages, actualTotalPages);
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(1, 1, 0)]
    [InlineData(1, 10, 101)]
    [InlineData(10, 10, 101)]
    [InlineData(11, 10, 101)]
    public async Task GetBrowseAdsPage_WhenRequestIsValid_ShouldReturnCorrectAds(int pageNumber, int pageSize, int adCount)
    {
        // Arrange
        var ads = CreateTestAds(adCount);
        SetupMockGetAllAds(ads);

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize);

        int expectedAdsSkipped = (pageNumber - 1) * pageSize;
        List<Ad> expectedAds = ads
            .Skip(expectedAdsSkipped)
            .Take(pageSize)
            .ToList();

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.True(response.Success);
        Assert.Equal(expectedAds.Count, response.Ads.Count);
        for (int i = 0; i < response.Ads.Count; i++)
        {
            Ad expected = expectedAds[i];
            AdListItemDto actual = response.Ads[i];

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.TutorId, actual.TutorId);
            Assert.Equal(expected.User.Name, actual.TutorName);
            Assert.Equal(expected.Subject, actual.Subject);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.CategoryId, actual.CategoryId);
            Assert.Equal(expected.Category.Name, actual.CategoryName);
            Assert.Equal(expected.Price, actual.Price);
            Assert.Equal(expected.Location, actual.Location);
            Assert.Equal(expected.IsRemote, actual.IsRemote);
        }

    }

    [Fact]
    public async Task GetAdDetails_WhenAdDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int adId = 1;
        int userId = 19;
        AdDetailsRequest request = new()
        {
            AdId = adId,
            UserId = userId
        };

        var ads = CreateTestAds(0);
        SetupMockGetAllAds(ads);
        SetupMockGetAdById(null);

        // Act
        var response = await BrowseService.GetAdDetails(request);

        // Assert
        Assert.Null(response);
    }

    [Fact]
    public async Task GetAdDetails_WhenUserIsOwner_ShouldReturnCorrectUserRelationship()
    {
        // Arrange
        var ads = CreateTestAds(1);
        var ad = ads.Single();
        SetupMockGetAllAds(ads);
        SetupMockGetAdById(ad);

        int adId = ad.Id;
        int userId = ad.TutorId;
        AdDetailsRequest request = new()
        {
            AdId = adId,
            UserId = userId
        };

        var expectedRelationship = AdToUserRelationship.Owner;

        // Act
        var response = await BrowseService.GetAdDetails(request);
        var actualRelationship = response!.UserRelationship;

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedRelationship, actualRelationship);
    }

    [Fact]
    public async Task GetAdDetails_WhenUserIsAcceptedStudent_ShouldReturnCorrectUserRelationship()
    {
        // Arrange
        var ads = CreateTestAds(1);
        var ad = ads.Single();

        int userId = ad.TutorId + 1;
        AdRequest acceptedAdRequest = new()
        {
            StudentId = userId,
            IsAccepted = true,
            Message = "",
            ReplyMessage = "",
            ReviewDate = DateTime.Now,
        };
        ad.AdRequests.Add(acceptedAdRequest);

        SetupMockGetAllAds(ads);
        SetupMockGetAdById(ad);

        int adId = ad.Id;

        AdDetailsRequest request = new()
        {
            AdId = adId,
            UserId = userId
        };

        var expectedRelationship = AdToUserRelationship.AcceptedStudent;

        // Act
        var response = await BrowseService.GetAdDetails(request);
        var actualRelationship = response!.UserRelationship;

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedRelationship, actualRelationship);
    }

    [Fact]
    public async Task GetAdDetails_WhenUserIsPendingStudent_ShouldReturnCorrectUserRelationship()
    {
        // Arrange
        var ads = CreateTestAds(1);
        var ad = ads.Single();

        int userId = ad.TutorId + 1;
        AdRequest acceptedAdRequest = new()
        {
            StudentId = userId,
            IsAccepted = false,
            Message = "",
            ReplyMessage = "",
            ReviewDate = null,
        };
        ad.AdRequests.Add(acceptedAdRequest);

        SetupMockGetAllAds(ads);
        SetupMockGetAdById(ad);

        int adId = ad.Id;

        AdDetailsRequest request = new()
        {
            AdId = adId,
            UserId = userId
        };

        var expectedRelationship = AdToUserRelationship.PendingStudent;

        // Act
        var response = await BrowseService.GetAdDetails(request);
        var actualRelationship = response!.UserRelationship;

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedRelationship, actualRelationship);
    }

    [Fact]
    public async Task GetAdDetails_WhenUserHasNoRelationshipToAd_ShouldReturnCorrectUserRelationship()
    {
        // Arrange
        var ads = CreateTestAds(1);
        var ad = ads.Single();

        int userId = ad.TutorId + 1;

        SetupMockGetAllAds(ads);
        SetupMockGetAdById(ad);

        int adId = ad.Id;

        AdDetailsRequest request = new()
        {
            AdId = adId,
            UserId = userId
        };

        var expectedRelationship = AdToUserRelationship.None;

        // Act
        var response = await BrowseService.GetAdDetails(request);
        var actualRelationship = response!.UserRelationship;

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedRelationship, actualRelationship);
    }

    [Fact]
    public async Task GetAdDetails_WhenAdExists_ShouldReturnCorrectAdDetails()
    {
        // Arrange
        var ads = CreateTestAds(1);
        var ad = ads.Single();

        SetupMockGetAllAds(ads);
        SetupMockGetAdById(ad);

        int adId = ad.Id;
        int userId = 19;
        AdDetailsRequest request = new()
        {
            AdId = adId,
            UserId = userId
        };

        // Act
        var response = await BrowseService.GetAdDetails(request);

        // Assert
        Assert.NotNull(response);

        Assert.Equal(ad.Id, response.AdId);
        Assert.Equal(ad.TutorId, response.TutorId);
        Assert.Equal(ad.User.Id, response.TutorId);
        Assert.Equal(ad.User.Name, response.TutorName);
        Assert.Equal(ad.Title, response.Title);
        Assert.Equal(ad.CategoryId, response.CategoryId);
        Assert.Equal(ad.Category.Id, response.CategoryId);
        Assert.Equal(ad.Category.Name, response.CategoryName);
        Assert.Equal(ad.Subject, response.Subject);
        Assert.Equal(ad.Location, response.Location);
        Assert.Equal(ad.Price, response.Price);
        Assert.Equal(ad.IsRemote, response.IsRemote);
        Assert.Equal(ad.Description, response.Description);

        Assert.True(Enum.IsDefined(response.UserRelationship));
    }

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
