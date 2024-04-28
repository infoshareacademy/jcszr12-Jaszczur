using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;
public class BrowseService : IBrowseService
{
    private readonly IDbRepository<Ad> _adRepository;

    public BrowseService(IDbRepository<Ad> adRepository)
    {
        _adRepository = adRepository;
    }
    public async Task<GetBrowseAdsPageResponse> GetBrowseAdsPage(GetBrowseAdsPageRequest request)
    {
        if (request.PageSize < 1 || request.PageNumber < 1)
        {
            return new()
            {
                Success = false,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = 0
            };
        }

        int adCount = await _adRepository.GetAll()
            .CountAsync();

        int totalPages = adCount / request.PageSize;
        if (adCount % request.PageSize != 0)
        {
            totalPages++;
        }

        request.PageNumber = Math.Min(request.PageNumber, totalPages);

        int resultsToSkip = (request.PageNumber - 1) * request.PageSize;
        List<AdListItemDto> ads = await _adRepository.GetAll()
            .Skip(resultsToSkip)
            .Take(request.PageSize)
            .Select(ad => new AdListItemDto()
            {
                Id = ad.Id,
                TutorId = ad.TutorId,
                TutorName = ad.User.Name,
                Subject = ad.Subject,
                Title = ad.Title,
                Description = ad.Description,
                CategoryId = ad.CategoryId,
                CategoryName = ad.Category.Name,
                Price = ad.Price,
                Location = ad.Location,
                IsRemote = ad.IsRemote
            })
            .ToListAsync();

        GetBrowseAdsPageResponse response = new()
        {
            Success = true,
            Ads = ads,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };

        return response;
    }
}
