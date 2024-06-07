using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using Moq;

namespace TutorLizard.BusinessLogic.Tests.Services.Tutor;

public class TutorServiceGetTutorsScheduleForAdTests : TutorServiceTestsBase
{
    [Fact]
    public async Task GetTutorsScheduleForAd_WhenScheduleIsNotNull_ShouldNotReturnEmptyValue()
    {
        // Assign
        var adId = 1;
        int requestCount = 100;
        var scheduleItems = CreateTestScheduleItems(requestCount);

        MockScheduleItemRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(scheduleItems);

        var request = new TutorsScheduleForAdRequest
        {
            AdId = adId
        };

        // Act
        var response = await TutorService.GetTutorsScheduleForAd(request);

        // Assert
        Assert.Equal(scheduleItems.Count, response.ScheduleItems.Count);
    }
}
