using Moq;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Tutor;

public class TutorServiceIsUserTheAdOwnerTests : TutorServiceTestsBase
{
    [Fact]
    public async Task IsUserTheAdOwner_UserIsOwner_ShouldReturnTrue()
    {
        // Arrange
        int usersAdCount = 20;
        int usersScheduleItemRequestCount = 0;
        var testUser = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);       
        var adId = testUser.Ads.First().Id;
        var ad = new Ad { TutorId = testUser.Id };

        MockAdRepository
            .Setup(repo => repo.GetById(adId)).ReturnsAsync(ad);

        var request = new IsUserTheAdOwnerRequest(adId,testUser.Id)
        {
            UserId = testUser.Id,
            AdId = adId           
        };
        // Act
        var response = await TutorService.IsUserTheAdOwner(request);

        // Assert
        Assert.True(response.IsOwner);
    }

    [Fact]
    public async Task IsUserTheAdOwner_UserIsOwner_ShouldReturnFalse()
    {
        // Arrange
        int usersAdCount = 20;
        int usersScheduleItemRequestCount = 0;
        var testUser = CreateTestUserAndAddToDb(usersAdCount, usersScheduleItemRequestCount);       
        var adId = testUser.Ads.First().Id;
        var ad = new Ad { TutorId = -1 };

        MockAdRepository
            .Setup(repo => repo.GetById(adId)).ReturnsAsync(ad);

        var request = new IsUserTheAdOwnerRequest(adId,testUser.Id)
        {
            UserId = testUser.Id, 
            AdId = adId           
        };
        // Act
        var response = await TutorService.IsUserTheAdOwner(request);

        // Assert
        Assert.False(response.IsOwner);
    }
}
