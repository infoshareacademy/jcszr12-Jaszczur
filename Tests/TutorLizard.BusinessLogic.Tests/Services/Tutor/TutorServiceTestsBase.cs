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
}
