using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Browse;

public abstract class BrowseServiceTestsBase
{
    protected BrowseService BrowseService;
    protected JaszczurContext DbContext;
    protected Fixture Fixture = new();
    protected Mock<IDbRepository<Ad>> MockAdRepository = new();
    protected Mock<IDbRepository<ScheduleItem>> MockScheduleItemRepository = new();

    public BrowseServiceTestsBase()
    {
        BrowseService = new(MockAdRepository.Object, MockScheduleItemRepository.Object);
        DbContext = SetupInMemoryDbContext();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }

    private IQueryable<TEntity> AddEntitiesToInMemoryDb<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        DbContext
            .Set<TEntity>()
            .AddRange(entities);
        DbContext.SaveChanges();

        return DbContext
            .Set<TEntity>()
            .AsQueryable();
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

    protected List<Ad> CreateTestAds(int adCount)
    {
        List<Ad> ads = Fixture
            .Build<Ad>()
                .Without(ad => ad.Id)
                .Without(ad => ad.AdRequests)
                .Without(ad => ad.ScheduleItems)
                .With(ad => ad.User, CreateTestUser())
                .With(ad => ad.Category, CreateTestCategory())
                .With(ad => ad.Price, Math.Abs(Fixture.Create<decimal>()))
            .CreateMany(adCount)
            .ToList();

        return ads;
    }

    private Category CreateTestCategory()
    {
        Category category = Fixture
            .Build<Category>()
                .Without(category => category.Ads)
            .Create();

        return category;
    }

    private List<ScheduleItemRequest> CreateTestScheduleItemRequests(int requestCount)
    {
        List<ScheduleItemRequest> requests = Fixture
            .Build<ScheduleItemRequest>()
                .Without(request => request.Id)
                .Without(request => request.ScheduleItem)
                .With(request => request.User, CreateTestUser())
            .CreateMany(requestCount)
            .ToList();

        return requests;
    }

    private List<ScheduleItem> CreateTestScheduleItems(int scheduleItemCount)
    {
        var ads = CreateTestAds(scheduleItemCount);

        List<ScheduleItem> scheduleItems = Fixture
            .Build<ScheduleItem>()
                .Without(item => item.Id)
                .Without(item => item.Ad)
                .Without(item => item.ScheduleItemRequests)
            .CreateMany(scheduleItemCount)
            .ToList();

        return scheduleItems;
    }
    private User CreateTestUser()
    {
        User user = Fixture
            .Build<User>()
                .Without(user => user.Ads)
                .Without(user => user.AdRequests)
                .Without(user => user.ScheduleItemRequests)
            .Create();

        return user;
    }

    protected User CreateTestUserAndAddToDb(int usersAdCount = 0, int usersScheduleItemRequestCount = 0)
    {
        User user = CreateTestUser();
        user.Ads = CreateTestAds(usersAdCount);
        user.ScheduleItemRequests = CreateTestScheduleItemRequests(usersScheduleItemRequestCount);
        AddEntitiesToInMemoryDb([user]);
        return user;
    }

    protected User CreateUserAndGiveHimExistingAds(int usersFinalAdCount)
    {
        User user = CreateTestUserAndAddToDb();

        while (user.Ads.Count < usersFinalAdCount)
        {
            ChangeUserInRandomAdInDb(user);
        }

        return user;
    }

    protected User CreateUserAndGiveHimExistingScheduleItemRequests(int usersFinalScheduleItemRequestCount)
    {
        User user = CreateTestUserAndAddToDb();

        while (user.ScheduleItemRequests.Count < usersFinalScheduleItemRequestCount)
        {
            ChangeUserInRandomScheduleItemRequestInDb(user);
        }

        return user;
    }

    protected JaszczurContext SetupInMemoryDbContext()
    {
        DbContextOptionsBuilder<JaszczurContext> dbBuilder = new();
        dbBuilder.UseInMemoryDatabase(databaseName: $"FakeDb{Guid.NewGuid()}");
        JaszczurContext context = new(dbBuilder.Options);
        return context;
    }

    protected void SetupMockGetAdById(Ad? ad)
    {
        MockAdRepository
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult(ad));
    }

    protected void SetupMockGetAllAds(List<Ad> ads)
    {
        var adsInDb = AddEntitiesToInMemoryDb(ads);
        MockAdRepository
            .Setup(x => x.GetAll())
            .Returns(adsInDb);
    }

    protected void SetupMockGetAllScheduleItems(int scheduleItemCount)
    {
        var scheduleItems = CreateTestScheduleItems(scheduleItemCount);
        var scheduleItemsInDb = AddEntitiesToInMemoryDb(scheduleItems);
        MockScheduleItemRepository
            .Setup(x => x.GetAll())
            .Returns(scheduleItemsInDb);
    }

    protected void SetupMockScheduleData(int scheduleItemCount, int adCount, int scheduleItemRequestCount)
    {
        SetupMockGetAllScheduleItems(scheduleItemCount);

        CreateAdsForScheduleItems(adCount);

        CreateScheduleItemRequestsForScheduleItems(scheduleItemRequestCount);
    }
}