using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;
public interface ICategoryService
{
    Task<GetCategoriesResponse> GetCategories(GetCategoriesRequest request);
}