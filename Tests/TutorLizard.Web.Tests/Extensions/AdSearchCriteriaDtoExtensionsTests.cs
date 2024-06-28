using AutoFixture;
using TutorLizard.Blazor.Models;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Web.Extensions;

namespace TutorLizard.Web.Tests.Extensions;
public class AdSearchCriteriaDtoExtensionsTests
{
    Fixture _fixture = new();

    [Fact]
    public void ToBase64String_WhenAnySearchIsTrue_ShouldReturnNotEmptyString()
    {
        // Arrange
        AdSearchCriteriaDto dto = _fixture
            .Build<AdSearchCriteriaDto>()
                .With(dto => dto.Text, "")
                .With(dto => dto.PriceMin, 0)
                .With(dto => dto.PriceMax, 0)
                .With(dto => dto.Location, "")
                .With(dto => dto.IsRemote, false)
                .With(dto => dto.CategoryId, 1)
            .Create();

        // Act
        string actual = dto.ToBase64String();

        // Assert
        Assert.NotNull(actual);
        Assert.False(String.IsNullOrEmpty(actual));
        Assert.False(String.IsNullOrWhiteSpace(actual));
    }

    [Fact]
    public void ToBase64String_WhenAnySearchIsFalse_ShouldReturnEmptyString()
    {
        // Arrange
        AdSearchCriteriaDto dto = _fixture
            .Build<AdSearchCriteriaDto>()
                .With(dto => dto.Text, (string?)null)
                .With(dto => dto.PriceMin, (decimal?)null)
                .With(dto => dto.PriceMax, (decimal?)null)
                .With(dto => dto.Location, (string?)null)
                .With(dto => dto.IsRemote, (bool?)null)
                .With(dto => dto.CategoryId, (int?)null)
            .Create();

        // Act
        string actual = dto.ToBase64String();

        // Assert
        Assert.NotNull(actual);
        Assert.True(String.IsNullOrEmpty(actual));
        Assert.True(String.IsNullOrWhiteSpace(actual));
    }

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

    [Fact]
    public void ToDto_WhenSearchByPropertiesAreTrue_SetsSearchValues()
    {
        // Arrange
        AdSearchCriteriaViewModel viewModel = _fixture
            .Build<AdSearchCriteriaViewModel>()
                .With(vm => vm.SearchByText, true)
                .With(vm => vm.SearchByPriceMin, true)
                .With(vm => vm.SearchByPriceMax, true)
                .With(vm => vm.SearchByLocation, true)
                .With(vm => vm.SearchByIsRemote, true)
                .With(vm => vm.SearchByCategoryId, true)
            .Create();

        // Act
        AdSearchCriteriaDto dto = viewModel.ToDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(viewModel.Text, dto.Text);
        Assert.Equal(viewModel.PriceMin, dto.PriceMin);
        Assert.Equal(viewModel.PriceMax, dto.PriceMax);
        Assert.Equal(viewModel.Location, dto.Location);
        Assert.Equal(viewModel.IsRemote, dto.IsRemote);
        Assert.Equal(viewModel.CategoryId, dto.CategoryId);
    }

    [Fact]
    public void ToDto_WhenSearchByPropertiesAreFalse_SetsNullSearchValues()
    {
        // Arrange
        AdSearchCriteriaViewModel viewModel = _fixture
            .Build<AdSearchCriteriaViewModel>()
                .With(vm => vm.SearchByText, false)
                .With(vm => vm.SearchByPriceMin, false)
                .With(vm => vm.SearchByPriceMax, false)
                .With(vm => vm.SearchByLocation, false)
                .With(vm => vm.SearchByIsRemote, false)
                .With(vm => vm.SearchByCategoryId, false)
            .Create();

        // Act
        AdSearchCriteriaDto dto = viewModel.ToDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Null(dto.Text);
        Assert.Null(dto.PriceMin);
        Assert.Null(dto.PriceMax);
        Assert.Null(dto.Location);
        Assert.Null(dto.IsRemote);
        Assert.Null(dto.CategoryId);
    }


    [Fact]
    public void ToViewModel_WhenSearchValueIsNotNull_ShouldCorrectlySetProperties()
    {
        // Arrange
        AdSearchCriteriaDto expected = _fixture.Create<AdSearchCriteriaDto>();

        // Act
        AdSearchCriteriaViewModel actual = expected.ToViewModel();

        // Assert
        Assert.NotNull(actual);

        Assert.True(actual.SearchByText);
        Assert.True(actual.SearchByPriceMin);
        Assert.True(actual.SearchByPriceMax);
        Assert.True(actual.SearchByLocation);
        Assert.True(actual.SearchByIsRemote);
        Assert.True(actual.SearchByCategoryId);

        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.PriceMin, actual.PriceMin);
        Assert.Equal(expected.PriceMax, actual.PriceMax);
        Assert.Equal(expected.Location, actual.Location);
        Assert.Equal(expected.IsRemote, actual.IsRemote);
        Assert.Equal(expected.CategoryId, actual.CategoryId);
    }

    [Fact]
    public void ToViewModel_WhenSearchValueIsNull_ShouldCorrectlySetProperties()
    {
        // Arrange
        AdSearchCriteriaDto dto = _fixture
            .Build<AdSearchCriteriaDto>()
                .With(dto => dto.Text, (string?)null)
                .With(dto => dto.PriceMin, (decimal?)null)
                .With(dto => dto.PriceMax, (decimal?)null)
                .With(dto => dto.Location, (string?)null)
                .With(dto => dto.IsRemote, (bool?)null)
                .With(dto => dto.CategoryId, (int?)null)
            .Create();

        // Act
        AdSearchCriteriaViewModel actual = dto.ToViewModel();

        // Assert
        Assert.NotNull(actual);

        Assert.False(actual.SearchByText);
        Assert.False(actual.SearchByPriceMin);
        Assert.False(actual.SearchByPriceMax);
        Assert.False(actual.SearchByLocation);
        Assert.False(actual.SearchByIsRemote);
        Assert.False(actual.SearchByCategoryId);

        Assert.Equal("", actual.Text);
        Assert.Equal(0, actual.PriceMin);
        Assert.Equal(0, actual.PriceMax);
        Assert.Equal("", actual.Location);
        Assert.Equal(false, actual.IsRemote);
        Assert.Equal(1, actual.CategoryId);
    }
}
