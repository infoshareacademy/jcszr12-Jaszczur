using Microsoft.Extensions.Options;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Options;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json;
public class CategoryJsonRepository : JsonRepositoryBase<Category>, ICategoryRepository
{
    public CategoryJsonRepository(IOptions<DataJsonFilePaths> options) : base(options.Value.Categories)
    {
    }

    public Category CreateCategory(string name, string? description = null)
    {
        Category newCategory = new(GetNewId(), name, description);
        _data.Add(newCategory);
        SaveToJson();

        return newCategory;
    }

    public List<Category> GetAllCategories()
    {
        return _data;
    }

    public Category? GetCategoryById(int id)
    {
        return _data.Find(x => x.Id == id);
    }

    public void UpdateCategory(Category category)
    {
        var toUpdate = GetCategoryById(category.Id);
        if (toUpdate is null)
            return;

        toUpdate.Name = category.Name;
        toUpdate.Description = category.Description;

        SaveToJson();
    }

    public void DeleteCategoryById(int id)
    {
        var toDelete = GetCategoryById(id);
        if (toDelete is null)
            return;

        _data.Remove(toDelete);

        SaveToJson();
    }

    private int GetNewId()
    {
        if (_data.Any())
            return _data.Max(x => x.Id) + 1;

        return 1;
    }
}
