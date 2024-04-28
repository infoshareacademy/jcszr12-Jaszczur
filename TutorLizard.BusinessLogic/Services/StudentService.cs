using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Services;
public class StudentService : IStudentService
{
    private readonly IDbRepository<Ad> _adRepository;
    private readonly IDbRepository<AdRequest> _adRequestRepository;
    public StudentService(IDbRepository<Ad> adRepository,
                          IDbRepository<AdRequest> adRequestRepository)
    {
        _adRepository = adRepository;
        _adRequestRepository = adRequestRepository;
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
                ReplyMessage = ar.ReplyMessage
            })
            .ToList();

        return new StudentsAdRequestsResponse
        {
            AdRequests = adRequestsListDtos
        };
    }

    public async Task<AdRequestStatusResponse> ViewPendingAdRequest(AdRequestStatusRequest request)
    {
        var adRequestDetails = await _adRequestRepository.GetAll()
            .Include(adrequest => adrequest.Ad)
            .Where(adrequest => adrequest.StudentId == request.StudentId
                  && adrequest.Ad.Id == adrequest.AdId
                  && adrequest.IsAccepted == false)
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
            Status = adRequestDetails.ReviewDate != null ? AdRequestStatusResponse.RequestStatus.Pending : AdRequestStatusResponse.RequestStatus.Rejected
        };

        return response;
    }
}
