using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.BusinessLogic.Interfaces.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService : ITutorService
{
    // TODO inject needed repositories
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;
    private readonly IDbRepository<Ad> _adRepository;
    public TutorService(IDbRepository<ScheduleItem> scheduleItemRepository,
                        IDbRepository<Ad> adRepository)
    {
        _scheduleItemRepository = scheduleItemRepository;
        _adRepository = adRepository;
    }

    public async Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request)
    {
        int adId = request.AdId;
        int userId = (int)request.UserId;

        Ad? ad = await _adRepository.GetById(adId);
        bool isOwner = ad != null && ad.TutorId == userId;

        return new IsUserTheAdOwnerResponse
        {
            IsOwner = isOwner,
        };
    }

    public async Task<CreateScheduleItemResponse> CreateItem(CreateScheduleItemRequest request)
    {
        int adId = request.AdId;
        int userId = request.UserId;
        DateTime dateTime = request.DateTime;

        Ad? ad = await _adRepository.GetById(adId);
        bool isOwner = ad != null && ad.TutorId == userId;

        if (!isOwner)
        {
            return new CreateScheduleItemResponse
            {
                Success = false
            };
        }

        var lastScheduleItem = await _scheduleItemRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        int lastItemId = lastScheduleItem?.Id ?? 0;

        int createdItemId = lastItemId + 1;

        var scheduleItem = new ScheduleItem(createdItemId, adId, dateTime);

        await _scheduleItemRepository.Create(scheduleItem);

        return new CreateScheduleItemResponse
        {
            Success = true,
            CreatedItemId = createdItemId
        };
    }
}
