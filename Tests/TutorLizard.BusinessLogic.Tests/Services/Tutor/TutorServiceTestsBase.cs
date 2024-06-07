using AutoFixture;
using Moq;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Tutor;

public class TutorServiceTestsBase : TestsWithInMemoryDbBase
{
    protected TutorService TutorService;
    protected Fixture Fixture = new();
    protected Mock<IDbRepository<ScheduleItem>> MockScheduleItemRepository = new();
    protected Mock<IDbRepository<ScheduleItemRequest>> MockScheduleItemRequestRepository = new();
    protected Mock<IDbRepository<Ad>> MockAdRepository = new();
    protected Mock<IDbRepository<AdRequest>> MockAdRequestRepository = new();

    protected TutorServiceTestsBase() : base()
    {
        TutorService = new(MockScheduleItemRepository.Object,
                           MockScheduleItemRequestRepository.Object,
                           MockAdRequestRepository.Object,
                           MockAdRepository.Object);
    }

    protected void SetupMockGetAllScheduleItems(List<ScheduleItem> scheduleItems) 
    {
        var scheduleItemsInDb = AddEntitiesToInMemoryDb(scheduleItems);

        MockScheduleItemRepository
            .Setup(x => x.GetAll())
            .Returns(scheduleItemsInDb);
    }
    protected void SetupMockGetScheduleItemById(ScheduleItem? scheduleItem) 
    {
        MockScheduleItemRepository
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult(scheduleItem));
    }
    protected void SetupMockGetAllScheduleItemRequests(List<ScheduleItemRequest> scheduleItemRequests)
    {
        var scheduleItemRequestsInDb = AddEntitiesToInMemoryDb(scheduleItemRequests);

        MockScheduleItemRequestRepository
            .Setup(x => x.GetAll())
            .Returns(scheduleItemRequestsInDb);
    }
    protected void SetupMockGetScheduleItemRequestById(ScheduleItemRequest? scheduleItemRequest) 
    {
        MockScheduleItemRequestRepository
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult(scheduleItemRequest));
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
    protected void SetupMockGetAllAdRequests(List<AdRequest> adRequests) 
    {
        var adRequestsInDb = AddEntitiesToInMemoryDb (adRequests);

        MockAdRequestRepository
            .Setup(x => x.GetAll())
            .Returns(adRequestsInDb);
    }
    protected void SetupMockGetAdRequestById(AdRequest? adRequest) 
    {
        MockAdRequestRepository
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(Task.FromResult(adRequest));
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

    protected List<AdRequest> CreateTestAdRequests(int adRequestCount)
    {
        List<AdRequest> adreqs = Fixture
            .Build<AdRequest>()
                .Without(adreq => adreq.Id)
                .Without(adreq => adreq.User)
                .Without(adreq => adreq.Ad)
                .With(adreq=> adreq.User, CreateTestUser())
                .With(adreq => adreq.Message, Fixture.Create<string>())
                .With(adreq => adreq.IsRemote, Fixture.Create<bool>())
                .With(adreq => adreq.IsRemote, Fixture.Create<bool>())
            .CreateMany(adRequestCount)
            .ToList();

        return adreqs;
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
    protected List<ScheduleItem> CreateTestScheduleItemsAsQuerable(int scheduleItemCount)
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
}
