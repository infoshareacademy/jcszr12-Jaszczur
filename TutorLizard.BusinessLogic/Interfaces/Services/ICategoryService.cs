using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Services;
public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategories();
}