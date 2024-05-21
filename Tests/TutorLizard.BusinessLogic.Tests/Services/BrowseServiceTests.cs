using Moq;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.BusinessLogic.Tests.Services;
public class BrowseServiceTests
{
    private readonly BrowseService _browseService;
    private readonly Mock<IDbRepository<Ad>> _mockAdRepository = new();
    private readonly Mock<IDbRepository<ScheduleItem>> _mockScheduleItemRepository = new();
    public BrowseServiceTests()
    {
        _browseService = new(_mockAdRepository.Object, _mockScheduleItemRepository.Object);
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
}
