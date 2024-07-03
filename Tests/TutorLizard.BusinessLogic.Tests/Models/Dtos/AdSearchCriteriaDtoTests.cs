using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.BusinessLogic.Tests.Models.Dtos;
public class AdSearchCriteriaDtoTests
{
    [Theory]
    [InlineData("", null, null, null, null, null)]
    [InlineData(null, 0, null, null, null, null)]
    [InlineData(null, null, 0, null, null, null)]
    [InlineData(null, null, null, "", null, null)]
    [InlineData(null, null, null, null, false, null)]
    [InlineData(null, null, null, null, null, 1)]
    [InlineData("", 0, 0, "", false, 1)]
    public void AnySearch_WhenAnySearchValueIsNotNull_ShouldReturnTrue(string? text,
                                                                       int? priceMin,
                                                                       int? priceMax,
                                                                       string? location,
                                                                       bool? isRemote,
                                                                       int? categoryId)
    {
        // Arrange
        bool expected = true;

        // Act
        AdSearchCriteriaDto actual = new()
        {
            Text = text,
            PriceMin = (decimal?)priceMin,
            PriceMax = (decimal?)priceMax,
            Location = location,
            IsRemote = isRemote,
            CategoryId = categoryId,
        };

        // Assert
        Assert.Equal(expected, actual.AnySearch);
    }

    [Fact]
    public void AnySearch_WhenAllSearchValuesAreNull_ShouldReturnFalse()
    {
        // Arrange
        bool expected = false;

        // Act
        AdSearchCriteriaDto actual = new()
        {
            Text = null,
            PriceMin = null,
            PriceMax = null,
            Location = null,
            IsRemote = null,
            CategoryId = null,
        };

        // Assert
        Assert.Equal(expected, actual.AnySearch);
    }
}
