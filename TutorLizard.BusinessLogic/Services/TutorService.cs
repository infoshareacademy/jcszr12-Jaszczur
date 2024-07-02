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
public class TutorService : ITutorService
{
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;
    private readonly IDbRepository<ScheduleItemRequest> _scheduleItemRequestRepository;
    private readonly IDbRepository<Ad> _adRepository;
    private readonly ILogger<TutorService> _logger;
    private readonly IDbRepository<AdRequest> _adRequestRepository;

    public TutorService(IDbRepository<ScheduleItem> scheduleItemRepository,
                        IDbRepository<ScheduleItemRequest> scheduleItemRequestRepository,
                        IDbRepository<AdRequest> adRequestRepository,
                        IDbRepository<Ad> adRepository,
                        ILogger<TutorService> logger)
    {
        _scheduleItemRepository = scheduleItemRepository;
        _scheduleItemRequestRepository = scheduleItemRequestRepository;
        _adRequestRepository = adRequestRepository;
        _adRepository = adRepository;
        _logger = logger;
    }

    public async Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(IsUserTheAdOwner), request);

        if (request.UserId is null)
        {
            _logger.LogWarning("Requested User is null.");
            IsUserTheAdOwnerResponse userNotFoundResponse = new()
            {
                IsOwner = false
            };
            _logger.LogReturningResponse(userNotFoundResponse);
            return userNotFoundResponse;
        }

        int adId = request.AdId;
        int userId = (int)request.UserId;

        Ad? ad = await _adRepository.GetById(adId);
        bool isOwner = ad != null && ad.TutorId == userId;

        IsUserTheAdOwnerResponse response = new()
        {
            IsOwner = isOwner,
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<CreateScheduleItemResponse> CreateScheduleItem(CreateScheduleItemRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(CreateScheduleItem), request);

        int adId = request.AdId;
        int userId = request.UserId;
        DateTime dateTime = request.DateTime;

        Ad? ad = await _adRepository.GetById(adId);
        bool isOwner = ad != null && ad.TutorId == userId;

        if (!isOwner)
        {
            _logger.LogWarning("User requesting creation of ScheduleItem is not owner of the Ad. ScheduleItem will not be created.");
            CreateScheduleItemResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        var scheduleItem = new ScheduleItem()
        {
            AdId = adId,
            DateTime = dateTime
        };

        await _scheduleItemRepository.Create(scheduleItem);

        CreateScheduleItemResponse response = new()
        {
            Success = true,
            CreatedItemId = scheduleItem.Id
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetTutorsScheduleForAdResponse> GetTutorsScheduleForAd(GetTutorsScheduleForAdRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetTutorsScheduleForAd), request);

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


        GetTutorsScheduleForAdResponse response = new()
        {
            AdId = request.AdId,
            ScheduleItems = scheduleItems
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<AcceptScheduleItemRequestResponse> AcceptScheduleItemRequest(AcceptScheduleItemRequestRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(AcceptScheduleItemRequest), request);

        bool isOwner = await _scheduleItemRequestRepository.GetAll()
            .Where(r => r.Id == request.ScheduleItemRequestId)
            .Select(r => r.ScheduleItem.Ad.TutorId == request.TutorId)
            .FirstOrDefaultAsync();

        if (isOwner == false)
        {
            _logger.LogWarning("User requesting is not owner of the Ad.");
            AcceptScheduleItemRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        var updated = await _scheduleItemRequestRepository.Update(request.ScheduleItemRequestId, r =>
        {
            r.IsAccepted = true;
        });

        if (updated is null)
        {
            _logger.LogWarning("Requested ScheduleItemRequest to accept not found.");
            AcceptScheduleItemRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        AcceptScheduleItemRequestResponse response = new()
        {
            Success = true
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<UnacceptScheduleItemRequestResponse> UnacceptScheduleItemRequest(UnacceptScheduleItemRequestRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(UnacceptScheduleItemRequest), request);

        bool isOwner = await _scheduleItemRequestRepository.GetAll()
            .Where(r => r.Id == request.ScheduleItemRequestId)
            .Select(r => r.ScheduleItem.Ad.TutorId == request.TutorId)
            .FirstOrDefaultAsync();

        if (isOwner == false)
        {
            _logger.LogWarning("User requesting is not owner of the Ad.");
            UnacceptScheduleItemRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        var updated = await _scheduleItemRequestRepository.Update(request.ScheduleItemRequestId, r =>
        {
            r.IsAccepted = false;
        });

        if (updated is null)
        {
            _logger.LogWarning("Requested ScheduleItemRequest to unaccept not found.");
            UnacceptScheduleItemRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        UnacceptScheduleItemRequestResponse response = new()
        {
            Success = true
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetTutorsAllAdRequestsResponse> GetTutorsAllAdRequests(GetTutorsAllAdRequestsRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetTutorsAllAdRequests), request);

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

        GetTutorsAllAdRequestsResponse response = new()
        {
            AdRequests = adRequestsList
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetTutorsPendingAdRequestsResponse> GetTutorsPendingAdRequests(GetTutorsPendingAdRequestsRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetTutorsPendingAdRequests), request);

        List <AdRequestsListDto> adRequests = await _adRequestRepository.GetAll()
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

        GetTutorsPendingAdRequestsResponse response = new()
        {
            AdRequests = adRequests
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<UpdateAdRequestResponse> UpdateAdRequest(UpdateAdRequestRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(UpdateAdRequest), request);

        var adRequest = await _adRequestRepository
            .Update(request.AdRequestId, entity =>
            {
                entity.ReviewDate = DateTime.Now;
                entity.ReplyMessage = request.ReplyMessage;
            });

        if (request.Action == UpdateAdRequestRequest.UpdateAction.Accept)
        {
            adRequest = await _adRequestRepository
                .Update(request.AdRequestId, entity => entity.IsAccepted = true);
        }

        if (adRequest is null)
        {
            _logger.LogWarning("Requested AdRequest to update not found.");
            UpdateAdRequestResponse failedResponse = new()
            {
                IsSuccessful = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        UpdateAdRequestResponse response = new()
        {
            IsSuccessful = true
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetTutorsAdsResponse> GetTutorsAds(GetTutorsAdsRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetTutorsAds), request);

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

        GetTutorsAdsResponse response = new()
        {
            AdList = adListDtos
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<CreateAdResponse> CrateAd(CreateAdRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(CrateAd), request);

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
        catch (Exception ex)
        {
            _logger.LogError("Caught exception while creating Ad: {@Exception}", ex);
            CreateAdResponse failedResponse = new()
            {
                SuccessfullyCreated = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        CreateAdResponse response = new()
        {
            SuccessfullyCreated = true,
            CreatedAdId = toCreate.Id
        };
        _logger.LogReturningResponse(response);
        return response;
    }
}
