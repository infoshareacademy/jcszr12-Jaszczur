using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;
public interface ICategoryService
{
    Task<GetCategoriesResponse> GetCategories(GetCategoriesRequest request);
}