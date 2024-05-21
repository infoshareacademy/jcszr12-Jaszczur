using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services;
public class BrowseServiceTests : IDisposable
{
    private readonly BrowseService _browseService;
    private readonly Mock<IDbRepository<Ad>> _mockAdRepository = new();
    private readonly Mock<IDbRepository<ScheduleItem>> _mockScheduleItemRepository = new();
    private readonly Fixture _fixture = new();
    private readonly JaszczurContext _dbContext;
    public BrowseServiceTests()
    {
        _browseService = new(_mockAdRepository.Object, _mockScheduleItemRepository.Object);
        _dbContext = SetupInMemoryDbContext();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, -1)]
    public async Task GetBrowseAdsPage_WhenRequestIsInvalid_ShouldReturnUnsuccessfulResponse(int pageSize, int pageNumber)
    {
        // Arrange
        GetBrowseAdsPageRequest request = new(pageNumber, pageSize);
        GetBrowseAdsPageResponse expected = new()
        {
            Success = false,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = 0
        };

        // Act
        var actual = await _browseService.GetBrowseAdsPage(request);

        // Assert
        Assert.Equivalent(expected, actual);
    }
    private List<Ad> CreateTestAds(int adCount)
    {
        User user = _fixture
            .Build<User>()
                .Without(user => user.Ads)
                .Without(user => user.AdRequests)
                .Without(user => user.ScheduleItemRequests)
            .Create();

        Category category = _fixture
            .Build<Category>()
                .Without(category => category.Ads)
            .Create();

        List<Ad> ads = _fixture
            .Build<Ad>()
                .Without(ad => ad.Id)
                .Without(ad => ad.AdRequests)
                .Without(ad => ad.ScheduleItems)
                .With(ad => ad.User, user)
                .With(ad => ad.Category, category)
                .With(ad => ad.Price, Math.Abs(_fixture.Create<decimal>()))
            .CreateMany(adCount)
            .ToList();

        return ads;
    }

    private void SetupMockGetAllAds(List<Ad> ads)
    {
        var adsInDb = AddAdsToInMemoryDb(ads);
        _mockAdRepository
            .Setup(x => x.GetAll())
            .Returns(adsInDb);
    }

    private IQueryable<Ad> AddAdsToInMemoryDb(List<Ad> ads)
    {
        _dbContext.Ads.AddRange(ads);
        _dbContext.SaveChanges();

        return _dbContext.Ads
            .AsQueryable();
    }

    private JaszczurContext SetupInMemoryDbContext()
    {
        DbContextOptionsBuilder<JaszczurContext> dbBuilder = new();
        dbBuilder.UseInMemoryDatabase(databaseName: $"FakeDb{Guid.NewGuid()}");
        JaszczurContext context = new(dbBuilder.Options);
        return context;
    }
}
