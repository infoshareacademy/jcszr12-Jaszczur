using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
public interface ICategoryRepository
{
    Category CreateCategory(string name, string? description = null);
    void DeleteCategoryById(int id);
    List<Category> GetAllCategories();
    Category? GetCategoryById(int id);
    void UpdateCategory(Category category);
}