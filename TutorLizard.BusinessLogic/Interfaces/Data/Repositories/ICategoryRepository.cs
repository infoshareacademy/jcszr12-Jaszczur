using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
public interface ICategoryRepository
{
    Category CreateCategory(string name, string? description = null);
    void DeleteCategory(int id);
    List<Category> GetAllCategories();
    Category? GetcategoryById(int id);
    void UpdateCategory(Category category);
}