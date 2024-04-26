using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService : ITutorService
{
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;

    public TutorService(IDbRepository<ScheduleItem> scheduleItemRepository)
    {
        _scheduleItemRepository = scheduleItemRepository;
    }

    public Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request)
    {
        // TODO add logic (this is only for tests)
        var response = new IsUserTheAdOwnerResponse
        {
            IsOwner = true
        };
        return Task.FromResult(response);
    }

    public async Task<TutorsScheduleForAdResponse> GetTutorsScheduleForAd(TutorsScheduleForAdRequest request)
    {
        List<TutorsScheduleItemDto> scheduleItems = await _scheduleItemRepository.GetAll()
            .Where(item => item.AdId == request.AdId)
            .Select(item =>
                new TutorsScheduleItemDto()
                {
                    Id = item.Id,
                    AdId = item.AdId,
                    DateTime = item.DateTime,
                    Requests = item.ScheduleItemRequests.Select(request =>
                        new TutorsScheduleItemRequestDto()
                        {
                            Id = request.Id,
                            StudentName = request.User.Name,
                            StudentId = request.StudentId,
                            IsAccepted = request.IsAccepted,
                            IsRemote = request.IsRemote,
                            DateCreated = request.DateCreated,
                            CanBeAccepted = item.ScheduleItemRequests.Any(r => r.IsAccepted) == false
                        }).ToList()
                }
            )
            .ToListAsync();


        TutorsScheduleForAdResponse response = new()
        {
            AdId = request.AdId,
            ScheduleItems = scheduleItems
        };

        return response;
    }
}
