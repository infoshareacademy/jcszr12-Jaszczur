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
}
