using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

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
    public async Task GetBrowseAdsPage_WhenRequestIsValid_ShouldReturnCorrectTotals(int pageNumber, int pageSize, int expectedTotalAds, int expectedTotalPages)
    {
        // Arrange
        var ads = CreateTestAds(expectedTotalAds);
        SetupMockGetAllAds(ads);

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);
        int actualTotalAds = response.TotalAds;
        int actualTotalPages = response.TotalPages;

        // Assert
        Assert.True(response.Success);
        Assert.Equal(expectedTotalAds, actualTotalAds);
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
            .OrderBy(ad => ad.DateCreated)
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
    public async Task GetBrowseAdsPage_WhenSearchTextIsNotNullOrWhiteSpace_ShouldApplySearchByText()
    {
        // Arrange
        string searchText = Guid.NewGuid().ToString();
        int adCount = 6;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            Text = searchText
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adToFindByTitle = ads[0];
        adToFindByTitle.Title = $"Test {searchText} Test";

        Ad adToFindBySubject = ads[1];
        adToFindBySubject.Subject = searchText.ToUpper();

        Ad adToFindByDescription = ads[2];
        adToFindByDescription.Description = searchText;

        Ad adNotToFindByTitle = ads[3];
        adNotToFindByTitle.Title = searchText[..^1];

        Ad adNotToFindBySubject = ads[4];
        adNotToFindBySubject.Subject = searchText[1..];

        Ad adNotToFindByDescription = ads[5];
        adNotToFindByDescription.Description = searchText[1..^1];

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adToFindByTitle.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adToFindBySubject.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adToFindByDescription.Id);

        Assert.DoesNotContain(response.Ads, ad => ad.Id == adNotToFindByTitle.Id);
        Assert.DoesNotContain(response.Ads, ad => ad.Id == adNotToFindBySubject.Id);
        Assert.DoesNotContain(response.Ads, ad => ad.Id == adNotToFindByDescription.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData("\t")]
    public async Task GetBrowseAdsPage_WhenSearchTextIsNullOrWhiteSpace_ShouldNotApplySearchByText(string? searchText)
    {
        // Arrange
        int adCount = 10;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            Text = searchText
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);
        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Equal(adCount, response.TotalAds);
        foreach (Ad ad in ads)
        {
            Assert.Contains(response.Ads, adDto => adDto.Id == ad.Id);
        }
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchPriceMinIsNotNull_ShouldApplySearchByPriceMin()
    {
        // Arrange
        decimal searchPriceMin = 100m;
        int adCount = 3;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            PriceMin = searchPriceMin
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adWithPriceEqualToPriceMin = ads[0];
        adWithPriceEqualToPriceMin.Price = searchPriceMin;

        Ad adWithPriceLargerThanPriceMin = ads[1];
        adWithPriceLargerThanPriceMin.Price = searchPriceMin + 1;

        Ad adWithPriceLowerThanPriceMin = ads[2];
        adWithPriceLowerThanPriceMin.Price = searchPriceMin - 1;

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceEqualToPriceMin.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceLargerThanPriceMin.Id);
        Assert.DoesNotContain(response.Ads, ad => ad.Id == adWithPriceLowerThanPriceMin.Id);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchPriceMinIsNull_ShouldNotApplySearchByPriceMin()
    {
        // Arrange
        decimal? searchPriceMin = null;
        int adCount = 3;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            PriceMin = searchPriceMin
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adWithPriceDecimalMinValue = ads[0];
        adWithPriceDecimalMinValue.Price = decimal.MinValue;

        Ad adWithPriceDecimalMaxValue = ads[1];
        adWithPriceDecimalMaxValue.Price = decimal.MaxValue;

        Ad adWithPriceZero = ads[2];
        adWithPriceZero.Price = decimal.Zero;

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceDecimalMinValue.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceDecimalMaxValue.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceZero.Id);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchPriceMaxIsNotNull_ShouldApplySearchByPriceMin()
    {
        // Arrange
        decimal searchPriceMax = 100m;
        int adCount = 3;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            PriceMax = searchPriceMax
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adWithPriceEqualToPriceMax = ads[0];
        adWithPriceEqualToPriceMax.Price = searchPriceMax;

        Ad adWithPriceLargerThanPriceMax = ads[1];
        adWithPriceLargerThanPriceMax.Price = searchPriceMax + 1;

        Ad adWithPriceLowerThanPriceMax = ads[2];
        adWithPriceLowerThanPriceMax.Price = searchPriceMax - 1;

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceEqualToPriceMax.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceLowerThanPriceMax.Id);
        Assert.DoesNotContain(response.Ads, ad => ad.Id == adWithPriceLargerThanPriceMax.Id);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchPriceMaxIsNull_ShouldNotApplySearchByPriceMin()
    {
        // Arrange
        decimal? searchPriceMax = null;
        int adCount = 3;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            PriceMax = searchPriceMax
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adWithPriceDecimalMinValue = ads[0];
        adWithPriceDecimalMinValue.Price = decimal.MinValue;

        Ad adWithPriceDecimalMaxValue = ads[1];
        adWithPriceDecimalMaxValue.Price = decimal.MaxValue;

        Ad adWithPriceZero = ads[2];
        adWithPriceZero.Price = decimal.Zero;

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceDecimalMinValue.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceDecimalMaxValue.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adWithPriceZero.Id);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchLocationIsNotNullOrWhiteSpace_ShouldApplySearchByLocation()
    {
        // Arrange
        string searchLocation = Guid.NewGuid().ToString();
        int adCount = 2;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            Location = searchLocation
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adToFindByLocation = ads[0];
        adToFindByLocation.Location = $"Test {searchLocation} Test";

        Ad adNotToFindByLocation = ads[1];
        adNotToFindByLocation.Location = searchLocation[..^1];

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adToFindByLocation.Id);
        Assert.DoesNotContain(response.Ads, ad => ad.Id == adNotToFindByLocation.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData("\t")]
    public async Task GetBrowseAdsPage_WhenSearchLocationIsNullOrWhiteSpace_ShouldNotApplySearchByLocation(string? searchLocation)
    {
        // Arrange
        int adCount = 10;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            Location = searchLocation
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);
        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Equal(adCount, response.TotalAds);
        foreach (Ad ad in ads)
        {
            Assert.Contains(response.Ads, adDto => adDto.Id == ad.Id);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetBrowseAdsPage_WhenSearchIsRemoteIsNotNull_ShouldApplySearchByIsRemote(bool searchIsRemote)
    {
        // Arrange
        int adCount = 2;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            IsRemote = searchIsRemote
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adToFindByIsRemote = ads[0];
        adToFindByIsRemote.IsRemote = searchIsRemote;

        Ad adNotToFindByIsRemote = ads[1];
        adNotToFindByIsRemote.IsRemote = !searchIsRemote;

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adToFindByIsRemote.Id);
        Assert.DoesNotContain(response.Ads, ad => ad.Id == adNotToFindByIsRemote.Id);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchIsRemoteIsNull_ShouldNotApplySearchByIsRemote()
    {
        // Arrange
        bool? searchIsRemote = null;
        int adCount = 2;
        int pageNumber = 1;
        int pageSize = adCount;

        AdSearchCriteriaDto searchCriteria = new()
        {
            IsRemote = searchIsRemote
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        var ads = CreateTestAds(adCount);

        Ad adWithIsRemoteTrue = ads[0];
        adWithIsRemoteTrue.IsRemote = true;

        Ad adWithIsRemoteFalse = ads[1];
        adWithIsRemoteFalse.IsRemote = false;

        SetupMockGetAllAds(ads);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adWithIsRemoteTrue.Id);
        Assert.Contains(response.Ads, ad => ad.Id == adWithIsRemoteFalse.Id);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchCategoryIdIsNotNull_ShouldApplySearchByCategoryId()
    {
        // Arrange
        int adCount = 2;
        int pageNumber = 1;
        int pageSize = adCount;


        var ads = CreateTestAds(adCount);

        Ad adToFindByCategoryId = ads[0];

        Ad adNotToFindByCategoryId = ads[1];
        adNotToFindByCategoryId.Category = CreateTestCategory();
        
        SetupMockGetAllAds(ads);

        int searchCategoryId = adToFindByCategoryId.CategoryId;

        AdSearchCriteriaDto searchCriteria = new()
        {
            CategoryId = searchCategoryId
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Contains(response.Ads, ad => ad.Id == adToFindByCategoryId.Id);
        Assert.DoesNotContain(response.Ads, ad => ad.Id == adNotToFindByCategoryId.Id);
    }

    [Fact]
    public async Task GetBrowseAdsPage_WhenSearchCategoryIdIsNull_ShouldNotApplySearchByCategoryId()
    {
        // Arrange
        int adCount = 2;
        int pageNumber = 1;
        int pageSize = adCount;


        var ads = CreateTestAds(adCount);

        ads[0].Category = CreateTestCategory();
        ads[1].Category = CreateTestCategory();

        SetupMockGetAllAds(ads);

        int? searchCategoryId = null;

        AdSearchCriteriaDto searchCriteria = new()
        {
            CategoryId = searchCategoryId
        };

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        // Act
        var response = await BrowseService.GetBrowseAdsPage(request);

        // Assert
        foreach (Ad ad in ads)
        {
            Assert.Contains(response.Ads, adDto => adDto.Id == ad.Id);
        }
    }
}
