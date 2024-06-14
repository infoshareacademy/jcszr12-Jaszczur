using Moq;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Student
{
    public class StudentServiceScheduleItemRequestTests : StudentServiceTestBase
    {
        [Fact]
        public async Task CreateScheduleItemRequest_WhenIsRemoteIsTrue_ShouldSetIsRemoteToTrue()
        {
            // Arrange
            var ad = new Ad
            {
                Description = "Test Description",
                Location = "Test Location",
                Subject = "Test Subject",
                Title = "Test Title",
                TutorId = 2
            };
            var scheduleItem = new ScheduleItem { Id = 1, Ad = ad };
            SetupMockGetScheduleItemById(scheduleItem);

            var scheduleItems = new List<ScheduleItem> { scheduleItem };
            SetupMockGetAllScheduleItems(scheduleItems);

            var request = new CreateScheduleItemRequestRequest
            {
                StudentId = 3,
                ScheduleItemId = 1,
                IsRemote = true
            };

            MockScheduleItemRequestRepository
                .Setup(x => x.Create(It.Is<ScheduleItemRequest>(req => req.IsRemote == true)))
                .Returns((ScheduleItemRequest req) => Task.FromResult(req))
                .Verifiable(Times.Once);

            // Act
            await StudentService.CreateScheduleItemRequest(request);

            // Assert
            MockScheduleItemRequestRepository.VerifyAll();

        }

        [Fact]
        public async Task CreateScheduleItemRequest_WhenIsRemoteIsFalse_ShouldSetIsRemoteToFalse()
        {
            // Arrange
            var scheduleItem = new ScheduleItem { Id = 1, Ad = new Ad { TutorId = 2 } };
            SetupMockGetScheduleItemById(scheduleItem);

            var request = new CreateScheduleItemRequestRequest
            {
                StudentId = 2,
                ScheduleItemId = 1,
                IsRemote = false
            };

            // Act
            await StudentService.CreateScheduleItemRequest(request);

            // Assert
            MockScheduleItemRequestRepository.Verify(repo => repo.Create(It.Is<ScheduleItemRequest>(req => req.IsRemote == false)), Times.Once);
        }

        [Fact]
        public async Task CreateScheduleItemRequest_WhenRequestSent_ShouldReturnSuccess()
        {
            // Arrange
            var scheduleItem = new ScheduleItem { Id = 1 };
            var studentId = 1;
            var isRemote = true;

            SetupMockGetScheduleItemById(scheduleItem);

            var request = new CreateScheduleItemRequestRequest
            {
                StudentId = studentId,
                ScheduleItemId = scheduleItem.Id,
                IsRemote = isRemote
            };

            // Act
            var response = await StudentService.CreateScheduleItemRequest(request);

            // Assert
            Assert.True(response.Success);
        }





    }
}
