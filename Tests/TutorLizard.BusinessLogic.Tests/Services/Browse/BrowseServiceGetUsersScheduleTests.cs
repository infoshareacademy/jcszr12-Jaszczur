using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;

namespace TutorLizard.BusinessLogic.Tests.Services.Browse;
public class BrowseServiceGetUsersScheduleTests : BrowseServiceTestsBase
{
    [Fact]
    public async Task GetUsersSchedule_WhenUserHasNoAds_ShouldReturnEmptyTutorsSchedule()
    {
        // Arrange
        ScheduleItemParameterSet parameters =new(Month: DateTime.Now.Month,
                                                  Year: DateTime.Now.Year,
                                                  Count: 100);
        SetupMockGetAllScheduleItems([parameters]);

        int usersAdCount = 0;
        int usersScheduleItemRequestCount = 10;
        User userWithoutAds = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);

        int userId = userWithoutAds.Id;
        GetUsersScheduleRequest request = new()
        {
            UserId = userId,
            Month = parameters.Month,
            Year = parameters.Year,
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
        ScheduleItemParameterSet parameters = new(Month: DateTime.Now.Month,
                                                  Year: DateTime.Now.Year,
                                                  Count: 100);
        SetupMockGetAllScheduleItems([parameters]);

        int usersAdCount = 10;
        int usersScheduleItemRequestCount = 0;
        User userWithoutAdRequests = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);

