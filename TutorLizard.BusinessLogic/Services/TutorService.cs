using Azure;
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
    private readonly IDbRepository<ScheduleItemRequest> _scheduleItemRequestRepository;
    private readonly IDbRepository<AdRequest> _adRequestRepository;

    public TutorService(IDbRepository<ScheduleItem> scheduleItemRepository,
                        IDbRepository<ScheduleItemRequest> scheduleItemRequestRepository,
                        IDbRepository<AdRequest> adRequestRepository)
    {
        _scheduleItemRepository = scheduleItemRepository;
        _scheduleItemRequestRepository = scheduleItemRequestRepository;
        _adRequestRepository = adRequestRepository;
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

    public async Task<TutorAllAdRequestsResponse> ViewAllAdRequests(TutorAllAdRequestsRequest request)
    {
        List<AdRequest> adRequests = await _adRequestRepository.GetAll()
            .Include(adrequest => adrequest.Ad)
            .ThenInclude(ad => ad.Category)
            .Where(adrequest => adrequest.Ad.TutorId == request.TutorId)
            .ToListAsync();

        TutorAllAdRequestsResponse response = new()
        {
            AdRequests = adRequests
                        .Select(adrequest => new AdRequestsListDto(adrequest.Id,
                                                       adrequest.StudentId,
                                                       adrequest.AdId,
                                                       adrequest.IsAccepted,
                                                       adrequest.Message,
                                                       adrequest.ReplyMessage,
                                                       adrequest.IsRemote,
                                                       adrequest.Ad.Title,
                                                       adrequest.Ad.Subject,
                                                       adrequest.Ad.Category.Name))
                        .ToList()
        };


        return response;
    }

    public async Task<TutorsPendingAdRequestsResponse> ViewAllPendingAdRequests(TutorsPendingAdRequestsRequest request)
    {
        List<AdRequestsListDto> adRequests = await _adRequestRepository.GetAll()
            .Where(adrequest => 
                adrequest.Ad.TutorId == request.TutorId &&
                adrequest.ReviewDate == null &&
                adrequest.IsAccepted == false)
            .Include(adrequest => adrequest.Ad)
            .ThenInclude(ad => ad.Category)
            .Select(adrequest => new AdRequestsListDto(adrequest.Id,
                                                       adrequest.StudentId,
                                                       adrequest.AdId,
                                                       adrequest.IsAccepted,
                                                       adrequest.Message,
                                                       adrequest.ReplyMessage,
                                                       adrequest.IsRemote,
                                                       adrequest.Ad.Title,
                                                       adrequest.Ad.Subject,
                                                       adrequest.Ad.Category.Name))
            .ToListAsync();

        TutorsPendingAdRequestsResponse response = new()
        {
            AdRequests = adRequests
        };

        return response;
    }

    public async Task<UpdateTutorsPendingAdRequestResponse> UpdateAdRequest(UpdateTutorsPendingAdRequestRequest request)
    {
        var adRequest = await _adRequestRepository
            .Update(request.AdRequestId, entity =>
            {
                 entity.ReviewDate = DateTime.Now;
                 entity.ReplyMessage = request.ReplyMessage;
            });

        if (request.Action == UpdateTutorsPendingAdRequestRequest.UpdateAction.Accept)
        {
            adRequest = await _adRequestRepository
                .Update(request.AdRequestId, entity => entity.IsAccepted = true);
        }

        UpdateTutorsPendingAdRequestResponse response = new();

        if (adRequest is null)
            response.IsSuccessful = false;
        else
            response.IsSuccessful = true;

        return response;
    }
}
