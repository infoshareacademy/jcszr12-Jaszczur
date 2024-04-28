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
    private readonly IDbRepository<Ad> _adRepository;
    private readonly IDbRepository<AdRequest> _adRequestRepository;

    public TutorService(IDbRepository<ScheduleItem> scheduleItemRepository,
                        IDbRepository<ScheduleItemRequest> scheduleItemRequestRepository,
                        IDbRepository<AdRequest> adRequestRepository,
                        IDbRepository<Ad> adRepository)
    {
        _scheduleItemRepository = scheduleItemRepository;
        _scheduleItemRequestRepository = scheduleItemRequestRepository;
        _adRepository = adRepository;
    }

    public async Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request)
    {
        if (request.UserId is null)
        {
            return new IsUserTheAdOwnerResponse
            {
                IsOwner = false
            };
        }

        int adId = request.AdId;
        int userId = (int)request.UserId;

        Ad? ad = await _adRepository.GetById(adId);
        bool isOwner = ad != null && ad.TutorId == userId;

        return new IsUserTheAdOwnerResponse
        {
            IsOwner = isOwner,
        };
    }

    public async Task<CreateScheduleItemResponse> CreateScheduleItem(CreateScheduleItemRequest request)
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

        var scheduleItem = new ScheduleItem()
        {
            AdId = adId,
            DateTime = dateTime
        };

        await _scheduleItemRepository.Create(scheduleItem);

        return new CreateScheduleItemResponse
        {
            Success = true,
            CreatedItemId = scheduleItem.Id
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

    public async Task<TutorAllAdRequestsResponse> ViewAllAdRequests(TutorAllAdRequestsRequest request)
    {
        List<AdRequestsListDto> adRequestsList = await _adRequestRepository.GetAll()
            .Include(adrequest => adrequest.Ad)
            .ThenInclude(ad => ad.Category)
            .Where(adrequest => adrequest.Ad.TutorId == request.TutorId)
            .Select(adrequest => new AdRequestsListDto(adrequest.Id,
                                                       adrequest.StudentId,
                                                       adrequest.AdId,
                                                       adrequest.IsAccepted,
                                                       adrequest.Message,
                                                       adrequest.ReplyMessage,
                                                       adrequest.IsRemote,
                                                       adrequest.Ad.Title,
                                                       adrequest.Ad.Subject,
                                                       adrequest.Ad.Category.Name,
                                                       adrequest.ReviewDate))
            .ToListAsync();

        TutorAllAdRequestsResponse response = new()
        {
            AdRequests = adRequestsList
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
                                                       adrequest.Ad.Category.Name,
                                                       adrequest.ReviewDate))
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

    public async Task<TutorsAdsResponse> ViewTutorsAds(TutorsAdsRequest request)
    {
        var tutorId = request.TutorId;

        var tutorsAds = await _adRepository.GetAll()
            .Include(ad => ad.User)
            .Include(ad => ad.Category)
            .Where(ad => ad.TutorId == tutorId)
            .ToListAsync();

        var adListDtos = tutorsAds
            .Select(ad => new AdListItemDto
            {
                Id = ad.Id,
                TutorId = ad.TutorId,
                Subject = ad.Subject,
                Title = ad.Title,
                Description = ad.Description,
                CategoryId = ad.CategoryId,
                Price = ad.Price,
                Location = ad.Location,
                IsRemote = ad.IsRemote,
                CategoryName = ad.Category.Name,
                TutorName = ad.User.Name
            })
            .ToList();

        return new TutorsAdsResponse
        {
            AdList = adListDtos
        };
    }

    public async Task<CreateAdResponse> CrateAd(CreateAdRequest request)
    {
        Ad toCreate = new()
        {
            TutorId = request.TutorId,
            Subject = request.Subject,
            Title = request.Subject,
            Description = request.Description,
            CategoryId = request.CategoryId,
            Price = request.Price,
            Location = request.Location,
            IsRemote = request.IsRemote
        };

        try
        {
            await _adRepository.Create(toCreate);
        }
        catch
        {
            return new()
            {
                SuccessfullyCreated = false
            };
        }

        return new()
        {
            SuccessfullyCreated = true,
            CreatedAdId = toCreate.Id
        };
    }
}
