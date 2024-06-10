using Microsoft.EntityFrameworkCore;
using TutorLizard.Shared.Enums;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Services;
public class BrowseService : IBrowseService
{
    private readonly IDbRepository<Ad> _adRepository;
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;

    public BrowseService(IDbRepository<Ad> adRepository,
                         IDbRepository<ScheduleItem> _scheduleItemRepository)
    {
        _adRepository = adRepository;
        this._scheduleItemRepository = _scheduleItemRepository;
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
        if (adCount == 0 || adCount % request.PageSize != 0)
        {
            totalPages++;
        }

        request.PageNumber = Math.Min(request.PageNumber, totalPages);

        int resultsToSkip = Math.Max((request.PageNumber - 1) * request.PageSize, 0);
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

    public async Task<GetAdDetailsResponse?> GetAdDetails(GetAdDetailsRequest request)
    {
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

        return response;
    }

    public async Task<GetUsersScheduleResponse> GetUsersSchedule(GetUsersScheduleRequest request)
    {
        List<TutorsScheduleItemSummaryDto> tutorsSchedule = await _scheduleItemRepository.GetAll()
            .Where(i => i.Ad.TutorId == request.UserId)
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
            .Where(i => i.ScheduleItemRequests.Any(r => r.StudentId == request.UserId))
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
            StudentsSchedule = studentsSchedule
        };

        return response;
    }
}
