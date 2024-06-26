using Moq;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services.Student
{
    public class StudentServiceScheduleItemRequestTests : StudentServiceTestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateScheduleItemRequest_ShouldSetIsRemoteCorrectly(bool isRemote)
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
            SetupMockGetAllScheduleItems(new List<ScheduleItem> { scheduleItem });

            var request = new CreateScheduleItemRequestRequest
            {
                StudentId = 3,
                ScheduleItemId = 1,
                IsRemote = isRemote
            };

            MockScheduleItemRequestRepository
                .Setup(x => x.Create(It.Is<ScheduleItemRequest>(req => req.IsRemote == isRemote)))
                .Returns((ScheduleItemRequest req) => Task.FromResult(req))
                .Verifiable(Times.Once);

            // Act
            await StudentService.CreateScheduleItemRequest(request);

            // Assert
            MockScheduleItemRequestRepository.VerifyAll();
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
