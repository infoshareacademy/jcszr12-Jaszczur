using Microsoft.EntityFrameworkCore;
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
    public StudentService(IDbRepository<Ad> adRepository,
                          IDbRepository<AdRequest> adRequestRepository,
                          IDbRepository<ScheduleItem> scheduleItemRepository,
                          IDbRepository<ScheduleItemRequest> scheduleItemRequestRepository)
    {
        _adRepository = adRepository;
        _adRequestRepository = adRequestRepository;
        _scheduleItemRepository = scheduleItemRepository;
        _scheduleItemRequestRepository = scheduleItemRequestRepository;
    }

    public async Task<CreateScheduleItemRequestResponse> CreateScheduleItemRequest(CreateScheduleItemRequestRequest request)
    {
        int studentId = request.StudentId;
        int scheduleItemId = request.ScheduleItemId;

        ScheduleItem? scheduleItem = await _scheduleItemRepository.GetById(scheduleItemId);

        bool isOwner = _scheduleItemRepository.GetAll()
            .Any(si => si.Id == request.ScheduleItemId && si.Ad.TutorId == request.StudentId);

        if (isOwner)
        {
            return new CreateScheduleItemRequestResponse
            {
                Success = false
            };
        }

        var scheduleItemRequest = new ScheduleItemRequest()
        {
            ScheduleItemId = scheduleItemId,
            DateCreated = DateTime.UtcNow,
            StudentId = studentId,
            IsAccepted = false
        };

        await _scheduleItemRequestRepository.Create(scheduleItemRequest);

        return new CreateScheduleItemRequestResponse 
        { 
            Success = true,
            CreatedScheduleItemRequestId = scheduleItemRequest.Id,
        };
    }

    public async Task<StudentsAcceptedAdsResponse> ViewAcceptedAds(StudentsAcceptedAdsRequest request)
    {
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

        return new StudentsAcceptedAdsResponse
        {
            Ads = adListDtos,
        };
    }

    public async Task<StudentsAdRequestsResponse> ViewAdRequests(StudentsAdRequestsRequest request)
    {
        var studentId = request.StudentId;

        var adRequests = await _adRequestRepository.GetAll()
            .Include(ar => ar.Ad)
            .ThenInclude(ad => ad.Category)
            .Where(ar => ar.StudentId == studentId && ar.DateCreated != null)
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

        return new StudentsAdRequestsResponse
        {
            AdRequests = adRequestsListDtos
        };
    }

    public async Task<AdRequestStatusResponse> ViewAdRequestStatus(AdRequestStatusRequest request)
    {
        var adRequestDetails = await _adRequestRepository.GetAll()
            .Include(adrequest => adrequest.Ad)
            .Where(adrequest => adrequest.StudentId == request.StudentId
                  && adrequest.Ad.Id == adrequest.AdId
                  && adrequest.IsAccepted == false)
            .OrderByDescending(adrequest => adrequest.DateCreated)
            .FirstOrDefaultAsync();

        if (adRequestDetails is null)
            return new AdRequestStatusResponse() { IsSuccessful = false }; 

        AdRequestStatusResponse response = new AdRequestStatusResponse()
        {
            Id = adRequestDetails.Id,
            AdId = adRequestDetails.AdId,
            Message = adRequestDetails.Message,
            ReplyMessage = adRequestDetails.ReplyMessage,
            DateCreated = adRequestDetails.DateCreated,
            ReviewDate = adRequestDetails.ReviewDate,
            Status = adRequestDetails.ReviewDate == null ? AdRequestStatusResponse.RequestStatus.Pending : AdRequestStatusResponse.RequestStatus.Rejected,
            IsSuccessful = true
        };

        return response;
    }

    public async Task<StudentCancelAdRequestResponse> DeleteAdRequest(StudentCancelAdRequestRequest request)
    {
        var deletedAdRequest = await _adRequestRepository.Delete(request.Id);

        StudentCancelAdRequestResponse response = new StudentCancelAdRequestResponse();
        if (deletedAdRequest == null)
            response.IsSuccessful = true;
        else
            response.IsSuccessful = false;

        return response;
    }

    public async Task<CreateAdRequestResponse> CreateAdRequest(CreateAdRequestRequest request)
    {
        Ad? ad = await _adRepository.GetById(request.AdId);

        if (ad is null)
        {
            return new()
            {
                Success = false
            };
        }

        bool userIsOwnerOfAd = ad.TutorId == request.StudentId;

        if (userIsOwnerOfAd)
        {
            return new()
            {
                Success = false
            };
        }

        var userAlreadySentRequest = await _adRequestRepository.GetAll()
            .Where(r => r.AdId == request.AdId)
            .AnyAsync(r => r.StudentId == request.StudentId);

        if (userAlreadySentRequest)
        {
            return new()
            {
                Success = false
            };
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
        catch
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

    public async Task<AvailableScheduleForAdResponse> GetAvailableScheduleForAd(AvailableScheduleForAdRequest request)
    {
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

        AvailableScheduleForAdResponse response = new()
        {
            AdId = request.AdId,
            IsAccepted = isAccepted,
            Items = items
        };

        return response;
    }
}
