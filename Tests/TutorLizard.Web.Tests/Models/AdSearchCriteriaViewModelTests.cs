using TutorLizard.Web.Models;

namespace TutorLizard.Web.Tests.Models;
public class AdSearchCriteriaViewModelTests
{
    [Theory]
    [InlineData(true, false, false, false, false, false)]
    [InlineData(false, true, false, false, false, false)]
    [InlineData(false, false, true, false, false, false)]
    [InlineData(false, false, false, true, false, false)]
    [InlineData(false, false, false, false, true, false)]
    [InlineData(false, false, false, false, false, true)]
    [InlineData(true, true, true, true, true, true)]
    public void AnySearch_WhenAnySerchByPropertyIsTrue_ShouldReturnTrue(bool searchByText,
                                                                        bool searchByPriceMin,
                                                                        bool searchByPriceMax,
                                                                        bool searchByLocation,
                                                                        bool searchByIsRemote,
                                                                        bool searchByCategoryId)
    {
        // Arrange
        bool expected = true;

        // Act
        AdSearchCriteriaViewModel actual = new()
        {
            SearchByText = searchByText,
            SearchByPriceMin = searchByPriceMin,
            SearchByPriceMax = searchByPriceMax,
            SearchByLocation = searchByLocation,
            SearchByIsRemote = searchByIsRemote,
            SearchByCategoryId = searchByCategoryId,
        };

        // Assert
        Assert.Equal(expected, actual.AnySearch);
    }

    [Fact]
    public void AnySearch_WhenAllSerchByPropertiesAreFalse_ShouldReturnFalse()
    {
        // Arrange
        bool expected = false;

        // Act
        AdSearchCriteriaViewModel actual = new()
        {
            SearchByText = false,
            SearchByPriceMin = false,
            SearchByPriceMax = false,
            SearchByLocation = false,
            SearchByIsRemote = false,
            SearchByCategoryId = false,
        };

        // Assert
        Assert.Equal(expected, actual.AnySearch);
    }
}
