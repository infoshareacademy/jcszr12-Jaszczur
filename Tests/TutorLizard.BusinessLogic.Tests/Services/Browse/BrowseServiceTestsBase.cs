using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Browse;

public abstract class BrowseServiceTestsBase : TestsWithInMemoryDbBase
{
    protected BrowseService BrowseService;
    protected Fixture Fixture = new();
    protected Mock<IDbRepository<Ad>> MockAdRepository = new();
    protected Mock<IDbRepository<ScheduleItem>> MockScheduleItemRepository = new();

    protected BrowseServiceTestsBase() : base()
    {
        BrowseService = new(MockAdRepository.Object, MockScheduleItemRepository.Object);
    }

    protected void SetupMockGetAllAds(List<Ad> ads)
    {
        var adsInDb = AddEntitiesToInMemoryDb(ads);
        MockAdRepository
            .Setup(x => x.GetAll())
            .Returns(adsInDb);
    }

    protected void SetupMockGetAdById(Ad? ad)
    {
        MockAdRepository
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult(ad));
    }

    protected void SetupMockGetAllScheduleItems(int scheduleItemCount)
    {
        var scheduleItems = CreateTestScheduleItems(scheduleItemCount);
        var scheduleItemsInDb = AddEntitiesToInMemoryDb(scheduleItems);
        MockScheduleItemRepository
            .Setup(x => x.GetAll())
            .Returns(scheduleItemsInDb);
    }

    protected User CreateTestUserAndAddToDb(int usersAdCount = 0, int usersScheduleItemRequestCount = 0)
    {
        User user = CreateTestUser();
        user.Ads = CreateTestAds(usersAdCount);
        user.ScheduleItemRequests = CreateTestScheduleItemRequests(usersScheduleItemRequestCount);
        AddEntitiesToInMemoryDb([user]);
        return user;
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

    protected User CreateTestUser()
    {
        User user = Fixture
            .Build<User>()
                .Without(user => user.Ads)
                .Without(user => user.AdRequests)
                .Without(user => user.ScheduleItemRequests)
            .Create();

        return user;
    }

    protected Category CreateTestCategory()
    {
        Category category = Fixture
            .Build<Category>()
                .Without(category => category.Ads)
            .Create();

        return category;
    }

    protected List<ScheduleItemRequest> CreateTestScheduleItemRequests(int requestCount)
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

    protected List<ScheduleItem> CreateTestScheduleItems(int scheduleItemCount)
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

    protected IQueryable<TEntity> AddEntitiesToInMemoryDb<TEntity>(List<TEntity> entities)
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
}