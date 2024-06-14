using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Services;
public class CategoryService : ICategoryService
{
    private readonly IDbRepository<Category> _categoryRepository;

    public CategoryService(IDbRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetAllCategories()
    {
        return await _categoryRepository
            .GetAll()
            .Select(category => category.ToDto())
            .ToListAsync();
    }
}
