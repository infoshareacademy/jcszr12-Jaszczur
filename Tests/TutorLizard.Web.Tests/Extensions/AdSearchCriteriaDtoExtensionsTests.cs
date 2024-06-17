using AutoFixture;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.Web.Extensions;

namespace TutorLizard.Web.Tests.Extensions;
public class AdSearchCriteriaDtoExtensionsTests
{
    Fixture _fixture = new();

    [Fact]
    public void ToAdSearchCriteriaDto_WhenCalledOnResultOfToBase64String_ShouldCorrectlyDeserialize()
    {
        // Arrange
        AdSearchCriteriaDto expected = _fixture.Create<AdSearchCriteriaDto>();
        string serialized = expected.ToBase64String();

        // Act
        AdSearchCriteriaDto? actual = serialized.ToAdSearchCriteriaDto();

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public void ToSearchCriteriaDto_WhenCalledOnRandomString_ShouldReturnNull()
    {
        // Arrange
        string input = _fixture.Create<string>();

        // Act
        AdSearchCriteriaDto? actual = input.ToAdSearchCriteriaDto();

        Assert.Null(actual);
    }
}
