using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data.Repositories;
public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    protected override string FilePath => "Data/categories.json";
    public CategoryRepository()
    {
        LoadFromJson();
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

    public Category? GetcategoryById(int id)
    {
        return _data.Find(x => x.Id == id);
    }

    public void UpdateCategory(Category category)
    {
        var toUpdate = GetcategoryById(category.Id);
        if (toUpdate is null)
            return;

        toUpdate.Name = category.Name;
        toUpdate.Description = category.Description;

        SaveToJson();
    }

    public void DeleteCategory(int id)
    {
        var toDelete = GetcategoryById(id);
        if (toDelete is null)
            return;

        _data.Remove(toDelete);

        SaveToJson();
    }

    private int GetNewId()
    {
        if (_data.Any())
            return _data.Max(x => x.Id);

        return 1;
    }
}
