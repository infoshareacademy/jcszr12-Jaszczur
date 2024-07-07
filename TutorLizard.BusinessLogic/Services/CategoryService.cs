using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Services;
public class CategoryService : ICategoryService
{
    private readonly IDbRepository<Category> _categoryRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IDbRepository<Category> categoryRepository,
                           ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<GetCategoriesResponse> GetCategories(GetCategoriesRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetCategories), request);

        List<CategoryDto> categories = await _categoryRepository
            .GetAll()
            .Select(category => category.ToDto())
            .ToListAsync();

        GetCategoriesResponse response = new()
        {
            Success = true,
            Categories = categories
        };

        _logger.LogReturningResponse(response);
        return response;
    }
}
