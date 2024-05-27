using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Browse;
public class BrowseServiceGetBrowseAdsPageTests : BrowseServiceTestsBase
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
}
