using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Student
{
    public class StudentServiceTestBase : TestsWithInMemoryDbBase
    {
        protected StudentService StudentService;
        protected Fixture Fixture = new();
        protected Mock<IDbRepository<Ad>> MockAdRepository = new();
        protected Mock<IDbRepository<AdRequest>> MockAdRequestRepository = new();
        protected Mock<IDbRepository<ScheduleItem>> MockScheduleItemRepository = new();
        protected Mock<IDbRepository<ScheduleItemRequest>> MockScheduleItemRequestRepository = new();
        protected Mock<ILogger<StudentService>> MockLogger = new();

        protected StudentServiceTestBase() : base()
        {
            StudentService = new StudentService(MockAdRepository.Object,
                                                MockAdRequestRepository.Object,
                                                MockScheduleItemRepository.Object,
                                                MockScheduleItemRequestRepository.Object,
                                                MockLogger.Object);
        }

        protected void SetupMockGetScheduleItemById(ScheduleItem? scheduleItem)
        {
            MockScheduleItemRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(scheduleItem));
        }

        protected void SetupMockGetAllScheduleItems(List<ScheduleItem> scheduleItems)
        {
            var scheduleItemsInDb = AddEntitiesToInMemoryDb(scheduleItems);
            MockScheduleItemRepository
                .Setup(x => x.GetAll())
                .Returns(scheduleItemsInDb);
        }
    }
}
