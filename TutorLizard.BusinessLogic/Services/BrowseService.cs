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
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;

    public BrowseService(IDbRepository<Ad> adRepository,
                         IDbRepository<ScheduleItem> _scheduleItemRepository)
    {
        _adRepository = adRepository;
        this._scheduleItemRepository = _scheduleItemRepository;
    }
    public async Task<GetBrowseAdsPageResponse> GetBrowseAdsPage(GetBrowseAdsPageRequest request)
    {
        int resultsToSkip = (request.PageNumber - 1) * request.PageSize;
        List<AdListItemDto> ads = await _adRepository.GetAll()
            .Include(ad => ad.User)
            .Include(ad => ad.Category)
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
            Ads = ads,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = 1
        };

        return response;
    }

    public async Task<UsersScheduleResponse> GetUsersSchedule(UsersScheduleRequest request)
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

        UsersScheduleResponse response = new()
        {
            TutorsSchedule = tutorsSchedule,
            StudentsSchedule = studentsSchedule
        };

        return response;
    }
}
