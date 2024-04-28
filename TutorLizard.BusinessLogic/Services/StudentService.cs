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
                ReplyMessage = ar.ReplyMessage,
                ReviewDate = ar.ReviewDate
            })
            .ToList();

        return new StudentsAdRequestsResponse
        {
            AdRequests = adRequestsListDtos
        };

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
}
