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
public class StudentService : IStudentService
{
    private readonly IDbRepository<Ad> _adRepository;
    private readonly IDbRepository<AdRequest> _adRequestRepository;
    private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;
    private readonly IDbRepository<ScheduleItemRequest> _scheduleItemRequestRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IDbRepository<Ad> adRepository,
                          IDbRepository<AdRequest> adRequestRepository,
                          IDbRepository<ScheduleItem> scheduleItemRepository,
                          IDbRepository<ScheduleItemRequest> scheduleItemRequestRepository,
                          ILogger<StudentService> logger)
    {
        _adRepository = adRepository;
        _adRequestRepository = adRequestRepository;
        _scheduleItemRepository = scheduleItemRepository;
        _scheduleItemRequestRepository = scheduleItemRequestRepository;
        _logger = logger;
    }

    #region Schedule
    public async Task<CreateScheduleItemRequestResponse> CreateScheduleItemRequest(CreateScheduleItemRequestRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(CreateScheduleItemRequest), request);

        int studentId = request.StudentId;
        int scheduleItemId = request.ScheduleItemId;

        ScheduleItem? scheduleItem = await _scheduleItemRepository.GetById(scheduleItemId);

        bool isOwner = _scheduleItemRepository.GetAll()
            .Any(si => si.Id == request.ScheduleItemId && si.Ad.TutorId == request.StudentId);

        if (isOwner)
        {
            _logger.LogWarning("User requesting creation of ScheduleItemRequest is owner of the Ad. ScheduleItemRequest will not be created.");
            CreateScheduleItemRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        var scheduleItemRequest = new ScheduleItemRequest()
        {
            ScheduleItemId = scheduleItemId,
            DateCreated = DateTime.UtcNow,
            StudentId = studentId,
            IsAccepted = false,
            IsRemote = request.IsRemote
        };

        await _scheduleItemRequestRepository.Create(scheduleItemRequest);

        CreateScheduleItemRequestResponse response = new()
        {
            Success = true,
            CreatedScheduleItemRequestId = scheduleItemRequest.Id
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetAvailableScheduleForAdResponse> GetAvailableScheduleForAd(GetAvailableScheduleForAdRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetAvailableScheduleForAd), request);

        List<ScheduleItemDto> items = await _scheduleItemRepository.GetAll()
            .Where(si => si.Ad.AdRequests.Any(ar => ar.StudentId == request.StudentId && ar.IsAccepted) && si.AdId == request.AdId)
            .Select(si => new ScheduleItemDto()
            {
                AdId = si.AdId,
                DateTime = si.DateTime,
                Id = si.Id,
                Status = si.ScheduleItemRequests.Any(sir => sir.StudentId == request.StudentId && sir.IsAccepted) ? ScheduleItemDto.ScheduleItemRequestStatus.Accepted
                    : si.ScheduleItemRequests.Any(sir => sir.StudentId == request.StudentId) ? ScheduleItemDto.ScheduleItemRequestStatus.Pending
                    : ScheduleItemDto.ScheduleItemRequestStatus.RequestNotSent
            })
            .ToListAsync();

        bool isAccepted = await _adRequestRepository.GetAll()
            .Where(ar => ar.AdId == request.AdId)
            .AnyAsync(ar => ar.StudentId == request.StudentId && ar.IsAccepted);

        bool isRemote = await _adRepository.GetAll()
            .Where(ad => ad.Id == request.AdId)
            .Select(ad => ad.IsRemote)
            .FirstOrDefaultAsync();

        GetAvailableScheduleForAdResponse response = new()
        {
            AdId = request.AdId,
            IsAccepted = isAccepted,
            IsRemote = isRemote,
            Items = items
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    #endregion
    #region Ads
    public async Task<GetStudentsAcceptedAdsResponse> GetStudentsAcceptedAds(GetStudentsAcceptedAdsRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetStudentsAcceptedAds), request);

        var studentId = request.StudentId;

        var acceptedAdRequests = await _adRequestRepository.GetAll()
            .Include(ar => ar.Ad)
            .ThenInclude(ad => ad.Category)
            .Include(ar => ar.Ad)
            .ThenInclude(Ad => Ad.User)
            .Where(ar => ar.StudentId == studentId && ar.IsAccepted)
            .ToListAsync();

        var acceptedAds = acceptedAdRequests
            .Select(ar => ar.Ad).ToList();

        var adListDtos = acceptedAds
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

        GetStudentsAcceptedAdsResponse response = new()
        {
            Ads = adListDtos,
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetStudentsAdRequestsResponse> GetStudentsAdRequests(GetStudentsAdRequestsRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetStudentsAdRequests), request);

        var studentId = request.StudentId;

        var adRequests = await _adRequestRepository.GetAll()
            .Include(ar => ar.Ad)
            .ThenInclude(ad => ad.Category)
            .Where(ar => ar.StudentId == studentId)
            .ToListAsync();

        var adRequestsListDtos = adRequests
            .Select(ar => new AdRequestsListDto
            {
                Id = ar.Id,
                AdId = ar.AdId,
                StudentId = ar.StudentId,
                Message = ar.Message,
                IsRemote = ar.IsRemote,
                IsAccepted = ar.IsAccepted,
                AdSubject = ar.Ad.Subject,
                AdTitle = ar.Ad.Title,
                CategoryName = ar.Ad.Category.Name,
                ReplyMessage = ar.ReplyMessage,
                ReviewDate = ar.ReviewDate
            })
            .ToList();

        GetStudentsAdRequestsResponse response = new()
        {
            AdRequests = adRequestsListDtos
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<GetAdRequestStatusResponse> GetAdRequestStatus(GetAdRequestStatusRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(GetAdRequestStatus), request);

        var adRequestDetails = await _adRequestRepository.GetAll()
            .Include(adrequest => adrequest.Ad)
            .Where(adrequest => adrequest.StudentId == request.StudentId
                  && adrequest.Ad.Id == adrequest.AdId
                  && adrequest.IsAccepted == false)
            .OrderByDescending(adrequest => adrequest.DateCreated)
            .FirstOrDefaultAsync();

        if (adRequestDetails is null)
        {
            _logger.LogWarning("Requested Ad not found.");
            GetAdRequestStatusResponse failedResponse = new()
            {
                IsSuccessful = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        GetAdRequestStatusResponse response = new GetAdRequestStatusResponse()
        {
            Id = adRequestDetails.Id,
            AdId = adRequestDetails.AdId,
            Message = adRequestDetails.Message,
            ReplyMessage = adRequestDetails.ReplyMessage,
            DateCreated = adRequestDetails.DateCreated,
            ReviewDate = adRequestDetails.ReviewDate,
            Status = adRequestDetails.ReviewDate == null ? GetAdRequestStatusResponse.RequestStatus.Pending : GetAdRequestStatusResponse.RequestStatus.Rejected,
            IsSuccessful = true
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<DeleteAdRequestResponse> DeleteAdRequest(DeleteAdRequestRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(DeleteAdRequest), request);

        var deletedAdRequest = await _adRequestRepository.Delete(request.Id);

        if (deletedAdRequest is null)
        {
            _logger.LogWarning("Requested Ad not found.");
            DeleteAdRequestResponse failedResponse = new()
            {
                IsSuccessful = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        DeleteAdRequestResponse response = new()
        {
            IsSuccessful = true
        };
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<CreateAdRequestResponse> CreateAdRequest(CreateAdRequestRequest request)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(CreateAdRequest), request);

        Ad? ad = await _adRepository.GetById(request.AdId);

        if (ad is null)
        {
            _logger.LogWarning("Creating AdRequest unsuccessful.");
            CreateAdRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        bool userIsOwnerOfAd = ad.TutorId == request.StudentId;

        if (userIsOwnerOfAd)
        {
            _logger.LogWarning("User requesting creation of AdRequest is owner of the Ad. AdRequest will not be created.");
            CreateAdRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        var userAlreadySentRequest = await _adRequestRepository.GetAll()
            .Where(r => r.AdId == request.AdId)
            .AnyAsync(r => r.StudentId == request.StudentId);

        if (userAlreadySentRequest)
        {
            _logger.LogWarning("AdRequest already sent. AdRequest will not be created.");
            CreateAdRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        AdRequest toCreate = new()
        {
            AdId = request.AdId,
            IsAccepted = false,
            IsRemote = request.IsRemote,
            Message = request.Message,
            StudentId = request.StudentId
        };

        try
        {
            await _adRequestRepository.Create(toCreate);
        }
        catch (Exception ex)
        {
            _logger.LogError("Caught exception while creating AdRequest: {@Exception}", ex);
            CreateAdRequestResponse failedResponse = new()
            {
                Success = false
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        CreateAdRequestResponse response = new()
        {
            Success = true
        };
        _logger.LogReturningResponse(response);
        return response;
    }
}
#endregion