using System.Text.Json;
using System.Text.Json.Serialization;
using TutorLizard.Blazor.Models;
using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.Web.Extensions;

public static class AdSearchCriteriaDtoExtensions
{
    static JsonSerializerOptions _serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    public static string ToBase64String(this AdSearchCriteriaDto searchCriteria)
    {
        if (searchCriteria.AnySearch == false)
        {
            return "";
        }
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(searchCriteria, _serializerOptions);
        return Convert.ToBase64String(bytes);
    }

    public static AdSearchCriteriaDto? ToAdSearchCriteriaDto(this string base64String)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return JsonSerializer.Deserialize<AdSearchCriteriaDto>(bytes, _serializerOptions);
        }
        catch
        {
            return null;
        }
    }

    public static AdSearchCriteriaDto ToDto(this AdSearchCriteriaViewModel viewModel)
    {
        return new AdSearchCriteriaDto()
        {
            Text = viewModel.SearchByText ? viewModel.Text : null,
            PriceMin = viewModel.SearchByPriceMin ? viewModel.PriceMin : null,
            PriceMax = viewModel.SearchByPriceMax ? viewModel.PriceMax : null,
            Location = viewModel.SearchByLocation ? viewModel.Location : null,
            IsRemote = viewModel.SearchByIsRemote ? viewModel.IsRemote : null,
            CategoryId = viewModel.SearchByCategoryId ? viewModel.CategoryId : null,
        };
    }

    public static AdSearchCriteriaViewModel ToViewModel(this AdSearchCriteriaDto dto)
    {
        return new AdSearchCriteriaViewModel()
        {
            SearchByText = String.IsNullOrEmpty(dto.Text) == false,
            SearchByPriceMin = dto.PriceMin is not null,
            SearchByPriceMax = dto.PriceMax is not null,
            SearchByLocation = String.IsNullOrEmpty(dto.Location) == false,
            SearchByIsRemote = dto.IsRemote is not null,
            SearchByCategoryId = dto.CategoryId is not null,
            Text = dto.Text ?? "",
            PriceMin = dto.PriceMin ?? 0,
            PriceMax = dto.PriceMax ?? 0,
            Location = dto.Location ?? "",
            IsRemote = dto.IsRemote ?? false,
            CategoryId = dto.CategoryId ?? 1,
        };
    }
}
