using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models;
using Moq;

namespace TutorLizard.BusinessLogic.Tests.Services.Tutor;

public class TutorServiceCreateScheduleItemTests : TutorServiceTestsBase
{
    [Fact]
    public async Task CreateScheduleItem_WhenUserIsOwner_ShouldReturnSuccess()
    {
        // Arrange
        int usersAdCount = 20;
        int usersScheduleItemRequestCount = 0;
        var testUser = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);
        var adId = testUser.Ads.First().Id;
        var ad = new Ad { TutorId = testUser.Id };

        MockAdRepository
            .Setup(repo => repo.GetById(adId)).ReturnsAsync(ad);


        var request = new CreateScheduleItemRequest()
        {
            UserId = testUser.Id,
            AdId = adId,
            DateTime = DateTime.Now
        };
        // Act
        var response = await TutorService.CreateScheduleItem(request);
        // Assert
        Assert.True(response.Success);
    }

    [Fact]
    public async Task CreateScheduleItem_WhenUserIsNotTheOwner_ShouldReturnFalse()
    {
        // Arrange
        int usersAdCount = 20;
        int usersScheduleItemRequestCount = 0;
        var testUser = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);
        var adId = testUser.Ads.First().Id;
        var ad = new Ad { TutorId = -1 };

        MockAdRepository
            .Setup(repo => repo.GetById(adId)).ReturnsAsync(ad);


        var request = new CreateScheduleItemRequest()
        {
            UserId = testUser.Id,
            AdId = adId,
            DateTime = DateTime.Now
        };
        // Act
        var response = await TutorService.CreateScheduleItem(request);
        // Assert
        Assert.False(response.Success);
    }    
    
    [Fact]
    public async Task CreateScheduleItem_WhenAdIdIsIncorrect_ShouldReturnFalse()
    {
        // Arrange
        int usersAdCount = 20;
        int usersScheduleItemRequestCount = 0;
        var testUser = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);
        var adId = -1;
        var ad = new Ad { TutorId = testUser.Id };

        MockAdRepository
            .Setup(repo => repo.GetById(adId)).ReturnsAsync(ad);


        var request = new CreateScheduleItemRequest()
        {
            UserId = testUser.Id,
            AdId = adId,
            DateTime = DateTime.Now
        };
        // Act
        var response = await TutorService.CreateScheduleItem(request);
        // Assert
        Assert.False(response.Success);
    }    
}
