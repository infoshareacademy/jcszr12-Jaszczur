using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Enums;
using TutorLizard.Shared.Models.DTOs.Requests;

namespace TutorLizard.BusinessLogic.Tests.Services.Browse;
public class BrowseServiceGetAdDetailsTests : BrowseServiceTestsBase
{
    [Fact]
    public async Task GetAdDetails_WhenAdDoesntExist_ShouldReturnNull()
    {
        // Arrange
        int adId = 1;
        int userId = 19;
        GetAdDetailsRequest request = new()
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
        GetAdDetailsRequest request = new()
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

        GetAdDetailsRequest request = new()
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

        GetAdDetailsRequest request = new()
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

        GetAdDetailsRequest request = new()
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
        GetAdDetailsRequest request = new()
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
}
