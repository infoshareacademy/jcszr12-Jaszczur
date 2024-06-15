using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Tests.Controllers.Browse;
public class BrowseControllerAdsTests : BrowseControllerTestsBase
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(int.MinValue, 1)]
    public async Task Ads_WhenProvidedInvalidPageNumber_ShouldAskForPageOne(int pageNumber, int expectedUsedPageNumber)
    {
        // Arrange
        List<GetBrowseAdsPageRequest> requests = [];
        GetBrowseAdsPageResponse response = CreateGetBrowseAdsPageResponse(success: true);

        MockBrowseService
            .Setup(x => x.GetBrowseAdsPage(Capture.In(requests)))
            .Returns(Task.FromResult(response));

        // Act
        await BrowseController.Ads(pageNumber);

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
        GetBrowseAdsPageResponse response = CreateGetBrowseAdsPageResponse(success: true);

        MockBrowseService
            .Setup(x => x.GetBrowseAdsPage(Capture.In(requests)))
            .Returns(Task.FromResult(response));

        // Act
        await BrowseController.Ads(pageNumber);

        // Assert
        Assert.Equal(expectedUsedPageNumber, requests.Single().PageNumber);
    }

    [Fact]
    public async Task Ads_WhenResponseIsUnsuccessful_ShouldReturnRedirectToAction()
    {
        // Arrange
        int pageNumber = 1;
        GetBrowseAdsPageResponse response = CreateGetBrowseAdsPageResponse(success: false);

        MockBrowseService
           .Setup(x => x.GetBrowseAdsPage(It.IsAny<GetBrowseAdsPageRequest>()))
           .Returns(Task.FromResult(response));

        // Act
        var result = await BrowseController.Ads(pageNumber);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Ads_WhenResponseIsUnsuccessful_ShouldShowFailureMessage()
    {
        // Arrange
        int pageNumber = 1;
        GetBrowseAdsPageResponse response = CreateGetBrowseAdsPageResponse(success: false);

        MockBrowseService
           .Setup(x => x.GetBrowseAdsPage(It.IsAny<GetBrowseAdsPageRequest>()))
           .Returns(Task.FromResult(response));

        MockUiMessagesService
            .Setup(x => x.ShowFailureMessage(It.IsAny<string>()))
            .Verifiable(Times.Once);

        // Act
        var result = await BrowseController.Ads(pageNumber);

        // Assert
        MockUiMessagesService.VerifyAll();
    }

    [Fact]
    public async Task Ads_WhenResponseIsSuccessful_ShouldReturnView()
    {
        // Arrange
        int pageNumber = 1;
        GetBrowseAdsPageResponse response = CreateGetBrowseAdsPageResponse(success: true);

        MockBrowseService
           .Setup(x => x.GetBrowseAdsPage(It.IsAny<GetBrowseAdsPageRequest>()))
           .Returns(Task.FromResult(response));

        // Act
        var result = await BrowseController.Ads(pageNumber);

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    private GetBrowseAdsPageResponse CreateGetBrowseAdsPageResponse(bool success)
    {
        return Fixture
                    .Build<GetBrowseAdsPageResponse>()
                        .With(r => r.Success, success)
                    .Create();
    }
}
