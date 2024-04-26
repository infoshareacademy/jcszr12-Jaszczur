using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
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
    private readonly IDbRepository<ScheduleItemRequest> _scheduleItemRequestRepository;
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;
    private readonly IDbRepository<Ad> _adRepository;
    public TutorService(IDbRepository<ScheduleItem> scheduleItemRepository,
                        IDbRepository<ScheduleItemRequest> scheduleItemRequestRepository,
                        IDbRepository<Ad> adRepository)
    {
        _scheduleItemRepository = scheduleItemRepository;
        _scheduleItemRequestRepository = scheduleItemRequestRepository;
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

    public async Task<AcceptScheduleItemRequestResponse> AcceptScheduleItemRequest(AcceptScheduleItemRequestRequest request)
    {
        bool isOwner = await _scheduleItemRequestRepository.GetAll()
            .Where(r => r.Id == request.ScheduleItemRequestId)
            .Select(r => r.ScheduleItem.Ad.TutorId == request.TutorId)
            .FirstOrDefaultAsync();

        if (isOwner == false)
        {
            return new()
            {
                Success = false
            };
        }

        var updated = await _scheduleItemRequestRepository.Update(request.ScheduleItemRequestId, r =>
        {
            r.IsAccepted = true;
        });

        if (updated is null)
        {
            return new()
            {
                Success = false
            };
        }

        return new()
        {
            Success = true
        };
    }

    public async Task<UnacceptScheduleItemRequestResponse> UnacceptScheduleItemRequest(UnacceptScheduleItemRequestRequest request)
    {
        bool isOwner = await _scheduleItemRequestRepository.GetAll()
            .Where(r => r.Id == request.ScheduleItemRequestId)
            .Select(r => r.ScheduleItem.Ad.TutorId == request.TutorId)
            .FirstOrDefaultAsync();

        if (isOwner == false)
        {
            return new()
            {
                Success = false
            };
        }

        var updated = await _scheduleItemRequestRepository.Update(request.ScheduleItemRequestId, r =>
        {
            r.IsAccepted = false;
        });

        if (updated is null)
        {
            return new()
            {
                Success = false
            };
        }

        return new()
        {
            Success = true
        };
    }
}