        int userId = userWithoutAdRequests.Id;
        GetUsersScheduleRequest request = new()
        {
            UserId = userId,
            Month = parameters.Month,
            Year = parameters.Year,
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
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;
        List<ScheduleItemParameterSet> parameterSets =
        [
            new(Month: month,
                Year: year,
                Count: 25),
            new(Month: DateTime.Now.AddMonths(Random.Shared.Next(3)).Month,
                Year: DateTime.Now.AddYears(1).Year,
                Count: 25),
        ];
        int adCount = 5;
        int scheduleItemRequestCount = 100;
        SetupMockScheduleData(parameterSets, adCount, scheduleItemRequestCount);

        int usersFinalAdCount = 2;
        User user = CreateUserAndGiveHimExistingAds(usersFinalAdCount);

        GetUsersScheduleRequest request = new()
        {
            UserId = user.Id,
            Month = month,
            Year = year,
        };

        int expectedScheduleItemCount = user.Ads.Sum(ad =>
            ad.ScheduleItems
              .Where(item => (item.DateTime.Month, item.DateTime.Year) == (month, year))
              .Count());

        // Act
        var response = await BrowseService.GetUsersSchedule(request);
        int actualScheduleItemCount = response.TutorsSchedule.Count;

        // Assert
        Assert.Equal(expectedScheduleItemCount, actualScheduleItemCount);

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
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;
        List<ScheduleItemParameterSet> parameterSets =
        [
            new(Month: month,
                Year: year,
                Count: 25),
            new(Month: DateTime.Now.AddMonths(Random.Shared.Next(3)).Month,
                Year: DateTime.Now.AddYears(1).Year,
                Count: 25),
        ];
        int adCount = 5;
        int scheduleItemRequestCount = 100;
        SetupMockScheduleData(parameterSets, adCount, scheduleItemRequestCount);

        int usersFinalScheduleItemRequestCount = 20;
        User user = CreateUserAndGiveHimExistingScheduleItemRequests(usersFinalScheduleItemRequestCount);

        GetUsersScheduleRequest request = new()
        {
            UserId = user.Id,
            Month = month,
            Year = year,
        };

        int expectedScheduleItemCount = user.ScheduleItemRequests
            .Where(request => (request.ScheduleItem.DateTime.Month, request.ScheduleItem.DateTime.Year) == (month, year))
            .Count();

        // Act
        var response = await BrowseService.GetUsersSchedule(request);
        int actualScheduleItemCount = response.StudentsSchedule.Count;

        // Assert
        Assert.Equal(expectedScheduleItemCount, actualScheduleItemCount);

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
        ScheduleItemParameterSet parameters = new(Month: DateTime.Now.Month,
                                                  Year: DateTime.Now.Year,
                                                  Count: 1);
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData([parameters], adCount, scheduleItemRequestCount);

        int usersFinalAdCount = 1;
        User user = CreateUserAndGiveHimExistingAds(usersFinalAdCount);

        GetUsersScheduleRequest request = new()
        {
            UserId = user.Id,
            Month = parameters.Month,
            Year = parameters.Year,
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
        ScheduleItemParameterSet parameters = new(Month: DateTime.Now.Month,
                                                  Year: DateTime.Now.Year,
                                                  Count: 1);
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData([parameters], adCount, scheduleItemRequestCount);

        int usersFinalAdCount = 1;
        User user = CreateUserAndGiveHimExistingAds(usersFinalAdCount);

        GetUsersScheduleRequest request = new()
        {
            UserId = user.Id,
            Month = parameters.Month,
            Year = parameters.Year,
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
        ScheduleItemParameterSet parameters = new(Month: DateTime.Now.Month,
                                                  Year: DateTime.Now.Year,
                                                  Count: 1);
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData([parameters], adCount, scheduleItemRequestCount);

        int usersFinalScheduleItemRequestCount = 1;
        User user = CreateUserAndGiveHimExistingScheduleItemRequests(usersFinalScheduleItemRequestCount);

        GetUsersScheduleRequest request = new()
        {
            UserId = user.Id,
            Month = parameters.Month,
            Year = parameters.Year,
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
        ScheduleItemParameterSet parameters = new(Month: DateTime.Now.Month,
                                                  Year: DateTime.Now.Year,
                                                  Count: 1);
        int adCount = 1;
        int scheduleItemRequestCount = 1;
        SetupMockScheduleData([parameters], adCount, scheduleItemRequestCount);

        int usersFinalScheduleItemRequestCount = 1;
        User user = CreateUserAndGiveHimExistingScheduleItemRequests(usersFinalScheduleItemRequestCount);

        GetUsersScheduleRequest request = new()
        {
            UserId = user.Id,
            Month = parameters.Month,
            Year = parameters.Year,
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

    private void SetupMockScheduleData(List<ScheduleItemParameterSet> scheduleItemParameterSets, int adCount, int scheduleItemRequestCount)
    {
        SetupMockGetAllScheduleItems(scheduleItemParameterSets);

        CreateAdsForScheduleItems(adCount);

        CreateScheduleItemRequestsForScheduleItems(scheduleItemRequestCount);
    }

    private User CreateUserAndGiveHimExistingAds(int usersFinalAdCount)
    {
        User user = CreateTestUserAndAddToDb();

        while (user.Ads.Count < usersFinalAdCount)
        {
            ChangeUserInRandomAdInDb(user);
        }

        return user;
    }

    private User CreateUserAndGiveHimExistingScheduleItemRequests(int usersFinalScheduleItemRequestCount)
    {
        User user = CreateTestUserAndAddToDb();

        while (user.ScheduleItemRequests.Count < usersFinalScheduleItemRequestCount)
        {
            ChangeUserInRandomScheduleItemRequestInDb(user);
        }

        return user;
    }

    private void ChangeUserInRandomAdInDb(User user)
    {
        List<Ad> ads = DbContext.Ads.ToList();

        Ad ad = ads[Random.Shared.Next(ads.Count)];

        ad.User = user;
        DbContext.SaveChanges();
    }

    private void ChangeUserInRandomScheduleItemRequestInDb(User user)
    {
        List<ScheduleItem> scheduleItems = DbContext.ScheduleItems.ToList();

        ScheduleItem scheduleItem = scheduleItems[Random.Shared.Next(scheduleItems.Count)];

        ScheduleItemRequest? request = scheduleItem.ScheduleItemRequests.FirstOrDefault();

        if (request is not null)
        {
            request.User = user;
        }

        DbContext.SaveChanges();
    }

    private void CreateAdsForScheduleItems(int adCount)
    {
        var ads = CreateTestAds(adCount);
        AddEntitiesToInMemoryDb(ads);

        ChangeAdToRandomInAllScheduleItems();

        DbContext.SaveChanges();
    }

    private void CreateScheduleItemRequestsForScheduleItems(int scheduleItemRequestCount)
    {
        var scheduleItemRequests = CreateTestScheduleItemRequests(scheduleItemRequestCount);
        AddEntitiesToInMemoryDb(scheduleItemRequests);

        ChangeScheduleItemToRandomInAllScheduleItemRequests();

        DbContext.SaveChanges();
    }

    private void ChangeAdToRandomInAllScheduleItems()
    {
        List<ScheduleItem> scheduleItems = DbContext.ScheduleItems.ToList();
        List<Ad> ads = DbContext.Ads.ToList();

        foreach (ScheduleItem item in scheduleItems)
        {
            item.Ad = ads[Random.Shared.Next(ads.Count)];
        }

        DbContext.SaveChanges();
    }

    private void ChangeScheduleItemToRandomInAllScheduleItemRequests()
    {
        List<ScheduleItemRequest> scheduleItemRequests = DbContext.ScheduleItemRequests.ToList();
        List<ScheduleItem> scheduleItems = DbContext.ScheduleItems.ToList();

        foreach (ScheduleItemRequest request in scheduleItemRequests)
        {
            request.ScheduleItem = scheduleItems[Random.Shared.Next(scheduleItems.Count)];
        }

        DbContext.SaveChanges();
    }
}
