using Microsoft.EntityFrameworkCore;
using TutorLizard.Shared.Enums;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;
using Microsoft.Extensions.Logging;
using TutorLizard.BusinessLogic.Extensions;

namespace TutorLizard.BusinessLogic.Services;
public class BrowseService : IBrowseService
{
    private readonly IDbRepository<Ad> _adRepository;
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;
    private readonly ILogger<BrowseService> _logger;

    public BrowseService(IDbRepository<Ad> adRepository,
                         IDbRepository<ScheduleItem> _scheduleItemRepository,
                         ILogger<BrowseService> logger)
    {
        _adRepository = adRepository;
        this._scheduleItemRepository = _scheduleItemRepository;
        _logger = logger;
    }
    public async Task<GetBrowseAdsPageResponse> GetBrowseAdsPage(GetBrowseAdsPageRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetBrowseAdsPage), request);

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

        IQueryable<Ad> adsQuery = ApplySearchCriteria(_adRepository.GetAll(), request.SearchCriteria);
        (int totalPages, int totalAds) = await GetTotalCounts(adsQuery, request);
        adsQuery = ApplyPagination(adsQuery, request, totalPages);

        List<AdListItemDto> ads = await adsQuery
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
            TotalPages = totalPages,
            TotalAds = totalAds,
            SearchCriteria = request.SearchCriteria
        };

        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetAdDetailsResponse?> GetAdDetails(GetAdDetailsRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetAdDetails), request);

        GetAdDetailsResponse? response = await _adRepository.GetAll()
            .Where(a => a.Id == request.AdId)
            .Select(a => new GetAdDetailsResponse
            {
                AdId = a.Id,
                TutorId = a.TutorId,
                TutorName = a.User.Name,
                Title = a.Title,
                CategoryId = a.CategoryId,
                CategoryName = a.Category.Name,
                Subject = a.Subject,
                Location = a.Location,
                Price = a.Price,
                IsRemote = a.IsRemote,
                Description = a.Description,
                UserRelationship =
                    a.TutorId == request.UserId ? AdToUserRelationship.Owner
                    : a.AdRequests.Any(r => r.StudentId == request.UserId && r.IsAccepted ) ? AdToUserRelationship.AcceptedStudent
                    : a.AdRequests.Any(r => r.StudentId == request.UserId) ? AdToUserRelationship.PendingStudent
                    : AdToUserRelationship.None
            })
            .FirstOrDefaultAsync();

        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetUsersScheduleResponse> GetUsersSchedule(GetUsersScheduleRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetUsersSchedule), request);

        List<TutorsScheduleItemSummaryDto> tutorsSchedule = await _scheduleItemRepository.GetAll()
            .Where(i => i.Ad.TutorId == request.UserId &&
                                  i.DateTime.Month == request.Month &&
                                  i.DateTime.Year == request.Year)
            .OrderBy(i => i.DateTime)
            .Select(i => new TutorsScheduleItemSummaryDto()
            {
                Id = i.Id,
                AdId = i.AdId,
                AdTitle = i.Ad.Title,
                DateTime = i.DateTime,
                RequestCount = i.ScheduleItemRequests.Count,
                AcceptedStudentsName =
                    i.ScheduleItemRequests.Any(r => r.IsAccepted) ?
                    i.ScheduleItemRequests.First(r => r.IsAccepted).User.Name
                    : null,
            })
            .ToListAsync();


        List<StudentsScheduleItemSummaryDto> studentsSchedule = await _scheduleItemRepository.GetAll()
            .Where(i => i.ScheduleItemRequests.Any(r => r.StudentId == request.UserId) &&
                                           i.DateTime.Month == request.Month &&
                                           i.DateTime.Year == request.Year)
            .OrderBy(i => i.DateTime)
            .Select(i => new StudentsScheduleItemSummaryDto()
            {
                Id = i.Id,
                AdId = i.AdId,
                AdTitle = i.Ad.Title,
                TutorName = i.Ad.User.Name,
                DateTime = i.DateTime,
                Status =
                    i.ScheduleItemRequests.Any(r => r.StudentId == request.UserId && r.IsAccepted) ? StudentsScheduleItemSummaryDto.RequestStatus.Accepted
                    : i.ScheduleItemRequests.Any(r => r.IsAccepted) ? StudentsScheduleItemSummaryDto.RequestStatus.Rejected
                    : StudentsScheduleItemSummaryDto.RequestStatus.Pending
            })
            .ToListAsync();

        GetUsersScheduleResponse response = new()
        {
            TutorsSchedule = tutorsSchedule,
            StudentsSchedule = studentsSchedule,
            Month = request.Month,
            Year = request.Year,
        };

        _logger.LogReturningResponse(response);
        return response;
    }
    private IQueryable<Ad> ApplySearchCriteria(IQueryable<Ad> ads, AdSearchCriteriaDto searchCriteria)
    {
        ads = ApplySearchByText(ads, searchCriteria.Text);
        ads = ApplySearchByPriceMin(ads, searchCriteria.PriceMin);
        ads = ApplySearchByPriceMax(ads, searchCriteria.PriceMax);
        ads = ApplySearchByLocation(ads, searchCriteria.Location);
        ads = ApplySearchByIsRemote(ads, searchCriteria.IsRemote);
        ads = ApplySearchByCategoryId(ads, searchCriteria.CategoryId);

        return ads;
    }

    private IQueryable<Ad> ApplySearchByText(IQueryable<Ad> ads, string? text)
    {
        if (String.IsNullOrWhiteSpace(text))
        {
            return ads;
        }

        text = text.ToLower();

        ads = ads.Where(ad =>
            ad.Title.ToLower().Contains(text) ||
            ad.Subject.ToLower().Contains(text) ||
            ad.Description.ToLower().Contains(text));

        return ads;
    }

    private IQueryable<Ad> ApplySearchByPriceMin(IQueryable<Ad> ads, decimal? priceMin)
    {
        if (priceMin is null)
        {
            return ads;
        }

        ads = ads.Where(ad => ad.Price >= priceMin);

        return ads;
    }

    private IQueryable<Ad> ApplySearchByPriceMax(IQueryable<Ad> ads, decimal? priceMax)
    {
        if (priceMax is null)
        {
            return ads;
        }

        ads = ads.Where(ad => ad.Price <= priceMax);

        return ads;
    }

    private IQueryable<Ad> ApplySearchByLocation(IQueryable<Ad> ads, string? location)
    {
        if (String.IsNullOrWhiteSpace(location))
        {
            return ads;
        }

        location = location.ToLower();

        ads = ads.Where(ad => ad.Location.ToLower().Contains(location));

        return ads;
    }

    private IQueryable<Ad> ApplySearchByIsRemote(IQueryable<Ad> ads, bool? isRemote)
    {
        if (isRemote is null)
        {
            return ads;
        }

        ads = ads.Where(ad => ad.IsRemote == isRemote);

        return ads;
    }

    private IQueryable<Ad> ApplySearchByCategoryId(IQueryable<Ad> ads, int? categoryId)
    {
        if (categoryId is null)
        {
            return ads;
        }

        ads = ads.Where(ad => ad.CategoryId == categoryId);

        return ads;
    }

    private async Task<(int totalPages, int totalAds)> GetTotalCounts(IQueryable<Ad> ads, GetBrowseAdsPageRequest request)
    {
        int totalAds = await ads.CountAsync();

        int totalPages = totalAds / request.PageSize;
        if (totalAds == 0 || totalAds % request.PageSize != 0)
        {
            totalPages++;
        }

        return (totalPages, totalAds);
    }
    private IQueryable<Ad> ApplyPagination(IQueryable<Ad> ads, GetBrowseAdsPageRequest request, int totalPages)
    {
        request.PageNumber = Math.Min(request.PageNumber, totalPages);

        int resultsToSkip = Math.Max((request.PageNumber - 1) * request.PageSize, 0);

        ads = ads
            .OrderBy(ad => ad.DateCreated)
            .Skip(resultsToSkip)
            .Take(request.PageSize);

        return ads;
    }
}
