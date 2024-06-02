using Moq;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
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
            Assert.NotNull(response.CreatedScheduleItemRequestId);

            var createdScheduleItemRequest = DbContext.ScheduleItemRequests.FirstOrDefault();
            Assert.NotNull(createdScheduleItemRequest);
            Assert.True(createdScheduleItemRequest.IsRemote);
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
            Assert.NotNull(response.CreatedScheduleItemRequestId);
        }





    }
}
